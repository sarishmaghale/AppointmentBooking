using AppointmentBooking.Data;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class RegistrationRepo:IRegistrationRepo
    {
        private MedicareAppointmentDbContext db;
        public RegistrationRepo(MedicareAppointmentDbContext _db)
        {
            db = _db;
        }
        public async Task<decimal> AddPatientRegistration(PatientViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TblPatientRegistration data = new TblPatientRegistration()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        Age = model.Age,
                        Dob = model.Dob,
                        Contactno = model.Contactno,
                        Gender = model.Gender,
                        District = model.District,
                        Ethnicity = model.Ethnicity,
                        Panno = model.Panno,
                        Email = model.Email,
                        IsDelete = model.IsDelete,
                        CreatedDate = model.CreatedDate,
                        CreatedTime = model.CreatedDate,
                        CreatedByUser = model.CreatedByUser,
                        AgeType = model.AgeType,
                    };
                    await db.TblPatientRegistrations.AddAsync(data);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return data.Uhid;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return 0;
                }
            }

        }

        public async Task<AccountViewModel> CheckUserAccount(AccountViewModel model)
        {
            var result= await db.TblUserAccounts.Where(x => x.Username == model.Username && x.Password == model.Password && x.Shift == model.Shift && x.Department == model.Department).Select(m=> new AccountViewModel()
            {
                Username=m.Username,
                UserId=m.UserId,
                Shift=m.Shift,
                Department=m.Department,
            }).FirstOrDefaultAsync();
            if (result!=null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<PatientViewModel> GetPatientsByUhid(decimal uhid)
        {
            var data = await db.TblPatientRegistrations.Where(x => x.Uhid == uhid).Select(m => new PatientViewModel()
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Dob = m.Dob,
                Age = m.Age,
                AgeType = m.AgeType,
                Address = m.Address,
                Contactno = m.Contactno,
                Gender = m.Gender,
                Ethnicity = m.Ethnicity,
                Email = m.Email,
                Uhid = m.Uhid,
            }).FirstOrDefaultAsync();
            return data;
        }


        
    }
}
