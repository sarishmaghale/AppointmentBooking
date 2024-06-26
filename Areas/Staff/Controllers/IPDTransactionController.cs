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
        public IPDTransactionController(IRegistrationRepo _patientRegistration)
        {
            patientRegistration = _patientRegistration;
        }

       public IActionResult IPDRegistration()
        {
            return View();
        }
        public async Task<IActionResult> GetPatientInfoByUhid(decimal uhid)
        {
            var data = await patientRegistration.GetPatientsByUhid(uhid);
            return Json(data);
        }
    }
}
