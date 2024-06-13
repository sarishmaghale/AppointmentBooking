using AppointmentBooking.Data;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class OPDRespository:IOPDRepository
    {
        private MedicareAppointmentDbContext db;
        public OPDRespository(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }
        public async Task<int> AddOPDRegistration(OPDViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TblOpdregistration data = new TblOpdregistration()
                    {
                        Uhid = model.Uhid,
                        RegNo = model.RegNo,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Opdqueue = model.Opdqueue,
                        Department = model.Department,
                        Address = model.Address,
                        District = model.District,
                        Contactno = model.Contactno,
                        Dob = model.Dob,
                        Age = model.Age,
                        AgeType = model.AgeType,
                        Gender = model.Gender,
                        Religion = model.Religion,
                        ConsultantDr = model.ConsultantDr,
                        RoomNo = model.RoomNo,
                        FloorName = model.FloorName,
                        CaseType = model.CaseType,
                        PayType = model.PayType,
                        FeeType = model.FeeType,
                        Amount = model.Amount,
                        PaidAmt = model.PaidAmt,
                        RefererName = model.RefererName,
                        CreatedDate = model.CreatedDate,
                        CreatedTime = model.CreatedTime,
                        CreatedByUser = model.CreatedByUser,
                    };
                    await db.TblOpdregistrations.AddAsync(data);
                    int SrNo = await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return SrNo;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return 0;
                }
            }
        }
        public async Task<double> GetDoctorFees(int DoctorId, int FeeTypeId)
        {

           var data = await db.TblDoctorFeeTypeSetups.Where(x => x.DoctorId == DoctorId && x.FeeTypeId == FeeTypeId).FirstOrDefaultAsync();
           double amount=(double)( (data != null) ? data.Fee : 0);
            return amount;
        }

        public async Task<IEnumerable<OPDViewModel>> GetOPDReports()
        {
            var date = DateTime.Now.ToShortDateString();
          var data= await db.TblOpdregistrations.Where(x => x.CreatedDate == date).Select(m=> new OPDViewModel()
          {
              SrNo=m.SrNo,
              FirstName=m.FirstName,
              LastName=m.LastName,
              PayType=m.PayType,
              Amount=m.Amount,
              ConsultantDr=m.ConsultantDr,
              FeeType=m.FeeType,
          }).ToListAsync();
            return data;
        }
    }
}
