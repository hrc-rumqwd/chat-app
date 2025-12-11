using ChatApp.Shared.Models.Users.Dtos;

namespace ChatApp.Shared.Models.Messages.Dtos
{
    public class MessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
        public long SendBy { get; set; }
        public UserTinyViewDto Author { get; set; }
    }
}
