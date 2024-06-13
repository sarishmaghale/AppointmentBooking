using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.Models
{
    public class OPDBookingViewModel
    {
        public int OpdbookingId { get; set; }
        [Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Booking Date is required.")]
        public string? BookingDate { get; set; }
        public string? InsuranceId { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "District is required.")]
        public string? District { get; set; }
        [Required(ErrorMessage = "ContactNumber is required.")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string? Contactno { get; set; }
        [Required(ErrorMessage = "DOB required is required.")]

        public string? Dob { get; set; }
       

        public int? Age { get; set; }
        public string? Panno { get; set; }

        [Required(ErrorMessage = "Gender is required.")]

        public string? Gender { get; set; }
        [Required(ErrorMessage = "Ethnicity is required.")]

        public string? Ethnicity { get; set; }
        public string? Email { get; set; }
        public string? CreatedDateTime { get; set; }
        public string? AgeType { get; set; }
        public int? PayStatus { get; set; }

        public string? amount { get; set; }
        public string? product_service_charge { get; set; }
        public string? product_delivery_charge { get; set; }
        public string? tax_amount { get; set; }
        public string? total_amount { get; set; }
        public string? transaction_uuid { get; set; }
        public string? product_code { get; set; } // Merchant Code
        public string? success_url { get; set; } // Success URL
        public string? failure_url { get; set; }
        public string? signed_field_names { get; set; }
        public string? signature { get; set; }

        public string? refid { get; set; }

    }
}
