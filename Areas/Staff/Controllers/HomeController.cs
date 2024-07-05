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

        private IRegistrationRepository registration;
        public HomeController(IRegistrationRepository _registration)
        {
            registration = _registration;
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
        public IActionResult Index()
        {
            
            return View();
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
