using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class FolderState
    {
        [Key]
        public int FolderId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StateObjectTraceroute> Obj_id { get; set; }
        //public List<ObjectTraceroute> obj_id { get; set; }
    }
}
