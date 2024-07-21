using AppointmentBooking.Data;
using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class ReceiptViewModel
    {      
 
        public int? ReceiptNo { get; set; }
        public decimal? Uhid { get; set; }
        public int? Opdno { get; set; }
        public int? Ipdno { get; set; }
        public string? PayType { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByUser { get; set; }
        public double? TotalAmount { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
        public string? DoctorName { get; set; }
        public List<ReceiptDetails> ReceiptDetails { get; set; }

        //for cash summary form
        public int? GroupFilter { get; set; }
        public string? Gender { get; set; }
        public string? UserFilter { get; set; }
        public string? PayTypeFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string? TestGroupDept { get; set; }
        public List<ReceiptViewModel>? Receipts { get; set; }
        public double? ReceiptTotal { get; set; }
        public List<ReceiptViewModel>? Results { get; set; }
       
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
        public int? IpdRegNo { get; set; }
        public virtual TblCashReceipt? ReceiptNoNavigation { get; set; }
        public virtual TblTestGroupSetup? TestGroups
        {
            get; set;

        }
        public virtual ICollection<TblTestSetup>? TblTestSetups { get; set; }
        public List<TestParameterViewModel>? TestParameterList { get; set; }
        public string? TestGroupName { get; set; }
    }

    public class ExpenseEntryViewModel
    {
        public int? ExpenseEntryId { get; set; }
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
