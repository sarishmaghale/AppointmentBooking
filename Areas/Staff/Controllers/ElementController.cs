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
        private CommonUtility utils;
        public ElementController(CommonUtility _utils)
        {
            utils = _utils;
        }
        public IActionResult Doctors()
        {
            var data = utils.GetDoctorList();
            return View(data);
        }
    }
}
