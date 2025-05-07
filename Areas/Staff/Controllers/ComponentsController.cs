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
        private IComponentSetupRepository setupProvider;
        public ComponentsController(IComponentSetupRepository _componentSetup)
        {
            setupProvider = _componentSetup;
       
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
                int doctorId = await setupProvider.AddDoctor(model);
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
      public IActionResult HospitalTestSetup()
        {
    
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TestGroupSetup(TestSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await setupProvider.AddTestGroup(model);
                if (result)
                {
                    TempData["SuccessMsge"] = "TestGroup successfully added";
                }
                else
                {
                    TempData["FailedMsge"] = "Failed to add TestGroup";                    
                }
            }
            return RedirectToAction("HospitalTestSetup");
        }
        public async Task<IActionResult> TestSetup(TestSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await setupProvider.AddTest(model);
                if (result)
                {
                    TempData["SuccessMsge"] = "Tests successfully added";
                }
                else
                {
                    TempData["FailedMsge"] = "Failed to add Test";
                }
            }
            return RedirectToAction("HospitalTestSetup");
        }
        public async Task< IActionResult> EditDoctor(int id)
        {
            var result = await setupProvider.GetDoctorInfo(id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EditDoctor(DoctorSetupViewModel model)
        {
            bool result = await setupProvider.UpdateDoctorInfo(model);
            if (result)
            {
                TempData["SuccessMsge"] = "Doctor Info Updated successfully!";
            }
            else
            {
                TempData["FailedMsge"] = "Failed to update DoctorInfo!";
            }
            return RedirectToAction("Doctors", "Element");
        }
        public IActionResult BedSetup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BedCategorySetup(BedSetupViewModel model)
        {
            bool result = await setupProvider.AddBedCategory(model);
            if (result)
            {
                TempData["SuccessMsge"] = "Bed category added successfully!";
            }
            else
            {
                TempData["FailedMsge"] = "Failed to setup bed category!";
            }
            return RedirectToAction("BedSetup");
        }
        [HttpPost]
        public async Task<IActionResult> BedSetup(BedSetupViewModel model)
        {
            bool result = await setupProvider.AddBeds(model);
            if (result)
            {
                TempData["SuccessMsge"] = "Bed setup successfully!";
            }
            else
            {
                TempData["FailedMsge"] = "Failed to setup bed!";
            }
            return RedirectToAction("BedSetup");
        }
        public IActionResult ParameterSetup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ParameterSetup(ParameterSetupViewModel model)
        {
            bool result = await setupProvider.AddNewLabParameter(model);
            if (result)
            {
                TempData["SuccessMsge"] = "Parameter setup successfully!";
            }
            else
            {
                TempData["FailedMsge"] = "Failed to setup parameter!";
            }
            return RedirectToAction("ParameterSetup");
        }
        [HttpPost]
        public async Task<IActionResult> ParameterTestMapping(ParameterSetupViewModel model)
        {
            bool checkResult = await setupProvider.CheckParameterTestMapping(model);
            if (!checkResult)
            {
                bool result = await setupProvider.AddParameterTestMapping(model);
                if (result)
                {
                    TempData["SuccessMsge"] = "Parameter-Test Mapping successfully!";
                }
                else
                {
                    TempData["FailedMsge"] = "Failed to map Parameter-Test!";
                }
            }
            else
            {
                TempData["FailedMsge"] = "Mapping already exists!";
            }
            return RedirectToAction("ParameterSetup");
        }

        public async Task< IActionResult> UserSetup()
        {
            var model = new AccountViewModel();
            model.UserList = await setupProvider.GetUserList();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserSetup(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await setupProvider.AddNewUser(model);
                TempData["CreateUserMsge"] = (result) ? "Successfully added" : "Failed to add! Try again";
                return View();
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }
            return RedirectToAction("UserSetup");
        }
    }
}
