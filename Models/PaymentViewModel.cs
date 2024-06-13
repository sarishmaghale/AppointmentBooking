using System.Drawing;

namespace AppointmentBooking.Models
{
    public class PaymentViewModel
    {
        public string? amount { get; set; }
        public string? purchaseId { get; set; }
        public string? successUrl { get; set; }
        public string? failureUrl { get; set; }

        //ESEWA
        public string? productDeliverCharge { get; set; }
        public string? productServiceCharge { get; set; }
        public string? taxAmount { get; set; }
        public string? totalAmount { get; set; }
        public string? merchantCode { get; set; }
        public string? signedFieldNames { get; set; }
        public string? signature { get; set; }

        //Khalti
        public string? purchaseOrderName { get; set; }
        public string? merchantName { get; set; }
        public string? merchantEmail { get; set; }
        public string? merhchantPhone { get; set; }

    }

}
