namespace ChatApp.Application.Users.Shared.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string Alias { get; set; }
    }
}
