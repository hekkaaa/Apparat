using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("LossPackets")]
    public class Loss
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        [ConcurrencyCheck]
        public string? ListLoss { get; set; }
    }
}
