using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class ReceiptTransactionController : Controller
    { 

        private IReceiptRepository receiptProvider;private IOPDRepository opdProvider;
        public ReceiptTransactionController(IReceiptRepository _receiptProvider, IOPDRepository _opdProvider)
        {
            receiptProvider = _receiptProvider;
            opdProvider = _opdProvider;
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
        public async Task<IActionResult> GetPatientInfoByOPD(int opd)
        {
            var data = await opdProvider.GetPatientsByOPD(opd);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddCashReceipt([FromBody] ReceiptViewModel model)
        {
            bool res = await receiptProvider.AddCashReceipt(model);
            if (res)
            {
                TempData["CashReceiptMsge"] = "Successfully Added";
                string redirectUrl = Url.Action("CashReceipt", "ReceiptTransaction");
                return Json(new { redirectUrl });
            }
            TempData["CashReceiptMsge"] ="Failed to add";
            return RedirectToAction("CashReceipt");
        }

        public async Task<IActionResult> ReceiptDetailsPartial(int ReceiptNo)
        {
            var data = await receiptProvider.ReceiptDetails(3);
            return PartialView("_ReceiptDetailsPartial", data);
        }
        public async Task<IActionResult> ReceiptDetailsMain(int ReceiptNo)
        {
            var data = await receiptProvider.ReceiptDetails(3);
            return View(data);
        }
            

    }
}
