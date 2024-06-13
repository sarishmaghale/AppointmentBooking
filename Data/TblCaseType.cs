using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblCaseType
    {
        public TblCaseType()
        {
            TblOpdregistrations = new HashSet<TblOpdregistration>();
        }

        public int CaseTypeId { get; set; }
        public string? CaseTypeName { get; set; }

        public virtual ICollection<TblOpdregistration> TblOpdregistrations { get; set; }
    }
}
