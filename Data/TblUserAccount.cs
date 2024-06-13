using System;
using System.Collections.Generic;

namespace AppointmentBooking.Data
{
    public partial class TblUserAccount
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Shift { get; set; }
        public string? Department { get; set; }
    }
}
