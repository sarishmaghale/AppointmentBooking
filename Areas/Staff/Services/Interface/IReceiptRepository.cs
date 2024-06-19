using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IReceiptRepository
    {
        public Task<IEnumerable<ReceiptDetails>> GetTestsDataOnTestGroup(int TestGroupId);
        public Task<double> GetTestPrice(int TestId);
        public Task<bool> AddCashReceipt(ReceiptViewModel model);
        public Task<ReceiptViewModel> ReceiptDetails(int ReceiptNo);
    }
}
