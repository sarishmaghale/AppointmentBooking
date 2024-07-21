using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IPatientHistoryRepository
    {
        public Task<List<PatientHistoryItem>> GetPatientHistory(decimal Uhid,DateTime?targetDate);

    }
}
