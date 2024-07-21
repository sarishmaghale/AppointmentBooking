using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class ElementController : Controller
    {
        private IComponentSetupRepository componentProvider;private IPatientHistoryRepository historyProvider;
        private IRegistrationRepository patientProvider; 
        public ElementController(IComponentSetupRepository _componentProvider,IPatientHistoryRepository _historyProvider,IRegistrationRepository _patientProvider)
        {
            componentProvider = _componentProvider;
            historyProvider = _historyProvider;

            patientProvider = _patientProvider;
        }
        public async Task< IActionResult> Doctors()
        {
            var data =await componentProvider.GetDoctorList();
            return View(data);
        }
        public IActionResult PatientHistory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientHistory(PatientViewModel model)
        {
           
            if (model.Uhid <= 0)
            {
                ModelState.AddModelError("", "Please enter a valid Patient ID.");
                return View("PatientHistory");
            }
            var history = await historyProvider.GetPatientHistory(Convert.ToDecimal(model.Uhid),model.SearchDate);
            var patientData = await patientProvider.GetPatientsByUhid(Convert.ToDecimal(model.Uhid));
            var historyModel = new PatientViewModel()
            {
                PatientName =patientData.FirstName+" "+patientData.LastName,
            };
            historyModel.HistoryItems = history;
            return View(historyModel);
        }
    
        public async Task<IActionResult> PatientList()
        {
            var resultData = await patientProvider.GetPatientList();
            return View(resultData);
        }

    }
}
