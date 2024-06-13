using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblDoctorSetup
    {
        public TblDoctorSetup()
        {
            TblDoctorFeeTypeSetups = new HashSet<TblDoctorFeeTypeSetup>();
            TblOpdregistrations = new HashSet<TblOpdregistration>();
        }

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public int? DepartmentId { get; set; }

        public virtual TblDepartment? Department { get; set; }
        public virtual ICollection<TblDoctorFeeTypeSetup> TblDoctorFeeTypeSetups { get; set; }
        public virtual ICollection<TblOpdregistration> TblOpdregistrations { get; set; }
    }
}
