using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class ReceiptTransactionController : Controller
    {

        private IReceiptRepository receiptProvider; private IOPDRepository opdProvider; IIPDRepository ipdProvider;
        public ReceiptTransactionController(IReceiptRepository _receiptProvider, IOPDRepository _opdProvider, IIPDRepository _ipdProvider)
        {
            receiptProvider = _receiptProvider;
            opdProvider = _opdProvider;
            ipdProvider = _ipdProvider;
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
        public async Task<IActionResult> GetPatientInfoByIPD(int ipd)
        {
            var data = await ipdProvider.GetPatientsByIPD(ipd);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddCashReceipt([FromBody] ReceiptViewModel model)
        {
            int result = await receiptProvider.AddCashReceipt(model);
            if (result != 0)
            {
                TempData["CashReceiptMsge"] = "Successfully Added";
                return Json(new { result });
            }
            TempData["CashReceiptMsge"] = "Failed to add";
            return RedirectToAction("CashReceipt");
        }

        public async Task<IActionResult> ReceiptDetailsPartial(int ReceiptNo)
        {
            var data = await receiptProvider.GetReceiptDetails(ReceiptNo);
            return PartialView("_ReceiptDetailsPartial", data);
        }
        public async Task<IActionResult> ReceiptDetailsMain(int ReceiptNo)
        {          
            var data = await receiptProvider.GetReceiptDetails(ReceiptNo);
            return View(data);
        }

        public IActionResult CashSummary()
        {
            var model = new ReceiptViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CashSummary(ReceiptViewModel model)
        {
            var results = await receiptProvider.GetFilterCashSummary(model);
            model.Results = results;
            return View("CashSummary", model);
        }

        [HttpPost]
        public IActionResult ExportCashSummary(ReceiptViewModel model)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cash Summary Reports");
                worksheet.Cell(1, 1).Value = "Receipt No";
                worksheet.Cell(1, 2).Value = "Pay Type";
                worksheet.Cell(1, 3).Value = "Patient Name";
                worksheet.Cell(1, 4).Value = "Total Amount";
                worksheet.Cell(1, 5).Value = "Doctor Name";
                worksheet.Cell(1, 6).Value = "Created Date";

                var headerRange = worksheet.Range(1, 1, 1, 6);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                int row = 2;
                foreach (var group in model.Results)
                {
                    worksheet.Cell(row, 4).Value = group.TestGroupDept;
                    row++;
                    foreach (var receipt in group.Receipts)
                    {
                        worksheet.Cell(row, 1).Value = receipt.ReceiptNo;
                        worksheet.Cell(row, 2).Value = receipt.PayType;
                        worksheet.Cell(row, 3).Value = receipt.PatientName;
                        worksheet.Cell(row, 4).Value = receipt.TotalAmount;
                        worksheet.Cell(row, 5).Value = receipt.DoctorName;
                        worksheet.Cell(row, 6).Value = receipt.CreatedDate;
                        row++;
                    }
                    worksheet.Cell(row, 3).Value = "--TOTAL AMOUNT--";
                    worksheet.Cell(row, 4).Value = group.ReceiptTotal;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string excelName = $"CashSummary-{System.DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }

            }
        }

        public IActionResult DischargeBill()
        {
            return View();
        }
        public async Task<IActionResult> ExpenseEntry(int IpdNo)
        {
            var result = await ipdProvider.GetIPDExpenseEntries(IpdNo);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> DischargeBill(ReceiptViewModel model)
        {
            bool dataExists = await receiptProvider.CheckDischargeBill(Convert.ToInt32(model.Ipdno));
            if (!dataExists)
            {
                int ReceiptNo = await receiptProvider.AddDischargeBill(model);
                if (ReceiptNo!=0)
                {
                    TempData["DischargeBillMsge"] = "Bill successfully saved";
                    return RedirectToAction("ReceiptDetailsMain", new { ReceiptNo =ReceiptNo});
                }
                TempData["DischargeBillMsge"] = "Failed to save";
                return RedirectToAction("DischargeBill");
            }
            TempData["DischargedBill"] = "Bill was already saved!";
            return View();

        }

        public IActionResult PatientExpenseEntry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExpenseEntry([FromBody] List<ExpenseEntryViewModel> tableData)
        {
            bool result = await receiptProvider.AddPatientExpenseEntry(tableData);
            if (result)
            {
                TempData["SuccessMsge"] = "Expense added!";
            }
            else
            {
                TempData["FailedMsge"] = "Failed to add expensse!";
            }
            return View("PatientExpenseEntry");
        }
    }
}
