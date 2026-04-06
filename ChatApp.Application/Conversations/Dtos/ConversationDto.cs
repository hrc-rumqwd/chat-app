namespace ChatApp.Application.Conversations.Dtos
{
    public class ConversationDto
    {
        /// <summary>
        /// Conversation Id
        /// </summary>
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string? DisplayImage { get; set; }
        public bool IsGroup { get; set; }
        /// <summary>
        /// ParticipantId is the Id of the other user in a one-on-one conversation
        /// </summary>
        public long? ParticipantId { get; set; }
        public bool IsOnline { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
