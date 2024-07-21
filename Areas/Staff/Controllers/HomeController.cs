using AppointmentBooking.Areas.Staff.Models;
using AppointmentBooking.Areas.Staff.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentBooking.Areas.Staff.Controllers
{
   
    [Area("Staff")]

    public class HomeController : Controller
    {

        private IRegistrationRepository registration;private IIPDRepository ipdProvider;private IOPDRepository opdProvider;
        private IReceiptRepository receiptProvider;
        public HomeController(IRegistrationRepository _registration,IIPDRepository _ipdProvider,IOPDRepository _opdProvider,
            IReceiptRepository _receiptProvider)
        {
            registration = _registration;
            ipdProvider = _ipdProvider;
            opdProvider = _opdProvider;
            receiptProvider = _receiptProvider;
        }
       

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
               var user=await registration.CheckUserAccount(model);
                if (user!=null)
                {
                    var claims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, user.Username),
                       new Claim("UserId", user.UserId.ToString()),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    HttpContext.Session.SetString("UserName", user.Username);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserShift", user.Shift);
                    HttpContext.Session.SetString("UserDept", user.Department);
                    return RedirectToAction("Index");
                }

            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
            
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
       public async Task<IActionResult> GetPatientCount()
        {
            var ipdCount = await ipdProvider.GetIPDPatientCount();
            var opdCount = await opdProvider.GetOpdPatientCount();
            return Json(new { opdCount, ipdCount });
        }
        public async Task<List<object>> GetSalesData()
        {
            var data = await receiptProvider.GetSalesData();
            return data;
        }
        [Authorize]
        public IActionResult Dashboard()
        {
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "Home");
        }
    }
}
