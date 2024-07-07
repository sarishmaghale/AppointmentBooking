using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblIpdexpenseEntry
    {
        public int ExpenseEntryId { get; set; }
        public int? IpdregNo { get; set; }
        public decimal? Uhid { get; set; }
        public string? TestGroup { get; set; }
        public string? TestName { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public double? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
