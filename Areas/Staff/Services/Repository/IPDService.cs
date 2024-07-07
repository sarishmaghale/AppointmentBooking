using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class IPDService : IIPDRepository
    {
        private MedicareAppointmentDbContext db;
        public IPDService(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddIPDRegistration(IPDViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new TblIpdregistration()
                    {
                        Uhid = model.Uhid,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        AgeType = model.AgeType,
                        Gender = model.Gender,
                        Religion = model.Religion,
                        Gname = model.Gname,
                        Gaddress = model.Gaddress,
                        Gcontact = model.Gcontact,
                        Grelation = model.Grelation,
                        PayType = model.PayType,
                        AdmCharge = model.AdmCharge,
                        ConsultantDr = model.ConsultantDr,
                        BedType = model.IPDBedTypeId,
                        BedNo = model.BedNo,
                        CaseType = model.CaseType,
                        Complain = model.Complain,
                        Diagnosis = model.Diagnosis,
                        Remark = model.Remark,
                        CreatedDate = model.CreatedDate,
                        IsDischarged = 0,
                        CreatedByUser = model.CreatedByUser,
                        OpdSrNo = model.OpdSrNo,
                    };
                    await db.TblIpdregistrations.AddAsync(data);
                    await db.SaveChangesAsync();
                    var bedStatus = await db.TblIpdbedStatuses.Where(x => x.BedId == model.BedNo).FirstOrDefaultAsync();
                    if (bedStatus != null)
                    {
                        bedStatus.Status = 1;
                        bedStatus.IpdregNo = data.IpdregNo;
                        db.Entry(bedStatus).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    var expenseEntry = new TblIpdexpenseEntry()
                    {
                        IpdregNo = data.IpdregNo,
                        Uhid = data.Uhid,
                        TestGroup = "Bed Charge",
                        TestName = db.TblIpdbedTypes.Where(b => b.BedTypeId == model.IPDBedTypeId).Select(b => b.BedTypeName).FirstOrDefault(),
                        Price = model.IPDBedPrice,
                        Quantity = 1,
                        Amount = model.IPDBedPrice,
                        CreatedDate = model.CreatedDate,
                    };
                    await db.TblIpdexpenseEntries.AddAsync(expenseEntry);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return data.IpdregNo;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        public async Task<IEnumerable<IPDViewModel>> GetIPDBedValues(int BedTypeId)
        {
            var result = await (from bedCategory in db.TblIpdbedTypes
                                join beds in db.TblIpdbedStatuses on bedCategory.BedTypeId equals beds.BedTypeId
                                where bedCategory.BedTypeId == BedTypeId && beds.Status == 0 || beds.Status == null
                                select new IPDViewModel
                                {
                                    IPDBedTypeId = bedCategory.BedTypeId,
                                    BedNo = beds.BedId,
                                    IPDBedName = beds.BedName,
                                    IPDBedPrice = bedCategory.Price,
                                    IPDStatus=beds.Status,
                                }).ToListAsync();
            return result;
        }

        public async Task<ReceiptViewModel> GetIPDExpenseEntries(int IPDRegNo)
        {
            var model = new ReceiptViewModel();
            model.ReceiptDetails= await db.TblIpdexpenseEntries.Where(x => x.IpdregNo == IPDRegNo).Select(y => new ReceiptDetails()
            {
                IpdRegNo = IPDRegNo,
                TestGroupName = y.TestGroup,
                TestName = y.TestName,
                TestPrice = y.Price,
                Quantity = y.TestGroup == "Bed Charge" ? (y.CreatedDate.HasValue ? (DateTime.Now - y.CreatedDate.Value).Days : 0) : y.Quantity,
                Amount = y.Price * (y.TestGroup == "Bed Charge" ? (y.CreatedDate.HasValue ? (DateTime.Now - y.CreatedDate.Value).Days : 0) : y.Quantity),
            }).ToListAsync();
            var patientInfo = await db.TblIpdregistrations.Where(i => i.IpdregNo == IPDRegNo).FirstOrDefaultAsync();
            if (patientInfo != null)
            {
                model.Uhid = patientInfo.Uhid;
                model.PatientName = patientInfo.FirstName + " " + patientInfo.LastName;
                model.DoctorName = db.TblDoctorSetups.Where(d => d.DoctorId == patientInfo.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault();
                model.Age = patientInfo.Age + " " + patientInfo.AgeType;
                model.Gender = patientInfo.Gender;
                model.Address = patientInfo.Address;
                model.CreatedDate = patientInfo.CreatedDate;
            }
           
            return model;
        }

        public async Task<IEnumerable<IPDViewModel>> GetIPDPatientList()
        {
          var result=  await db.TblIpdregistrations.Select(x => new IPDViewModel() { 
              IpdregNo=x.IpdregNo,
          Uhid= x.Uhid,
          PatientName=x.FirstName+" "+x.LastName,
          Address=x.Address,
          District=x.District,
          Contactno=x.Contactno,
          Dob=x.Dob,
          AgeType=x.Age+" "+x.AgeType,
          Gender=x.Gender,
          Gname=x.Gname,
          Grelation=x.Grelation,
          Gcontact=x.Gcontact,
          DoctorName=db.TblDoctorSetups.Where(d=> d.DoctorId==x.ConsultantDr).Select(d=> d.DoctorName).FirstOrDefault(),
          IPDBedName=db.TblIpdbedStatuses.Where(b=> b.BedId==x.BedNo).Select(b=> b.BedName).FirstOrDefault(),
          CaseTypeName= db.TblCaseTypes.Where(c=> c.CaseTypeId==x.CaseType).Select(c=> c.CaseTypeName).FirstOrDefault(),
          CreatedDate=x.CreatedDate,
          IsDischarged=x.IsDischarged,
          }).ToListAsync();
            return result;
        }

        public async Task<IPDViewModel> GetPatientInfoOnBed(IDPBedViewModel model)
        {
            var result = await (from bedInfo in db.TblIpdbedStatuses
                              join patientInfo in db.TblIpdregistrations on bedInfo.IpdregNo equals patientInfo.IpdregNo
                              where bedInfo.BedId == model.BedId
                              select new IPDViewModel
                              {
                                  IpdregNo = Convert.ToInt32(bedInfo.IpdregNo),
                                  PatientName = patientInfo.FirstName + " " + patientInfo.LastName,
                                  AgeType = patientInfo.Age.ToString() + " " + patientInfo.AgeType,
                                  Contactno=patientInfo.Contactno,
                                  CaseTypeName = db.TblCaseTypes.Where(c => c.CaseTypeId == patientInfo.CaseType).Select(c => c.CaseTypeName).FirstOrDefault(),
                                  DoctorName = db.TblDoctorSetups.Where(d => d.DoctorId == patientInfo.ConsultantDr).Select(d => d.DoctorName).FirstOrDefault(),
                                  Complain = patientInfo.Complain,
                                  Diagnosis = patientInfo.Diagnosis,
                                  CreatedDate = patientInfo.CreatedDate,
                                  Address=patientInfo.Address,
                                  Uhid=patientInfo.Uhid,
                                  IPDBedName=bedInfo.BedName,
                                  Gname=patientInfo.Gname,
                                  Gcontact=patientInfo.Gcontact,
                                  Grelation=patientInfo.Grelation,
                              }).FirstOrDefaultAsync();
            return result;                          
        }

        public async Task<IPDViewModel> GetPatientsByIPD(int ipd)
        {
            var data = await db.TblIpdregistrations.Where(x => x.IpdregNo == ipd).Select(m => new IPDViewModel()
            {
                PatientName = m.FirstName + " " + m.LastName,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Address = m.Address,
                District = m.District,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                Uhid = m.Uhid,
                Gender = m.Gender,
                ConsultantDr = m.ConsultantDr,
                CreatedDate=m.CreatedDate,
                DoctorName = db.TblDoctorSetups.Where(y => y.DoctorId == m.ConsultantDr).Select(y => y.DoctorName).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<IDPBedViewModel>> GetWardsInfo(int BedTypeId)
        {
            try
            {
                var result = await (from bedCategory in db.TblIpdbedTypes
                                    join beds in db.TblIpdbedStatuses on bedCategory.BedTypeId equals beds.BedTypeId
                                    where bedCategory.BedTypeId == BedTypeId
                                    select new IDPBedViewModel
                                    {
                                        BedTypeId = bedCategory.BedTypeId,
                                        BedId = beds.BedId,
                                        BedName = beds.BedName,
                                        Price = bedCategory.Price,
                                        Status = beds.Status,
                                    }).ToListAsync();
                return result;
            }
            catch
            {
                return null;
            }
           
        }
    }
}
