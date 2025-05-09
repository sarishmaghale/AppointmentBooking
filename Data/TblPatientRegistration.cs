﻿using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblPatientRegistration
    {
        public TblPatientRegistration()
        {
            TblCashReceipts = new HashSet<TblCashReceipt>();
            TblIpdregistrations = new HashSet<TblIpdregistration>();
            TblOpdregistrations = new HashSet<TblOpdregistration>();
        }

        public decimal Uhid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; }
        public string? Dob { get; set; }
        public string? Contactno { get; set; }
        public string? Gender { get; set; }
        public string? District { get; set; }
        public string? Ethnicity { get; set; }
        public int? IsDelete { get; set; }
        public string? CreatedDate { get; set; }
        public string? CreatedTime { get; set; }
        public string? CreatedByUser { get; set; }
        public string? AgeType { get; set; }

        public virtual ICollection<TblCashReceipt> TblCashReceipts { get; set; }
        public virtual ICollection<TblIpdregistration> TblIpdregistrations { get; set; }
        public virtual ICollection<TblOpdregistration> TblOpdregistrations { get; set; }
    }
}
