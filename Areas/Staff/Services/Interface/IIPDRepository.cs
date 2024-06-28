using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IIPDRepository
    {
        public Task<IEnumerable<IPDViewModel>> GetIPDBedValues(int BedTypeId);
        public Task<int> AddIPDRegistration(IPDViewModel model);
    }
}
