using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("RequestTime")]
    public class RequestTime
    {
        public int Id { get; set; }
        public string ListTime { get; set; }
    }
}
