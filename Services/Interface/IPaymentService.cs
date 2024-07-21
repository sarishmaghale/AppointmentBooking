using AppointmentBooking.Models;

namespace AppointmentBooking.Services.Interface
{
    public interface IPaymentService
    {
        public string GenerateUniqueId(int id);
        public string GenerateSignature(string message);
        public Task<PaymentViewModel> ActivateEsewaPayment(int id,double?amount);
        public Task<OPDBookingViewModel> ValidateEsewaPayment(string encodedData);
        public Task<string> ActivateKhaltiPayment(int id, double? amount);
        public Task<OPDBookingViewModel> ValidateKhaltiPayment(string pidx, string Id);
    }
}
