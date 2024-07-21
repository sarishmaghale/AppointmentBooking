
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Models;
namespace AppointmentBooking.Services.Interface
{
    public interface IOPDBookingService
    {
        public Task<int> AddOPDBooking(OPDBookingViewModel model);
        public Task<int> UpdatePayStatus(OPDBookingViewModel model);
        public Task<OPDBookingViewModel> GetBookingInfo(int BookingId);

        public Task<PatientViewModel> GetPatientsByUhid(decimal uhid);
        public Task<double> GetDoctorFees(int DoctorId, int FeeTypeId);

    }
}
