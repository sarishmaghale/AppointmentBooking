using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IReceiptRepository
    {
        public Task<IEnumerable<ReceiptViewModel>> GetTestsDataOnTestGroup(int TestGroupId);
        public Task<double> GetTestPrice(int TestId);
    }
}
