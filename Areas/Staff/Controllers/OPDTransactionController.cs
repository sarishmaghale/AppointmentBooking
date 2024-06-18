using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
   
    public class OPDTransactionController : Controller
    {
        private CommonUtility utils;
        private IRegistrationRepo patientRegistration;
        private IOPDRepository opdRegistration;
   
        public OPDTransactionController(CommonUtility _utils, IRegistrationRepo _patientRegistration, IOPDRepository _opdRegistration)
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
            var data = await opdRegistration.GetOPDReports();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> FilterOPDReports(string paytype, string user, DateTime? fromDate, DateTime? toDate, List<DataTablesOrder> order)
        {
           var data= await opdRegistration.GetFilterOPDReports(paytype, user, fromDate, toDate,order);
            int totalCount = data.Count();
            int draw = int.Parse(Request.Form["draw"].FirstOrDefault());           
            var result = new
            {
                draw = draw,              
                recordsFiltered = totalCount,
              data=data
            };
            return Json(result);
            
        }
       
        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<OPDViewModel> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("OPD Reports");
                worksheet.Cell(1, 1).Value = "SrNo";
                worksheet.Cell(1, 2).Value = "PatientName";
                worksheet.Cell(1, 3).Value = "Paytype";
                worksheet.Cell(1, 4).Value = "Amount";
                worksheet.Cell(1, 5).Value = "DoctorName";
                worksheet.Cell(1, 6).Value = "FeeType";

                for(int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].SrNo;
                    worksheet.Cell(i + 2, 2).Value = data[i].FirstName;
                    worksheet.Cell(i + 2, 3).Value = data[i].PayType;
                    worksheet.Cell(i + 2, 4).Value = data[i].Amount;
                    worksheet.Cell(i + 2, 5).Value = data[i].ConsultantDr;
                    worksheet.Cell(i + 2, 6).Value = data[i].FeeType;
                }
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
    }
}
