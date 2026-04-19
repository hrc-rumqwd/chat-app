using ChatApp.Data.Entities;
using ChatApp.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            _ = builder.ToTable("Messages");

            _ = builder.Property(e => e.Content)
                .IsRequired();

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();

            _ = builder.HasOne(e => e.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(f => f.SenderId);

            _ = builder.HasOne(e => e.Conversation)
                .WithMany(u => u.Messages)
                .HasForeignKey(f => f.ConversationId);
        }
    }
}
