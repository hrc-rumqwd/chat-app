using ChatApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Contracts.DbContext
{
    public interface IApplicationDbContext
    {
        DbSet<AppUser> Users { get; set; }
        DbSet<AppRole> Roles { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Conversation> Conversations { get; set; }
        DbSet<UserConversation> UserConversations { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
