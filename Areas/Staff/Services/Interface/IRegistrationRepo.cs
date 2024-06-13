using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IRegistrationRepo
    {
        public Task<decimal> AddPatientRegistration(PatientViewModel model);
        public Task<PatientViewModel> GetPatientsByUhid(decimal uhid);
        public Task<AccountViewModel> CheckUserAccount(AccountViewModel model);
    }
}
