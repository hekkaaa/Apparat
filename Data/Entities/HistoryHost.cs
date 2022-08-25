using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("HistoryHost")]
    public class HistoryHost
    {
        public int Id { get; set; }
        public string Hostname { get; set; }

    }
}
