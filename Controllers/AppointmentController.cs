using AppointmentBooking.Data;
using AppointmentBooking.Services;
using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AppointmentBooking.Services.Interface;
using System;
using DocumentFormat.OpenXml.Office2010.Excel;
using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;

namespace AppointmentBooking.Controllers
{
    public class AppointmentController : Controller
    {
        private ApiService apiService;
        private IOPDBookingService opdbookingService;
        private IPaymentService paymentService;
        private CommonUtility utils;private IPathologyRepository labProvider;

        public AppointmentController(ApiService _insuranceService, IOPDBookingService _opdbookingService, IPaymentService _paymentService, CommonUtility _utils,IPathologyRepository _labProvider)
        {
            opdbookingService = _opdbookingService;
            apiService = _insuranceService;
            paymentService = _paymentService;
            utils = _utils;
            labProvider = _labProvider;
        }
       
        public IActionResult AppointmentBooking()
        {
            return View();
        }
     
        public async Task<IActionResult> GetInsureePersonalInfo(string insid)
        {
            var result = await apiService.GetInsureePersonalInfo(insid);
            return Json(result);
        }
        public async Task<IActionResult> GetPatientInfoByUhid(decimal uhid)
        {
            var data = await opdbookingService.GetPatientsByUhid(uhid);
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> AppointmentBooking(OPDBookingViewModel model, string button)
        {
                if (ModelState.IsValid)
                {
                int BookingId = await opdbookingService.AddOPDBooking(model);
                switch (button)
                {
                    case "Esewa":                     
                        if (BookingId > 0)
                        {
                            var payData = await paymentService.ActivateEsewaPayment(BookingId,model.Amount);
                            return View("ActivateEsewaPayment", payData);
                        }
                        else
                        {
                            TempData["BookingMsg"] = "Booking Failed";
                            return View(model);
                        }
                      
                    case "Khalti":
                        
                        if (BookingId > 0)
                        {
                            string paymentUrl = await paymentService.ActivateKhaltiPayment(BookingId,model.Amount);
                            return Redirect(paymentUrl);
                        }
                        else
                        {
                            TempData["BookingMsg"] = "Booking Failed";
                        }
                        return View(model);

                    default:
                        // Handle unknown actions or return an error
                        return BadRequest();
                }
              
                    
                }
                else
                {
                var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    // Log error.ErrorMessage or error.Exception
                    TempData["BookingMsg"] = error.ErrorMessage;

                }
            }
            return View("AppointmentBooking");           
        }

        [HttpGet]
        public async Task<IActionResult> BookingCard(int BookingId)
        {
            TempData["BookingId"] = BookingId;          
            var Data = await opdbookingService.GetBookingInfo(BookingId);
            return View(Data);
            
        }

       

        //public async Task<IActionResult> InitiateKhalti(int id)
        //{           
        //   string paymentUrl= await  paymentService.ActivateKhaltiPayment(id);           
        //    return Redirect(paymentUrl);
        //}

        //public async Task<IActionResult> InitiateEsewa(int id)
        //{
        //    var payData = await paymentService.ActivateEsewaPayment(id);
        //    return View("ActivateEsewaPayment",payData);
        //}

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess()
        {
            //response data from esewa
            string encodedData = HttpContext.Request.Query["data"];
            if (encodedData != null)
            {
                var data = await paymentService.ValidateEsewaPayment(encodedData);
                if (data != null)
                {
                    int id = await opdbookingService.UpdatePayStatus(data);
                    var getData = await opdbookingService.GetBookingInfo(id);
                    TempData["PaymentMessage"] = "Tranasction Successfull. OPDBooking Registered";
                    return View("BookingCard", getData);
                }
            }
            //response data from Khalti
            string pidx = HttpContext.Request.Query["pidx"];
            if (pidx != null)
            {
                string id = HttpContext.Request.Query["purchase_order_id"];
                var data = await paymentService.ValidateKhaltiPayment(pidx, id);
                if (data != null)
                {
                    int getId = await opdbookingService.UpdatePayStatus(data);
                    var getData = await opdbookingService.GetBookingInfo(getId);
                    TempData["PaymentMessage"] = "Tranasction Successfull. OPDBooking Registered";
                    return View("BookingCard", getData);

                }
            }
         
            TempData["PaymentMessage"] = "Tranasction Failed";
            return RedirectToAction("AppointmentBooking");
        }
        public async Task<IActionResult> GetDoctors(int DepartmentId)
        {
            var result = await utils.GetValuesOnDepartment(DepartmentId);
            return Json(result);
        }
        public async Task<IActionResult> GetDoctorFees(int FeeTypeId, int DoctorId)
        {
            double amount = await opdbookingService.GetDoctorFees(DoctorId, FeeTypeId);
            return Json(amount);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult LabReport()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LabReport(PathologyViewModel model)
        {
            if (model.LabNo != null && model.Uhid != null)
            {
              var data=  await labProvider.GetLabResult(Convert.ToInt32(model.LabNo), Convert.ToDecimal(model.Uhid));
                if (data == null)
                {
                    TempData["ResultMsge"] = "No Data available";
                }
                return View("LabReport",data);
            }             
                return RedirectToAction("LabReport");           
        }
    }
}
