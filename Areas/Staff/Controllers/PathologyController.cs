using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class PathologyController : Controller
    {
        private IPathologyRepository pathologyProvider;
        public PathologyController(IPathologyRepository _pathologyProvider)
        {
            pathologyProvider = _pathologyProvider;
        }

    
        
        public async Task<IActionResult> SampleRegistration()
        {
            var result = await pathologyProvider.GetLabPatients();
            return View(result);
        }
        public async Task<IActionResult> LabPatientPartial(int SrNo)
        {
            var data = await pathologyProvider.GetLabPatientInfo(SrNo);
            return Json(data);
        }
        public async Task<IActionResult> RegisteredPatient(int LabNo)
        {
            var data = await pathologyProvider.GetTestParameter(LabNo);
            return Json(data);
        }

      [HttpPost]
        public async Task<IActionResult> SampleRegistration(PathologyViewModel model)
        {
            int result = await pathologyProvider.AddSampleRegistration(model);
            if (result != 0)
            {
                TempData["SampleRegistrationMsge"] = "Successfully Registered! LabNo=" + result;
            }
            else
            {
                TempData["SampleRegistrationMsge"] = "Failed to add";
            }
            return RedirectToAction("SampleRegistration");
        }
        public async Task<IActionResult> ResultEntry()
        {
            var data = await pathologyProvider.GetSampleRegisteredPatients();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult>ResultEntry(PathologyViewModel model)
        {
            bool result = await pathologyProvider.AddResultEntry(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Successfully added!";
            }
            else
            {
                TempData["FailedMessage"] = "Failed to add!";
            }
            return RedirectToAction("ResultEntry");
        }
      public async Task<IActionResult> LabResults()
        {
            var result = await pathologyProvider.GetResultEntryList();
            return View(result);
        }
        public async Task<IActionResult> PatientLabRecord(int LabNo, decimal Uhid)
        {
            var result = await pathologyProvider.GetLabResult(LabNo, Uhid);
            return PartialView("_PatientLabRecordPartial", result);
        }
        public async Task<IActionResult> LabRecordInfo(int LabNo, decimal Uhid)
        {
            var data = await pathologyProvider.GetLabResult(LabNo, Uhid);
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> EditResultEntry(PathologyViewModel model)
        {
            bool reply = await pathologyProvider.EditResultEntry(model);
            if (reply)
            {
                TempData["SuccessMessage"] = "Successfully updated!";

            }
            else
            {
                TempData["FailedMessage"] = "Failed to update!";

            }
            return RedirectToAction("LabResults");
        }
    }
}
