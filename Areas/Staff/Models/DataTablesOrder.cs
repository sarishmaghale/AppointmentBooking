namespace AppointmentBooking.Areas.Staff.Models
{
    public class DataTablesOrder
    {
        public int Column { get; set; } // Index of the column to order by
        public DataTablesOrderDir Dir { get; set; } // Direction of the ordering (asc or desc)
    }

    public enum DataTablesOrderDir
    {
        Asc,
        Desc
    }
}
