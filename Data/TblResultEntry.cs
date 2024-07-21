using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblResultEntry
    {
        public int Id { get; set; }
        public int? LabNo { get; set; }
        public int? TestId { get; set; }
        public string? TestName { get; set; }
        public string? ParameterName { get; set; }
        public string? Range { get; set; }
        public string? Unit { get; set; }
        public string? Result { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
