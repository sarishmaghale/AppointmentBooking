using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IComponentSetupRepository
    {
        public Task<int> AddDoctor(DoctorSetupViewModel model);
        
    }
}
