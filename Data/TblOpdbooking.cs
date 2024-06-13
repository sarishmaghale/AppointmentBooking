using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblOpdbooking
    {
        public int OpdbookingId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? BookingDate { get; set; }
        public string? InsuranceId { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Contactno { get; set; }
        public string? Dob { get; set; }
        public int? Age { get; set; }
        public string? Panno { get; set; }
        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public string? Email { get; set; }
        public string? CreatedDateTime { get; set; }
        public string? AgeType { get; set; }
        public int? PayStatus { get; set; }
    }
}
