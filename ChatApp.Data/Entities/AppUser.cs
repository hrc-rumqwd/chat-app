using Microsoft.AspNetCore.Identity;

namespace ChatApp.Data.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string? Alias { get; set; }
    }
}
