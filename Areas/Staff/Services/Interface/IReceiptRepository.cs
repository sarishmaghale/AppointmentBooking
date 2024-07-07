using AppointmentBooking.Areas.Staff.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentBooking.Areas.Staff.Services.Interface
{
    public interface IReceiptRepository
    {
        public Task<IEnumerable<ReceiptDetails>> GetTestsDataOnTestGroup(int TestGroupId);
        public Task<double> GetTestPrice(int TestId);
        public Task<int> AddCashReceipt(ReceiptViewModel model);
        public Task<ReceiptViewModel> GetReceiptDetails(int ReceiptNo);
        public Task<List<ReceiptViewModel>> GetFilterCashSummary(ReceiptViewModel model);
        public Task<bool> AddDischargeBill(ReceiptViewModel model);
        public Task<bool> CheckDischargeBill(int IPDRegNo);
        public Task<bool> AddPatientExpenseEntry(List<ExpenseEntryViewModel> model);
    }
}
