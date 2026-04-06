using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Title)
                .HasMaxLength(100);

            builder.HasMany(m => m.Messages)
                .WithOne(c => c.Conversation)
                .HasForeignKey(f => f.ConversationId);

            builder.HasMany(m => m.Participants)
                .WithOne(c => c.Conversation)
                .HasForeignKey(f => f.ConversationId);

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();
        }
    }
}
