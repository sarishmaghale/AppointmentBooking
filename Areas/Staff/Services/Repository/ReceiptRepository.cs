using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class ReceiptRepository : IReceiptRepository

    {
        private MedicareAppointmentDbContext db;
        public ReceiptRepository(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }

        public async Task<bool> AddCashReceipt(ReceiptViewModel model)
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
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }
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

        public async Task<ReceiptViewModel> ReceiptDetails(int ReceiptNo)
        {
          var data=  await (from receipts in db.TblCashReceipts
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
    }
}
