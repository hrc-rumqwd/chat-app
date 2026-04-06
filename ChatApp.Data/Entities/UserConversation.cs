using ChatApp.Shared.Models.Commons;

namespace ChatApp.Data.Entities
{
    public class UserConversation : BaseEntity<long>
    {
        public long UserId { get; set; }
        public string UserDisplayName { get; set; }
        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public DateTime? HiddenBefore { get; set; }
        public AppUser User { get; set; }
    }
}
