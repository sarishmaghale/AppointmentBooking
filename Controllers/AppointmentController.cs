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

namespace AppointmentBooking.Controllers
{
    public class AppointmentController : Controller
    {
        private ApiService apiService;
        private IOPDBookingService opdbookingService;
        private IPaymentService paymentService;
        private CommonUtility utils;

        public AppointmentController(ApiService _insuranceService, IOPDBookingService _opdbookingService, IPaymentService _paymentService, CommonUtility _utils)
        {
            opdbookingService = _opdbookingService;
            apiService = _insuranceService;
            paymentService = _paymentService;
            utils = _utils;
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

        [HttpPost]
        public async Task<IActionResult> OpdBooking(OPDBookingViewModel model)
        {
                if (ModelState.IsValid)
                {
                    int BookingId = await opdbookingService.AddOPDBooking(model);
                    if (BookingId>0)
                    {
                        TempData["BookingMsg"] = "Successfully registered";
                        return RedirectToAction("BookingCard", new {BookingId});
                    }
                    else
                    {
                        TempData["BookingMsg"] = "Failed to add";
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


       

        public async Task<IActionResult> InitiateKhalti(int id)
        {           
           string paymentUrl= await  paymentService.ActivateKhaltiPayment(id);           
            return Redirect(paymentUrl);
        }

        public async Task<IActionResult> InitiateEsewa(int id)
        {
            var payData = await paymentService.ActivateEsewaPayment(id);
            return View("ActivateEsewaPayment",payData);
        }

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
                    TempData["PaymentMessage"] = "Tranasction Successfull";
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
                    TempData["PaymentMessage"] = "Tranasction Successfull";
                    return View("BookingCard", getData);

                }
            }
            int bookingId = (int)TempData["BookingId"];
            TempData["PaymentMessage"] = "Tranasction Failed";
            return RedirectToAction("BookingCard", new { BookingId = bookingId });
        }

    }
}
