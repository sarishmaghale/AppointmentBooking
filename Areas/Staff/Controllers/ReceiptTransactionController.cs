using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class ReceiptTransactionController : Controller
    { 

        private IReceiptRepository receiptProvider;
        public ReceiptTransactionController(IReceiptRepository _receiptProvider)
        {
            receiptProvider = _receiptProvider;
        }

        public IActionResult CashReceipt()
        {
            return View();
        }
        public async Task<IActionResult> GetTestOfGroup(int TestGroupId)
        {
            var data = await receiptProvider.GetTestsDataOnTestGroup(TestGroupId);
            return Json(data);
        }
        public async Task<double> GetTestPrice(int TestId)
        {
            return (await receiptProvider.GetTestPrice(TestId));
        }
    }
}
