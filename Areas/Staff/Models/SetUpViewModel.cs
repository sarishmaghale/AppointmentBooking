using AppointmentBooking.Data;
using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class SetUpViewModel
    {



    }
    public class DoctorSetupViewModel
    {
        public int DocFeeId { get; set; }
        public int? DoctorId { get; set; }  
     
        public string? DoctorName { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public List<DoctorFeeViewModel>? FeeDetails { get; set; }
        public List<DoctorFeeViewModel>? ExistingFees { get; set; }
        public virtual TblDoctorSetup? Doctor { get; set; }
        public virtual TblDepartment? Department { get; set; }
        public virtual TblFeeType? FeeType { get; set; }
    }
    public class DoctorFeeViewModel
    {
        public int? FeeTypeId { get; set; }
        public string? FeeTypeName { get; set; }
        public double? Fee { get; set; }
    }
    public class TestSetupViewModel
    {
        public int? TestGroupId { get; set; }
        public string? TestGroupName { get; set; }
        public int? TestId { get; set; }
        public string? TestName { get; set; }
        public double? TestPrice { get; set; }
    }
    public class BedSetupViewModel
    {
        public int? BedId { get; set; }
        public string? BedName { get; set; }
        public int? BedCategoryId { get; set; }
        public string? BedCategoryName { get; set; }
        public double? BedPrice { get; set; }
    }
    public class ParameterSetupViewModel
    {
        public int? Parameterid { get; set; }
        public string? ParameterName { get; set; }
        public string? Range { get; set; }
        public string? Unit { get; set; }
        public string? Method { get; set; }
        public int? TestId { get; set; }
    }
}
