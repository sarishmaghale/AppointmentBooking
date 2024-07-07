using AppointmentBooking.Data;

namespace AppointmentBooking.Areas.Staff.Models
{
    public class IPDViewModel
    {
        public int IpdregNo { get; set; }
        public decimal? Uhid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Contactno { get; set; }
        public string? Dob { get; set; }
        public int? Age { get; set; }
        public string? AgeType { get; set; }
        public string? Gender { get; set; }
        public string? Religion { get; set; }
        public string? Gname { get; set; }
        public string? Gaddress { get; set; }
        public string? Gcontact { get; set; }
        public string? Grelation { get; set; }
        public string? PayType { get; set; }
        public double? AdmCharge { get; set; }
        public int? ConsultantDr { get; set; }
        public int? BedType { get; set; }
        public int? BedNo { get; set; }
        public int? CaseType { get; set; }
        public string? Complain { get; set; }
        public string? Diagnosis { get; set; }
        public string? Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IsDischarged { get; set; }
        public int? IPDBedTypeId { get; set; }
        public string? IPDBedName { get; set; }
        public double? IPDBedPrice { get; set; }
        public int? IPDStatus { get; set; }
        public string? CreatedByUser { get; set; }
        public int? OpdSrNo { get; set; }
        public string? PatientName { get; set; }
        public string? CaseTypeName { get; set; }
        public string? DoctorName { get; set; }
       
        public virtual TblPatientRegistration? Uh { get; set; }
    }
    public class IDPBedViewModel
    {
        public int? BedTypeId { get; set; }
        public string? BedTypeName { get; set; }
        public double? Price { get; set; }
        public int BedId { get; set; }
        public string? BedName { get; set; }
        public int? Status { get; set; }
        public List<IDPBedViewModel>? BedList { get; set; }
    }
}
