﻿using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class OpdviewTable
    {
        public string? CaseTypeName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DoctorName { get; set; }
        public int SrNo { get; set; }
        public decimal? Uhid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Contactno { get; set; }
        public string? Dob { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? AgeType { get; set; }
        public string? Religion { get; set; }
        public decimal? RegNo { get; set; }
        public int? Opdqueue { get; set; }
        public int? RoomNo { get; set; }
        public string? FloorName { get; set; }
        public string? PayType { get; set; }
        public int? FeeType { get; set; }
        public double? Amount { get; set; }
        public int? RefererName { get; set; }
        public double? PaidAmt { get; set; }
        public string? CreatedDate { get; set; }
        public string? CreatedTime { get; set; }
        public string? CreatedByUser { get; set; }
    }
}
