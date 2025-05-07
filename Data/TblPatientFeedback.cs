using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblPatientFeedback
    {
        public int Id { get; set; }
        public string? FeedbackText { get; set; }
        public decimal? Uhid { get; set; }
        public string? PatientName { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
