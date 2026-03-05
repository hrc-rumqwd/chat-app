using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Commons.Abstractions;

namespace ChatApp.Data.Entities
{
    public class Message : BaseEntity<long>, IAuditableEntity
    {
        public string Content { get; set; }
        public long UserId { get; set; }
    }
}
