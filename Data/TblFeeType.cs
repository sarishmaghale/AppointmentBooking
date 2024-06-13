using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblFeeType
    {
        public TblFeeType()
        {
            TblDoctorFeeTypeSetups = new HashSet<TblDoctorFeeTypeSetup>();
        }

        public int FeeTypeId { get; set; }
        public string? FeeTypeName { get; set; }

        public virtual ICollection<TblDoctorFeeTypeSetup> TblDoctorFeeTypeSetups { get; set; }
    }
}
