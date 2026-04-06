using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data.Entities
{
    [Index(nameof(CreatedAt), Name = "Messages_CreatedAt_idx")]
    public class Message : BaseEntity<long>
    {
        public string Content { get; set; }
        public long SenderId { get; set; }
        public AppUser Sender { get; set; }
        public long? ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
