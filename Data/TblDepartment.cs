using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblDepartment
    {
        public TblDepartment()
        {
            TblDoctorSetups = new HashSet<TblDoctorSetup>();
            TblOpdregistrations = new HashSet<TblOpdregistration>();
        }

        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? RoomNo { get; set; }
        public string? FloorName { get; set; }

        public virtual ICollection<TblDoctorSetup> TblDoctorSetups { get; set; }
        public virtual ICollection<TblOpdregistration> TblOpdregistrations { get; set; }
    }
}
