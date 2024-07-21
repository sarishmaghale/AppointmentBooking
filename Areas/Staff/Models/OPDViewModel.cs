using AppointmentBooking.Data;
using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class OPDViewModel
    {
        public int SrNo { get; set; }
        public decimal? Uhid { get; set; }
        [Required(ErrorMessage = "FirstName is required.")]

        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]

        public string? LastName { get; set; }
        [Required(ErrorMessage = "Address is required.")]

        public string? Address { get; set; }
        [Required(ErrorMessage = "Please sleect District")]

        public string? District { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string? Contactno { get; set; }
        [Required(ErrorMessage = "DOB is required.")]

        public string? Dob { get; set; }
        [Required(ErrorMessage = "Age is required.")]

        public int? Age { get; set; }
        [Required(ErrorMessage = "Please select AgeType")]

        public string? AgeType { get; set; }
        [Required(ErrorMessage = "Please select Gender")]

        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public decimal? RegNo { get; set; }
        public int? Opdqueue { get; set; }
        [Required(ErrorMessage = "Please select Department")]

        public int? Department { get; set; }
        [Required(ErrorMessage = "Please select Consultant Doctor")]

        public int? ConsultantDr { get; set; }
        public int? RoomNo { get; set; }
        public string? FloorName { get; set; }
        [Required(ErrorMessage = "Please select CaseType")]

        public int? CaseType { get; set; }
        [Required(ErrorMessage = "Please select PayType")]

        public string? PayType { get; set; }


        public int? FeeType { get; set; }
        public double? Amount { get; set; }
        public double? PaidAmt { get; set; }
        public int? RefererName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedTime { get; set; }
        public string? CreatedByUser { get; set; }

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? FeeTypeName { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string? InsuranceId { get; set; }
        public int? Count { get; set; }
       
        public List<OPDViewModel>? opdReports { get; set; }
        public virtual TblCaseType? CaseTypeNavigation { get; set; }
        public virtual TblDoctorSetup? ConsultantDrNavigation { get; set; }
        public virtual TblDepartment? DepartmentNavigation { get; set; }
        public virtual TblPatientRegistration? Uh { get; set; }
    }
}
