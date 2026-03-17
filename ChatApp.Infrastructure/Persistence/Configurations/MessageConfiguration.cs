using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.Property(e => e.Content)
                .IsRequired();

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();
        }
    }
}
