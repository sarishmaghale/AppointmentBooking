using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IIPDRepository
    {
        public Task<IEnumerable<IPDViewModel>> GetIPDBedValues(int BedTypeId);
        public Task<int> AddIPDRegistration(IPDViewModel model);
        public Task<IEnumerable<IDPBedViewModel>> GetWardsInfo(int BedTypeId);
        public Task<IPDViewModel> GetPatientInfoOnBed(IDPBedViewModel model);
        public Task<ReceiptViewModel> GetIPDExpenseEntries(int IPDRegNo);
        public Task<IPDViewModel> GetPatientsByIPD(int ipd);
        public Task<IEnumerable<IPDViewModel>> GetIPDPatientList();
    }
}
