using System.ComponentModel.DataAnnotations;
namespace AppointmentBooking.Areas.Staff.Models
{
    public class AccountViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]

        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]

        public string? Password { get; set; }

        [Required(ErrorMessage = "Shift is required.")]

        public string? Shift { get; set; }

        [Required(ErrorMessage = "Department is required.")]

        public string? Department { get; set; }
    }
}
