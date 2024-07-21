using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
   
    public class OPDTransactionController : Controller
    {
        private CommonUtility utils;
        private IRegistrationRepository patientRegistration;
        private IOPDRepository opdRegistration;
   
        public OPDTransactionController(CommonUtility _utils, IRegistrationRepository _patientRegistration, IOPDRepository _opdRegistration)
        {
            utils = _utils;
            patientRegistration = _patientRegistration;
            opdRegistration = _opdRegistration;
         
        }
        public async Task<IActionResult> OPDRegistration()
        {
            var data = TempData["Uhid"];
            if (data != null)
            {
                if (decimal.TryParse(data.ToString(), out decimal uhid))
                {
                    var result = await utils.GetPatientsByUhid(uhid);
                    return View(result);
                }
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> OPDRegistration(OPDViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = await opdRegistration.AddOPDRegistration(model);
                TempData["OPDMsge"] = (id > 0) ? "Registered successfully" : "Failed";
                return RedirectToAction("OPDCard", new { OpdNo =id});

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    // Log error.ErrorMessage or error.Exception
                    TempData["OPDMSge"] = error.ErrorMessage;

                }
            }
            return RedirectToAction("OPDRegistration");
        }


        public async Task<IActionResult> GetDoctors(int DepartmentId)
        {
            var result = await utils.GetValuesOnDepartment(DepartmentId);
            return Json(result);
        }
        public async Task<IActionResult> GetPatientInfoByUhid(decimal uhid)
        {
            var data = await patientRegistration.GetPatientsByUhid(uhid);
            return Json(data);
        }
        public async Task<IActionResult> GetDoctorFees(int FeeTypeId, int DoctorId)
        {
            double amount =await opdRegistration.GetDoctorFees(DoctorId, FeeTypeId);
            return Json(amount);
        }

        [HttpGet]
        public async Task<IActionResult> OPDReports()
        {
            var model = new OPDViewModel();
            var data = await opdRegistration.GetOPDReports();
            model.opdReports = data;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OPDReports(OPDViewModel data)
        {
           var results= await opdRegistration.GetFilterOPDReports(data);
            data.opdReports = results;
            return View("OPDReports", data);
            
        }

        [HttpPost]
        public IActionResult ExportOPDReports(OPDViewModel model)
        {
            using (var workbook = new XLWorkbook())
            {
                var totalAmount = 0.0;
                var worksheet = workbook.Worksheets.Add("OPD Reports");
                worksheet.Cell(1, 1).Value = "SrNo";
                worksheet.Cell(1, 2).Value = "Pay Type";
                worksheet.Cell(1, 3).Value = "Patient Name";
                worksheet.Cell(1, 4).Value = "Amount";
                worksheet.Cell(1, 5).Value = "Fee Type";
                worksheet.Cell(1, 6).Value = "Doctor Name";

                var headerRange = worksheet.Range(1, 1, 1, 6);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                int row = 2;
                foreach (var result in model.opdReports)
                {
                    worksheet.Cell(row, 1).Value = result.SrNo;
                    worksheet.Cell(row, 2).Value = result.PayType;
                    worksheet.Cell(row, 3).Value = result.PatientName;
                    worksheet.Cell(row, 4).Value = result.Amount;
                    worksheet.Cell(row, 5).Value = result.FeeTypeName;
                    worksheet.Cell(row, 6).Value = result.DoctorName;
                    totalAmount = totalAmount +(double) result.Amount;
                    row++;
                }
                worksheet.Cell(row, 3).Value = "--TOTAL AMOUNT--";
                worksheet.Cell(row, 4).Value = totalAmount;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string excelName = $"OPDReports-{System.DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }

            }
        }

        public IActionResult ViewOPDDetails(int srNo)
        {
            return View();
        }
        public async Task<IActionResult> OPDCard(int OpdNo)
        {
            var result = await opdRegistration.GetPatientsByOPD(OpdNo);
            return View(result);
        }
        public async Task<IActionResult> OPDCardPartial(int OpdNo)
        {
            var result = await opdRegistration.GetPatientsByOPD(OpdNo);
            return PartialView("_OPDCardPartial", result);

        }
    }
}
