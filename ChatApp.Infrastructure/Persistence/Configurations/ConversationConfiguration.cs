using ChatApp.Data.Entities;
using ChatApp.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            _ = builder.ToTable("Conversations");
            _ = builder.HasKey(g => g.Id);

            _ = builder.Property(g => g.Title)
                .HasMaxLength(100);

            _ = builder.HasMany(m => m.Messages)
                .WithOne(c => c.Conversation)
                .HasForeignKey(f => f.ConversationId);

            _ = builder.HasMany(m => m.Participants)
                .WithOne(c => c.Conversation)
                .HasForeignKey(f => f.ConversationId);

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();
        }
    }
}
