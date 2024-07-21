using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblTestParameterMapping
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int? ParamaterId { get; set; }

        public virtual TblLabParameterSetup? Paramater { get; set; }
        public virtual TblTestSetup? Test { get; set; }
    }
}
