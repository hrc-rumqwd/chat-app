namespace ChatApp.Shared.Models.Users.Dtos
{
    public class UserTinyViewDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Alias { get; set; }
        public string DisplayName => string.IsNullOrEmpty(Alias)
            ? FullName
            : string.Join(" ", FullName, $"({Alias})");
    }
}
