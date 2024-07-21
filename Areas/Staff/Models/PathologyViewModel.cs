using Microsoft.EntityFrameworkCore.Storage;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class PathologyViewModel
    {
        public int? SrNo { get; set; }
        public decimal? Uhid { get; set; }
        public int? OpdNo { get; set; }
        public int? IpdNo { get; set; }
        public string? PatientName { get; set; }
        public int? RecordType { get; set; }
        public string? PatientType { get; set; }
        public int? RecordNo { get; set; }
        public int? isCollected { get; set; }
        public int?  LabNo{get;set;}
        public string? TestName { get; set; }

        public string? ContactNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ResultEntryDate { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
        public List<TestsListViewModel>? TestList{ get; set; }
        public string? ParameterName { get; set; }
        public string? Range { get; set; }
        public string? Unit { get; set; }
        public string? Result { get; set; }
        public int? TestId { get; set; }
        public string? Address { get; set; }
     public List<TestParameterViewModel>? LabResults { get; set; }
    }
    public class TestsListViewModel
    {
        public int? TestId { get; set; }
        public string? TestName { get; set; }
        public List<TestParameterViewModel>? TestParameterList { get; set; }
    }
    public class TestParameterViewModel
    {
        public int? ParamaterID { get; set; }
        public int? TestId { get; set; }
        public string? ParameterName { get; set; }
        public string? Range { get; set; }
        public string? Unit { get; set; }
        public string? TestName { get; set; }
        public string? Method { get; set; }
        public string? ParamResult { get; set; }
    }

}
