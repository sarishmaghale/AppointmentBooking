using AppointmentBooking.Data;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class ReceiptViewModel
    {      
 
        public int? ReceiptNo { get; set; }
        public decimal? Uhid { get; set; }
        public int? Opdno { get; set; }
        public int? Ipdno { get; set; }
        public string? PayType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
        public string? DoctorName { get; set; }
        public List<ReceiptDetails> ReceiptDetails { get; set; }
       
       }
    public class ReceiptDetails
    {
        public int? TestId { get; set; }
        public string? TestName { get; set; }
        public int? TestGroupId { get; set; }
        public double? TestPrice { get; set; }
        public string? TestGroup { get; set; }
        public int? Quantity { get; set; }
        public double? Amount { get; set; }
        public virtual TblCashReceipt? ReceiptNoNavigation { get; set; }
        public virtual TblTestGroupSetup? TestGroups
        {
            get; set;

        }
        public virtual ICollection<TblTestSetup> TblTestSetups { get; set; }
        public string? TestGroupName { get; set; }
    }
}
