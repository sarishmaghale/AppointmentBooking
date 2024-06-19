using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblReceiptDetail
    {
        public int DetailsId { get; set; }
        public int? ReceiptNo { get; set; }
        public string? TestGroup { get; set; }
        public string? TestName { get; set; }
        public double? TestPrice { get; set; }
        public int? Quantity { get; set; }
        public double? Amount { get; set; }

        public virtual TblCashReceipt? ReceiptNoNavigation { get; set; }
    }
}
