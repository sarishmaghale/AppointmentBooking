using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class IPDRepo : IIPDRepository
    {
        private MedicareAppointmentDbContext db;
        public IPDRepo(MedicareAppointmentDbContext _db)
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
                                }).ToListAsync();
            return result;
        }
    }
}
