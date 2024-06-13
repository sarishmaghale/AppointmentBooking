
using AppointmentBooking.Models;
namespace AppointmentBooking.Services.Interface
{
    public interface IOPDBookingService
    {
        public Task<int> AddOPDBooking(OPDBookingViewModel model);
        public Task<int> UpdatePayStatus(OPDBookingViewModel model);
        public Task<OPDBookingViewModel> GetBookingInfo(int BookingId);

    }
}
