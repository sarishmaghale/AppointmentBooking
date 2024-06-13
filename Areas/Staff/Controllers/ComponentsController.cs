using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class ComponentsController : Controller
    {
        private IComponentSetupRepo componentSetup;
        public ComponentsController(IComponentSetupRepo _componentSetup)
        {
            componentSetup = _componentSetup;
        }

       public IActionResult DoctorSetup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoctorSetup(DoctorSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                int doctorId = await componentSetup.AddDoctor(model);
                TempData["AddDoctorMsge"] = (doctorId > 0) ? "Successfully added" : "Failed to add! Try again";
                return View();
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            foreach (var error in errors)
            {
            ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }
            return View();
        }
        public IActionResult GetFeeTypeViews()
        {
            return PartialView("_FeeTypePartial");
        }
      
 
    }
}
