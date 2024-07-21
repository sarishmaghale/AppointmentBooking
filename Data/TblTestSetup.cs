using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblTestSetup
    {
        public TblTestSetup()
        {
            TblTestParameterMappings = new HashSet<TblTestParameterMapping>();
        }

        public int TestId { get; set; }
        public string? TestName { get; set; }
        public int? TestGroupId { get; set; }
        public double? TestPrice { get; set; }

        public virtual TblTestGroupSetup? TestGroup { get; set; }
        public virtual ICollection<TblTestParameterMapping> TblTestParameterMappings { get; set; }
    }
}
