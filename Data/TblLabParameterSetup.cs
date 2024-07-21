using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblLabParameterSetup
    {
        public TblLabParameterSetup()
        {
            TblTestParameterMappings = new HashSet<TblTestParameterMapping>();
        }

        public int ParameterId { get; set; }
        public string? ParameterName { get; set; }
        public string? Range { get; set; }
        public string? Unit { get; set; }
        public string? Method { get; set; }

        public virtual ICollection<TblTestParameterMapping> TblTestParameterMappings { get; set; }
    }
}
