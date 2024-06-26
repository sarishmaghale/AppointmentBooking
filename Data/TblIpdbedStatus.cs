using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblIpdbedStatus
    {
        public int BedId { get; set; }
        public string? BedName { get; set; }
        public int? BedTypeId { get; set; }
        public int? Status { get; set; }

        public virtual TblIpdbedType? BedType { get; set; }
    }
}
