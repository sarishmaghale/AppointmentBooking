using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblIpdregistration
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
        public DateTime? CreatedDate { get; set; }
        public int? IsDischarged { get; set; }
        public string? CreatedByUser { get; set; }
        public int? OpdSrNo { get; set; }

        public virtual TblPatientRegistration? Uh { get; set; }
    }
}
