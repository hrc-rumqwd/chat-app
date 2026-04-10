using ChatApp.Shared.Models.Commons;

namespace ChatApp.Data.Entities
{
    public class Conversation : BaseEntity<long>
    {
        public bool IsGroup { get; set; }
        public string? Title { get; set; }
        public string? LastMessageContent { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string InvitationPath { get; set; }
        public ICollection<UserConversation> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
