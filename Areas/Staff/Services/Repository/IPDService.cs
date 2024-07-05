using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
                        BedType = model.BedType,
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
                              }).FirstOrDefaultAsync();
            return result;                          
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
