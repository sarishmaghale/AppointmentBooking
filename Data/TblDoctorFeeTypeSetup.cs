using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblDoctorFeeTypeSetup
    {
        public int DocFeeId { get; set; }
        public int? DoctorId { get; set; }
        public int? FeeTypeId { get; set; }
        public double? Fee { get; set; }

        public virtual TblDoctorSetup? Doctor { get; set; }
        public virtual TblFeeType? FeeType { get; set; }
    }
}
