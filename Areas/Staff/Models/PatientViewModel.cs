﻿using AppointmentBooking.Data;
using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class PatientViewModel
    {
        public decimal? Uhid { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Age is required.")]

        public int? Age { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        public string? Dob { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string? Contactno { get; set; }

        [Required(ErrorMessage = "Please select Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Please sleect District")]
        public string? District { get; set; }


        [Required(ErrorMessage = "Please select Ethnicity")]
        public string? Ethnicity { get; set; }
        public int? Panno { get; set; }
        public string? Email { get; set; }
        public int? IsDelete { get; set; }
        public string? CreatedDate { get; set; }
        public string? CreatedTime { get; set; }
        public string? CreatedByUser { get; set; }


        [Required(ErrorMessage = "Please select AgeType")]
        public string? AgeType { get; set; }
        public string? InsuranceId { get; set; }
        public string? Department { get; set; }
        public string? Religion { get; set; }
        public string? Remarks { get; set; }
        public string? PatientName { get; set; }

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoomNo { get; set; }
        public string? FloorName { get; set; }
        public DateTime? SearchDate { get; set; }
		
		public virtual TblDepartment? Departments { get; set; }
        public List<PatientHistoryItem>? HistoryItems { get; set; }
        public IEnumerable<PatientFeedbackViewModel>? Feedbacks { get; set; }
    }

    public class PatientHistoryItem
    {
        public decimal? PatientId { get; set; }
        public DateTime? HistoryDate { get; set; }
        public string? Type { get; set; }
        public string? ConsultantDr { get; set; }
        public int? RecordNo { get; set; }
        public int? TypeId { get; set; }
        public double? BillAmount { get; set; }
        public int? Department { get; set; }
    }
    public class PatientFeedbackViewModel
    {
   
        public string? FeedbackText { get; set; }
        public decimal? Uhid { get; set; }
        public string? PatientName { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
