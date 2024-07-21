using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IPathologyRepository
    {
        public Task<IEnumerable<PathologyViewModel>> GetLabPatients();
        public Task<PathologyViewModel> GetLabPatientInfo(int SrNo);
        public Task<PathologyViewModel> GetTestParameter(int LabNo);
        public Task<int> AddSampleRegistration(PathologyViewModel model);
        public Task<IEnumerable<PathologyViewModel>> GetSampleRegisteredPatients();
        public Task<bool> AddResultEntry(PathologyViewModel model);
        public Task<bool> EditResultEntry(PathologyViewModel model);
        public Task<IEnumerable<PathologyViewModel>> GetResultEntryList();
        public Task<PathologyViewModel> GetLabResult(int LabNo, decimal Uhid);
    }
}
