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

        public int? FeeTypeId { get; set; }

        public double? Fee { get; set; }


        public string? DoctorName { get; set; }


        public int? DepartmentId { get; set; }
        public List<DoctorSetupViewModel>? FeeDetails { get; set; }
        public virtual TblDoctorSetup? Doctor { get; set; }
        public virtual TblDepartment? Department { get; set; }
        public virtual TblFeeType? FeeType { get; set; }
    }
}
