using System.Reflection;
using System.Xml.Linq;

namespace AppointmentBooking.Models
{
    public class InsureePatientInfo
    {
        public List<Entry> Entry { get; set; }

    }
    public class Entry
    {
        public Resource Resource { get; set; }
    }
    public class Resource
    {
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Id { get; set; }
        public List<Name> Name { get; set; }
        public List<Telecom> Telecom { get; set; }
    }
    public class Name
    {
        public string Family { get; set; }
        public List<string> Given { get; set; }
    }
    public class Telecom
    {
        public string Family { get; set; }
    }
}
