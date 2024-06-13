using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblTransactionStatus
    {
        public int TransactionId { get; set; }
        public int? OpdbookingId { get; set; }
        public string? RefId { get; set; }
        public double? Amount { get; set; }
    }
}
