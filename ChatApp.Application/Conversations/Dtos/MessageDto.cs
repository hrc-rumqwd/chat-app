namespace ChatApp.Application.Conversations.Dtos
{
    public class MessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
        public long SendBy { get; set; }
        public MessageAuthorDto Author { get; set; }
    }
}
