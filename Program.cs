using AppointmentBooking.Models;
using AppointmentBooking.Services;
using AppointmentBooking.Services.Interface;
using AppointmentBooking.Services.Repository;
using AppointmentBooking.Data;
using Microsoft.EntityFrameworkCore;
using AppointmentBooking.Areas;
using AppointmentBooking.Areas.Staff.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Policy;
using NuGet.Common;


var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<MedicareAppointmentDbContext>(item => item.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<CommonUtility>();
builder.Services.AddTransient<ApiService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IOPDBookingService, OPDBookingService>();
builder.Services.AddTransient<AppointmentBooking.Areas.Staff.Services.Interface.IOPDRepository, AppointmentBooking.Areas.Staff.Services.Repository.OPDRespository>();
builder.Services.AddTransient<AppointmentBooking.Areas.Staff.Services.Interface.IOPDRepository, AppointmentBooking.Areas.Staff.Services.Repository.OPDRespository>();
builder.Services.AddTransient<AppointmentBooking.Areas.Staff.Services.Interface.IReceiptRepository, AppointmentBooking.Areas.Staff.Services.Repository.ReceiptRepository>();

builder.Services.AddScoped<AppointmentBooking.Areas.Staff.Services.Interface.IRegistrationRepo, AppointmentBooking.Areas.Staff.Services.Repository.RegistrationRepo>();
builder.Services.AddScoped<AppointmentBooking.Areas.Staff.Services.Interface.IComponentSetupRepo, AppointmentBooking.Areas.Staff.Services.Repository.ComponentSetupRepo>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://imis.hib.gov.np/api/api_fhir/Patient/?identifier="); // Set your base URL
    // You can configure other properties of HttpClient here
});

//session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Staff/Home/LogIn";
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Dashboard}/{id?}"
    );
    endpoints.MapControllerRoute(
         name: "default",
    pattern: "{controller=Appointment}/{action=AppointmentBooking}/{id?}"
        );
});
   
app.Run();
