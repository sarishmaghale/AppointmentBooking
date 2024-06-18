using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblTestGroupSetup
    {
        public TblTestGroupSetup()
        {
            TblTestSetups = new HashSet<TblTestSetup>();
        }

        public int TestGroupId { get; set; }
        public string? TestGroupName { get; set; }

        public virtual ICollection<TblTestSetup> TblTestSetups { get; set; }
    }
}
