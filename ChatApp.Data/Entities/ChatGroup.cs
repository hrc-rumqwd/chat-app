using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Commons.Abstractions;

namespace ChatApp.Data.Entities
{
    public class ChatGroup : BaseEntity<long>
    {
        public string GroupName { get; set; }
    }
}
