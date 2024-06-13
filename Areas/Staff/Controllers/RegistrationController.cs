using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Controllers;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class RegistrationController : Controller
    {
        private ApiService apiService;
        private IRegistrationRepo provider;
       
        public RegistrationController(ApiService _apiService, IRegistrationRepo _provider)
        {
            apiService = _apiService;
            provider = _provider;

        }
        public IActionResult PatientRegistration()
        {
            return View();
        }
        public async Task<IActionResult> GetInsureePersonalInfo(string insid)
        {
            string url = "https://imis.hib.gov.np/api/api_fhir/Patient/?identifier=" + insid;
            string login = "baghaudahf";
            string psw = "jJPEQmFdT0zX3KE6S4ct";
            string HKeyword = "remote-user";
            string HValue = "bhfhir";
            var result = await apiService.GetInsureePersonalInfo(url, login, psw, HKeyword, HValue);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRegistration(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal uhid = await provider.AddPatientRegistration(model);
                if (uhid > 0)
                {
                    TempData["SucessRegisterMsg"] = "Successfully Added! Redirect to OPD Registration?";
                    TempData["Uhid"] = uhid.ToString();

                }
                else
                {
                    TempData["Msg"] = "Failed to add";
                }
                //return RedirectToAction("OPDRegistration", "OPDTransaction");
            }
            else
            {
                TempData["Msg"] = "Something went wrong";
            }
            return View("PatientRegistration");
        }
    }
}
