using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class IPDTransactionController : Controller
    {
        private IRegistrationRepo patientRegistration;
        private IOPDRepository opdProvider;
        private IIPDRepository ipdProvider;
        public IPDTransactionController(IRegistrationRepo _patientRegistration,IOPDRepository _opdProvider,IIPDRepository _ipdProvider)
        {
            patientRegistration = _patientRegistration;
            opdProvider = _opdProvider;
            ipdProvider = _ipdProvider;
        }

       public IActionResult IPDRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IPDRegistration(IPDViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await ipdProvider.AddIPDRegistration(model);
                TempData["IPDMsge"] = "Succcessfully added"+result;
                return View();
            }
            TempData["IPDMsge"] = "Failed to add";
            return View();
        }
        public async Task<IActionResult> GetPatientInfoByUhid(decimal uhid)
        {
            var data = await patientRegistration.GetPatientsByUhid(uhid);
            return Json(data);
        }
        public async Task<IActionResult> GetPatientInfoByOPD(int opd)
        {
            var data = await opdProvider.GetPatientsByOPD(opd);
            return Json(data);
        }
        public async  Task<IActionResult> GetBedValues(int BedTypeId)
        {
            var result =await ipdProvider.GetIPDBedValues(BedTypeId);
            return Json(result);
        }
    }
}
