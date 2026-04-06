using ChatApp.Shared.Models.Commons.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Data.Entities
{
    public class AppUser : IdentityUser<long>, IAuditableEntity, IRemovableEntity
    {
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string? Alias { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActived { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; }
    }
}
