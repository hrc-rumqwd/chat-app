using ChatApp.Shared.Models.Commons;

namespace ChatApp.Data.Entities
{
    public class Group : BaseEntity<long>
    {
        public string Name { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
