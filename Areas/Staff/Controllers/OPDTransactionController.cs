using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
