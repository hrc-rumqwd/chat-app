using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    public class UserConversationConfiguration : IEntityTypeConfiguration<UserConversation>
    {
        public void Configure(EntityTypeBuilder<UserConversation> builder)
        {
            _ = builder.ToTable("UserConversations");

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();

            _ = builder.HasOne(c => c.User)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(f => f.UserId);
        }
    }
}
