using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblIpdbedType
    {
        public TblIpdbedType()
        {
            TblIpdbedStatuses = new HashSet<TblIpdbedStatus>();
        }

        public int BedTypeId { get; set; }
        public string? BedTypeName { get; set; }
        public double? Price { get; set; }

        public virtual ICollection<TblIpdbedStatus> TblIpdbedStatuses { get; set; }
    }
}
