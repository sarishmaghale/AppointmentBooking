﻿using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize]
    public class IPDTransactionController : Controller
    {
        private IRegistrationRepository patientRegistration;
        private IOPDRepository opdProvider;
        private IIPDRepository ipdProvider;
        public IPDTransactionController(IRegistrationRepository _patientRegistration,IOPDRepository _opdProvider,IIPDRepository _ipdProvider)
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
                TempData["IPDMsge"] = "Succcessfully added";
                return RedirectToAction("IPDRegistration");
            }
            TempData["IPDMsge"] = "Failed to add";
            return View() ;
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
        public IActionResult RoomBedStatus()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoomBedStatus(int BedTypeId)
        {
            var result = await ipdProvider.GetWardsInfo(BedTypeId);
            return View(result);
        }

        public async Task<IActionResult> PatientOnBedDetail(int BedTypeId,int BedId)
        {
            var model = new IDPBedViewModel();
            model.BedTypeId = BedTypeId; model.BedId = BedId;
           var result= await ipdProvider.GetPatientInfoOnBed(model);
            return Json(result);
        }
        public IActionResult IPDPatientList()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>GetIPDPatientList(IPDViewModel model)
        {
            var data = await ipdProvider.GetIPDPatientList(model);
            model.IpdPatients = data;
            return View("IPDPatientList",model);
        }
        public async Task<IActionResult> IpdCardPartial(int IpdRegNo)
        {
            var data = await ipdProvider.GetPatientsByIPD(IpdRegNo);
            return PartialView("_IPDPatientDetailsPartial", data);
        }
    }
}
