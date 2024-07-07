using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class ReceiptService : IReceiptRepository

    {
        private MedicareAppointmentDbContext db;
        public ReceiptService(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddCashReceipt(ReceiptViewModel model)
        {
           using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TblCashReceipt cashReceipt = new TblCashReceipt()
                    {
                        Uhid = model.Uhid,
                        Opdno = model.Opdno,
                        Ipdno = model.Ipdno,
                        PayType = model.PayType,
                        CreatedDate = model.CreatedDate,
                        TotalAmount = model.TotalAmount,
                        CreatedByUser = model.CreatedByUser,
                    };
                    await db.TblCashReceipts.AddAsync(cashReceipt);
                    await db.SaveChangesAsync();
                   foreach(var item in model.ReceiptDetails)
                    {
                        TblReceiptDetail details = new TblReceiptDetail()
                        {
                            ReceiptNo = cashReceipt.ReceiptNo,
                            TestGroup = item.TestGroup,
                            TestName = item.TestName,
                            TestPrice = item.TestPrice,
                            Quantity = item.Quantity,
                            Amount = item.Amount,
                        };
                       await db.TblReceiptDetails.AddAsync(details);
                        await db.SaveChangesAsync();
                    }
                    transaction.Commit();
                    return cashReceipt.ReceiptNo;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        public async Task<List<ReceiptViewModel>> GetFilterCashSummary(ReceiptViewModel model)
        {
            var data = from receipts in db.TblCashReceipts
                       join details in db.TblReceiptDetails on receipts.ReceiptNo equals details.ReceiptNo
                       join opd in db.TblOpdregistrations on receipts.Opdno equals opd.SrNo into opdJoin
                       from opd in opdJoin.DefaultIfEmpty()
                       join ipd in db.TblIpdregistrations on receipts.Ipdno equals ipd.IpdregNo into ipdJoin
                       from ipd in ipdJoin.DefaultIfEmpty()
                       select new
                       {
                            receipts.PayType,
                            receipts.CreatedDate,
                           details.TestGroup,
                           details.TestName,
                           receipts.ReceiptNo,
                           receipts.Opdno,
                           receipts.TotalAmount,
                           details.Amount,
                           PatientName = receipts.Opdno != null ? (opd.FirstName + " " + opd.LastName) : (ipd.FirstName + " " + ipd.LastName),
                           ConsultantDr = receipts.Opdno != null ? opd.ConsultantDr : ipd.ConsultantDr,                         
                           receipts.CreatedByUser,
                        };
            var query = data.AsQueryable();
            string testGroup = db.TblTestGroupSetups.Where(x => x.TestGroupId == model.GroupFilter).Select(x => x.TestGroupName).FirstOrDefault() ?? "all";
            string? user = model.UserFilter;
            string? payType = model.PayTypeFilter;
            DateTime? fromDate = model.FromDateFilter;
            DateTime? toDate = model.ToDateFilter;
            if (!string.IsNullOrEmpty(testGroup) && !testGroup.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.TestGroup.Contains(testGroup));
            }

            if (!string.IsNullOrEmpty(payType) && !payType.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.PayType.Contains(payType));
            }
            if (!string.IsNullOrEmpty(user) && !user.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(t => t.CreatedByUser.Contains(user));
            }
            if (fromDate != null && toDate != null)
            {
                // Filter by date range
                query = query.Where(t => t.CreatedDate >= fromDate && t.CreatedDate <= toDate);
            }    
            var results = await query.GroupBy(t=> t.TestGroup).Select(g=> new ReceiptViewModel
            {
                TestGroupDept = g.Key,
                Receipts = g.Select(t => new ReceiptViewModel
                {
                    ReceiptNo = t.ReceiptNo,
                    PayType = t.PayType,
                    PatientName = t.PatientName,
                    TotalAmount = t.Amount,
                    DoctorName = db.TblDoctorSetups.Where(d => d.DoctorId == t.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault(),
                    CreatedDate = t.CreatedDate,
                }).ToList(),
                ReceiptTotal=g.Sum(t=>t.Amount)
            }).ToListAsync();
            return results;
        }

        public async Task<double> GetTestPrice(int TestId)
        {
            var price =await db.TblTestSetups.Where(x => x.TestId == TestId).Select(x => x.TestPrice).FirstOrDefaultAsync();
            return (double)price;
        }

        public async Task<IEnumerable<ReceiptDetails>> GetTestsDataOnTestGroup(int TestGroupId)
        {
            var result = await (from test in db.TblTestSetups
                                                 join testgroup in db.TblTestGroupSetups
                                                 on test.TestGroupId equals testgroup.TestGroupId
                                                 where test.TestGroupId == TestGroupId
                                                 select new ReceiptDetails
                                                 {
                                                     TestId = test.TestId,
                                                    TestName=test.TestName,
                                                    TestPrice=test.TestPrice,
                                                    TestGroupName=testgroup.TestGroupName,                                                   
                                                 }).ToListAsync();

            return result;

        }

        public async Task<ReceiptViewModel> GetReceiptDetails(int ReceiptNo)
        {
            var checkOPD = await db.TblCashReceipts.Where(c => c.ReceiptNo == ReceiptNo).Select(c => c.Opdno).FirstOrDefaultAsync();
            if (checkOPD != null)
            {
                var result = await GetCashReceiptDetails(ReceiptNo);
                return result;
            }
            else
            {
                var result = await GetDischargeReceiptDetails(ReceiptNo);
                return result;
            }



        }


        public async Task<ReceiptViewModel> GetDischargeReceiptDetails(int ReceiptNo)
        {
            var data = await (from receipts in db.TblCashReceipts
                              join ipd in db.TblIpdregistrations on receipts.Ipdno equals ipd.IpdregNo
                              join details in db.TblReceiptDetails on receipts.ReceiptNo equals details.ReceiptNo
                              where receipts.ReceiptNo == ReceiptNo
                              select new
                              {
                                  receipts.ReceiptNo,
                                  receipts.TotalAmount,
                                  receipts.CreatedDate,
                                  ipd.FirstName,
                                  ipd.LastName,
                                  ipd.Address,
                                  ipd.Age,
                                  ipd.AgeType,
                                  ipd.ConsultantDr,
                                  ReceiptDetails = new ReceiptDetails
                                  {
                                      TestGroup = details.TestGroup,
                                      TestName = details.TestName,
                                      Quantity = details.Quantity,
                                      TestPrice = details.TestPrice,
                                      Amount = details.Amount
                                  }
                              }).GroupBy(x => new
                              {
                                  x.ReceiptNo,
                                  x.TotalAmount,
                                  x.CreatedDate,
                                  x.FirstName,
                                  x.LastName,
                                  x.Address,
                                  x.Age,
                                  x.AgeType,
                                  x.ConsultantDr,
                              }).Select(y => new ReceiptViewModel
                              {
                                  ReceiptNo = y.Key.ReceiptNo,
                                  PatientName = y.Key.FirstName + " " + y.Key.LastName,
                                  TotalAmount = y.Key.TotalAmount,
                                  CreatedDate = y.Key.CreatedDate,
                                  Address = y.Key.Address,
                                  Age = y.Key.Age + " " + y.Key.AgeType,
                                  DoctorName = db.TblDoctorSetups.Where(a => a.DoctorId == y.Key.ConsultantDr).Select(a => a.DoctorName).FirstOrDefault(),
                                  ReceiptDetails = y.Select(x => x.ReceiptDetails).ToList()
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ReceiptViewModel> GetCashReceiptDetails(int ReceiptNo)
        {
            var data = await (from receipts in db.TblCashReceipts
                              join opd in db.TblOpdregistrations on receipts.Opdno equals opd.SrNo
                              join details in db.TblReceiptDetails on receipts.ReceiptNo equals details.ReceiptNo
                              where receipts.ReceiptNo == ReceiptNo
                              select new
                              {
                                  receipts.ReceiptNo,
                                  receipts.TotalAmount,
                                  receipts.CreatedDate,
                                  opd.FirstName,
                                  opd.LastName,
                                  opd.Address,
                                  opd.Age,
                                  opd.AgeType,
                                  opd.ConsultantDr,
                                  ReceiptDetails = new ReceiptDetails
                                  {
                                      TestGroup = details.TestGroup,
                                      TestName = details.TestName,
                                      Quantity = details.Quantity,
                                      TestPrice = details.TestPrice,
                                      Amount = details.Amount
                                  }
                              }).GroupBy(x => new
                              {
                                  x.ReceiptNo,
                                  x.TotalAmount,
                                  x.CreatedDate,
                                  x.FirstName,
                                  x.LastName,
                                  x.Address,
                                  x.Age,
                                  x.AgeType,
                                  x.ConsultantDr,
                              }).Select(y => new ReceiptViewModel
                              {
                                  ReceiptNo = y.Key.ReceiptNo,
                                  PatientName = y.Key.FirstName + " " + y.Key.LastName,
                                  TotalAmount = y.Key.TotalAmount,
                                  CreatedDate = y.Key.CreatedDate,
                                  Address = y.Key.Address,
                                  Age = y.Key.Age + " " + y.Key.AgeType,
                                  DoctorName = db.TblDoctorSetups.Where(a => a.DoctorId == y.Key.ConsultantDr).Select(a => a.DoctorName).FirstOrDefault(),
                                  ReceiptDetails = y.Select(x => x.ReceiptDetails).ToList()
                              }).FirstOrDefaultAsync();
            return data;
        }
        public async Task<bool> AddDischargeBill(ReceiptViewModel model)
        {
            int ReceiptNo = await AddCashReceipt(model);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {                
                    var ipdPatient = await db.TblIpdregistrations.Where(p => p.IpdregNo == model.Ipdno).FirstOrDefaultAsync();
                    if (ipdPatient != null)
                    {
                        ipdPatient.IsDischarged = 1;
                        db.Entry(ipdPatient).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        var bedNo = ipdPatient.BedNo;
                        var bedStatus = await db.TblIpdbedStatuses.Where(b => b.BedId == bedNo).FirstOrDefaultAsync();
                        if (bedStatus != null)
                        {
                            bedStatus.Status = 0;
                            bedStatus.IpdregNo = 0;
                            db.Entry(bedStatus).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                        return true;
                    }
                    await transaction.RollbackAsync();
                    return false;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> CheckDischargeBill(int IPDRegNo)
        {
            bool result = await db.TblCashReceipts.AnyAsync(r => r.Ipdno == IPDRegNo);
            return result;
        }

        public async Task<bool> AddPatientExpenseEntry(List<ExpenseEntryViewModel> model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model)
                    {
                        var expenseEntryData = new TblIpdexpenseEntry()
                        {
                            Uhid = item.Uhid,
                            IpdregNo = item.IpdregNo,
                            TestGroup = item.TestGroup,
                            TestName = item.TestName,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Amount = item.Amount,
                            CreatedDate = item.CreatedDate
                        };
                        await db.TblIpdexpenseEntries.AddAsync(expenseEntryData);
                        await db.SaveChangesAsync();
                        
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
                
        }
    }
}
