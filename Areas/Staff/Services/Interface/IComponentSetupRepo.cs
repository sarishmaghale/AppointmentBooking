using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IComponentSetupRepo
    {
        public Task<int> AddDoctor(DoctorSetupViewModel model);
        
    }
}
