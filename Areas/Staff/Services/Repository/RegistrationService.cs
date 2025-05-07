using AppointmentBooking.Data;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.Areas.Staff.Services.Repository
{
    public class RegistrationService:IRegistrationRepository
    {
        private HospitalManagementDbContext db;
        public RegistrationService(HospitalManagementDbContext _db)
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
                        IsDelete = model.IsDelete,
                        CreatedDate = model.CreatedDate,
                        CreatedTime = model.CreatedTime,
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
            var result= await db.TblUserAccounts.Where(x => x.Username == model.Username && x.Password == model.Password && x.Department == model.Department).Select(m=> new AccountViewModel()
            {
                Username=m.Username,
                UserId=m.UserId,
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

        public async Task<List<PatientViewModel>> GetPatientList()
        {
            var data = await db.TblPatientRegistrations.Select(x => new PatientViewModel()
            {
                PatientName = x.FirstName + " " + x.LastName,
                Uhid = x.Uhid,
                Address = x.Address,
                Age = x.Age,
                AgeType = x.AgeType,
                Dob = x.Dob,
                Contactno = x.Contactno,
                Gender = x.Gender,
                District = x.District,
                Ethnicity = x.Ethnicity,
                CreatedDate = x.CreatedDate,
                CreatedTime = x.CreatedTime,
                CreatedByUser = x.CreatedByUser,

            }).ToListAsync();
            return data;
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
                District=m.District,
                Address = m.Address,
                Contactno = m.Contactno,
                Gender = m.Gender,
                Ethnicity = m.Ethnicity,
                Uhid = m.Uhid,
            }).FirstOrDefaultAsync();
            return data;
        }


        
    }
}
