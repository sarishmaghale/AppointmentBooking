using AppointmentBooking.Areas.Staff.Models;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IComponentSetupRepository
    {
        public Task<int> AddDoctor(DoctorSetupViewModel model);
        public Task<bool> AddTestGroup(TestSetupViewModel model);
        public Task<bool> AddTest(TestSetupViewModel model);
     

        public Task<DoctorSetupViewModel> GetDoctorInfo(int DoctorId);
        public Task<bool> UpdateDoctorInfo(DoctorSetupViewModel model);
        public Task<bool> AddBedCategory(BedSetupViewModel model);
        public Task<bool> AddBeds(BedSetupViewModel model);
        public Task<IEnumerable<DoctorSetupViewModel>> GetDoctorList();
        public Task<bool> AddNewLabParameter(ParameterSetupViewModel model);
        public Task<bool> AddParameterTestMapping(ParameterSetupViewModel model);
        public Task<bool> CheckParameterTestMapping(ParameterSetupViewModel model);
        public Task<bool> AddNewUser(AccountViewModel model);
        public Task<IEnumerable<AccountViewModel>> GetUserList();
    }
}
