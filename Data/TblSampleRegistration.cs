using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblSampleRegistration
    {
        public int SrNo { get; set; }
        public decimal? Uhid { get; set; }
        public int? RecordType { get; set; }
        public int? RecordNo { get; set; }
        public int? IsCollected { get; set; }
        public int? LabNo { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
