using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblCashReceipt
    {
        public TblCashReceipt()
        {
            TblReceiptDetails = new HashSet<TblReceiptDetail>();
        }

        public int ReceiptNo { get; set; }
        public decimal? Uhid { get; set; }
        public int? Opdno { get; set; }
        public int? Ipdno { get; set; }
        public string? PayType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public string? CreatedByUser { get; set; }

        public virtual TblOpdregistration? OpdnoNavigation { get; set; }
        public virtual TblPatientRegistration? Uh { get; set; }
        public virtual ICollection<TblReceiptDetail> TblReceiptDetails { get; set; }
    }
}
