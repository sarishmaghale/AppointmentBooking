using AppointmentBooking.Data;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class ReceiptViewModel
    {      
        public string? TestGroupName { get; set; }

        public virtual ICollection<TblTestSetup> TblTestSetups { get; set; }
        public int? TestId { get; set; }
        public string? TestName { get; set; }
        public int? TestGroupId { get; set; }
        public double? TestPrice { get; set; }

        public virtual TblTestGroupSetup? TestGroup { get; set; }
    }
}
