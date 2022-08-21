using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("HostnameTracerouteObject")]
    public class StateObjectTraceroute
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
    }
}
