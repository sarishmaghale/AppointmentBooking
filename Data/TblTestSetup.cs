using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblTestSetup
    {
        public int TestId { get; set; }
        public string? TestName { get; set; }
        public int? TestGroupId { get; set; }
        public double? TestPrice { get; set; }

        public virtual TblTestGroupSetup? TestGroup { get; set; }
    }
}
