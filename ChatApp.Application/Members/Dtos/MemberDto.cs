namespace ChatApp.Application.Members.Dtos
{
    public class MemberDto
    {
        public long Id { get; set; }
        public bool IsGroup { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
