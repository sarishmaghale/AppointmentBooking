using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage;
using Irony.Parsing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class PathologyService : IPathologyRepository
    {
        private HospitalManagementDbContext db;
        public PathologyService(HospitalManagementDbContext _db)
        {
            db = _db;
        }
        public int GetMaxLabNo()
        {
            int maxLabNo =  Convert.ToInt32(db.TblSampleRegistrations.Max(item => item.LabNo));
            return  (maxLabNo+1);
        }
        public async Task<PathologyViewModel> GetLabPatientInfo(int SrNo)
        {
            var checkData = await db.TblSampleRegistrations.Where(x => x.SrNo == SrNo).Select(x => x.RecordType).FirstOrDefaultAsync();
            if (checkData == 1)
            {
               
                var data = await (from lab in db.TblSampleRegistrations
                                  join details in db.TblReceiptDetails on lab.RecordNo equals details.ReceiptNo
                                  join receipt in db.TblCashReceipts on  details.ReceiptNo equals receipt.ReceiptNo
                                  join opd in db.TblOpdregistrations on receipt.Opdno equals opd.SrNo
                                  where lab.SrNo == SrNo
                                  select new PathologyViewModel
                                  {
                                      TestName = details.TestName,
                                      SrNo = lab.SrNo,
                                      Uhid= receipt.Uhid,
                                      PatientName= opd.FirstName+""+opd.LastName,
                                      OpdNo= receipt.Opdno,
                                      RecordNo = lab.RecordNo,
                                      CreatedDate=receipt.CreatedDate,
                                      
                                      Age= opd.Age+" "+opd.AgeType,
                                      Gender=opd.Gender,
                                      TestList = (from t in db.TblTestSetups
                                                        join rd in db.TblReceiptDetails on t.TestName equals rd.TestName
                                                        where rd.ReceiptNo == lab.RecordNo && rd.TestGroup=="Pathology"
                                                        select new TestsListViewModel
                                                        {
                                                            TestName = t.TestName,
                                                            TestId = t.TestId
                                                        }).ToList()
                                  }).FirstOrDefaultAsync();
                return data;

            }
            if (checkData == 2)
            {
         
            }
            return null;
        }
        public async Task<PathologyViewModel> GetTestParameter(int LabNo)
        {
            var data = await (from lab in db.TblSampleRegistrations
                              join details in db.TblReceiptDetails on lab.RecordNo equals details.ReceiptNo
                              join receipt in db.TblCashReceipts on details.ReceiptNo equals receipt.ReceiptNo
                              join opd in db.TblOpdregistrations on receipt.Opdno equals opd.SrNo
                              where lab.LabNo == LabNo
                              select new PathologyViewModel
                              {
                                  TestName = details.TestName,
                                  SrNo = lab.SrNo,
                                  Uhid = receipt.Uhid,
                                  PatientName = opd.FirstName + " " + opd.LastName,
                                  OpdNo = receipt.Opdno,
                                  LabNo=lab.LabNo,
                                  RecordNo = lab.RecordNo,
                                  CreatedDate = receipt.CreatedDate,
                                  Age = opd.Age + " " + opd.AgeType,
                                  Gender = opd.Gender,
                                  TestList = (from t in db.TblTestSetups
                                                    join rd in db.TblReceiptDetails on t.TestName equals rd.TestName
                                                    where rd.ReceiptNo == lab.RecordNo && rd.TestGroup == "Pathology"
                                                    select new TestsListViewModel
                                                    {
                                                        TestName = t.TestName,
                                                        TestId = t.TestId,
                                                        TestParameterList = (from tp in db.TblTestParameterMappings
                                                                      join p in db.TblLabParameterSetups on tp.ParamaterId equals p.ParameterId
                                                                      where tp.TestId == t.TestId
                                                                      select new TestParameterViewModel
                                                                      {
                                                                          TestId=t.TestId,
                                                                          TestName=t.TestName,
                                                                          ParameterName = p.ParameterName,
                                                                          Range=p.Range,
                                                                          Unit=p.Unit,                                                                         
                                                                      }).ToList()
                                                    }).ToList()
                              }).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<PathologyViewModel>> GetLabPatients()
        {
            var data = await db.TblSampleRegistrations.Where(x => x.IsCollected == 0).Select(y => new PathologyViewModel() { 
            Uhid=y.Uhid,
            PatientName= db.TblPatientRegistrations.Where(p=> p.Uhid==y.Uhid).Select(p=>p.FirstName+" "+p.LastName).FirstOrDefault(),
            RecordNo=y.RecordNo,
            RecordType=y.RecordType,
            SrNo=y.SrNo,
            ContactNo=db.TblPatientRegistrations.Where(c=>c.Uhid==y.Uhid).Select(c=>c.Contactno).FirstOrDefault(),
            }).ToListAsync();

            return data;
        }

        public async Task<int> AddSampleRegistration(PathologyViewModel model)
        {
           using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                   
                    var updateSampleCollection = await db.TblSampleRegistrations.Where(x => x.SrNo == model.SrNo).FirstOrDefaultAsync();
                    if (updateSampleCollection != null)
                    {
                        updateSampleCollection.IsCollected = 1;//0=isCollected false, 1=isCollected true, 2=result entry
                        updateSampleCollection.LabNo = GetMaxLabNo();
                        updateSampleCollection.CreatedDate = model.CreatedDate;
                        db.Entry(updateSampleCollection).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return updateSampleCollection.LabNo??0 ;
                    }
                    return 0;
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message);
                    await transaction.RollbackAsync();
                    return 0;
                }
            }
        }

        public async Task<IEnumerable<PathologyViewModel>> GetSampleRegisteredPatients()
        {
            var data = await db.TblSampleRegistrations.Where(x => x.IsCollected == 1).Select(y => new PathologyViewModel()
            {
                Uhid = y.Uhid,
                PatientName = db.TblPatientRegistrations.Where(p => p.Uhid == y.Uhid).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault(),
                RecordNo = y.RecordNo,
                RecordType = y.RecordType,
                SrNo = y.SrNo,
                LabNo=y.LabNo,
                ContactNo = db.TblPatientRegistrations.Where(c => c.Uhid == y.Uhid).Select(c => c.Contactno).FirstOrDefault(),
            }).ToListAsync();

            return data;
        }

        public async Task<bool> AddResultEntry(PathologyViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.TestList != null)
                    {
                        foreach(var items in model.TestList)
                        {
                            if (items.TestParameterList != null)
                            {
                                foreach (var item in items.TestParameterList)
                                {
                                    var data = new TblResultEntry()
                                    {
                                        LabNo = model.LabNo,
                                        TestId = item.TestId,
                                        TestName = item.TestName,
                                        ParameterName = item.ParameterName,
                                        Range = item.Range,
                                        Unit = item.Unit,
                                        Result = item.ParamResult,
                                        CreatedDate=model.CreatedDate,
                                    };
                                    await db.TblResultEntries.AddAsync(data);
                                    await db.SaveChangesAsync();
                                 
                                }
                            }
                       
                        }
                        var updateCollectedStatus = await db.TblSampleRegistrations.Where(x => x.LabNo == model.LabNo).FirstOrDefaultAsync();
                        if (updateCollectedStatus != null)
                        {
                            updateCollectedStatus.IsCollected = 2; //0= not collected, 1=collected, 3=result entry,
                            db.Entry(updateCollectedStatus).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return true;
                        }
                    }
                    return false;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<IEnumerable<PathologyViewModel>> GetResultEntryList()
        {
            var resultData = await db.TblSampleRegistrations.Where(x => x.IsCollected == 2).Select(x => new PathologyViewModel()
            {
                LabNo= x.LabNo,
                Uhid=x.Uhid,
                PatientName= db.TblPatientRegistrations.Where(p=> p.Uhid==x.Uhid).Select(p=>p.FirstName+" " + p.LastName).FirstOrDefault(),
            }).OrderByDescending(x=>x.LabNo).ToListAsync();
            return resultData;
        }

        public async Task<PathologyViewModel> GetLabResult(int LabNo, decimal Uhid)
        {
            var data = await (from records in db.TblSampleRegistrations
                              join results in db.TblResultEntries on records.LabNo equals results.LabNo
                              join patient in db.TblPatientRegistrations on records.Uhid equals patient.Uhid
                              where records.LabNo == LabNo && records.Uhid == Uhid
                              group results by new
                              {
                                  records.Uhid,
                                  patient.FirstName,
                                  patient.LastName,
                                  patient.Age,
                                  patient.AgeType,
                                  patient.Gender,
                                  patient.Address,
                                  records.RecordType,
                                  records.LabNo,
                                  records.CreatedDate,
                                 
                              } into g
                              select new PathologyViewModel
                              {
                                  Uhid=g.Key.Uhid,
                                  PatientName=g.Key.FirstName+" "+g.Key.LastName,
                                  Age=g.Key.Age+" "+g.Key.AgeType,
                                  Address=g.Key.Address,
                                  Gender=g.Key.Gender,
                                  LabNo=g.Key.LabNo,
                                  TestList = g.GroupBy(r => r.TestName).Select(t => new TestsListViewModel
                                  {
                                      TestName = t.Key,
                                      TestParameterList = t.Select(p => new TestParameterViewModel
                                      {
                                          TestId=p.TestId,
                                          TestName=p.TestName,
                                          ParameterName = p.ParameterName,
                                          Range = p.Range,
                                          ParamResult = p.Result,
                                          Unit = p.Unit
                                      }).ToList()
                                  }).ToList()
                              }
                              ).FirstOrDefaultAsync();
            return data;
        }

        public async Task<bool> EditResultEntry(PathologyViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.TestList != null)
                    {
                        foreach (var items in model.TestList)
                        {
                            if (items.TestParameterList != null)
                            {
                                foreach (var item in items.TestParameterList)
                                {
                                    // Check if the entry already exists
                                    var existingEntry = await db.TblResultEntries
                                                               .FirstOrDefaultAsync(e => e.LabNo == model.LabNo
                                                                                      && e.TestId == item.TestId
                                                                                      && e.ParameterName == item.ParameterName);
                                    if (existingEntry != null)
                                    {
                                        existingEntry.Result = item.ParamResult;

                                        db.TblResultEntries.Update(existingEntry);
                                    }
                                    await db.SaveChangesAsync();

                                }
                             
                            }
                        }
                    
                        await transaction.CommitAsync();
                        return true;
                    }
                    return false;
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
