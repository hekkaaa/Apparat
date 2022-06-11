using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    [Table("LossPackets")]
    public class Loss
    {
        public int Id { get; set; }
        public string Hostname { get; set; }

    }
}
