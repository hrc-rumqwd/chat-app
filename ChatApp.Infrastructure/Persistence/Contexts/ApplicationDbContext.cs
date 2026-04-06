using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<
        AppUser,
        AppRole,
        long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        public DbSet<Message> Messages { get ; set ; }
        public DbSet<Conversation> Conversations { get ; set ; }
        public DbSet<UserConversation> UserConversations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            builder.ApplyIdentityConfiguration();
        }
    }
}
