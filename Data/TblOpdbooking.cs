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
        public int? Department { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Contactno { get; set; }
        public string? Dob { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AgeType { get; set; }
        public int? PayStatus { get; set; }
        public decimal? Uhid { get; set; }
        public string? CreatedTime { get; set; }
        public int? ConsultantDr { get; set; }
        public int? RoomNo { get; set; }
        public string? FloorName { get; set; }
        public int? CaseType { get; set; }
        public double? Amount { get; set; }
    }
}
