
using AppointmentBooking.Areas.Staff.Models;
namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IOPDRepository
    {
        public Task<int> AddOPDRegistration(OPDViewModel model);
        public Task<double> GetDoctorFees(int DoctorId, int FeeTypeId);
        public  Task<List<OPDViewModel>> GetOPDReports();

        public Task<List<OPDViewModel>> GetFilterOPDReports(OPDViewModel model);
        public Task<OPDViewModel> GetPatientsByOPD(int OPDNo);
        public Task<List<OPDViewModel>> GetOpdPatientCount();

    }
}
