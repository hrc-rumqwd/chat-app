using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    public class ChatGroupConfiguration : IEntityTypeConfiguration<ChatGroup>
    {
        public void Configure(EntityTypeBuilder<ChatGroup> builder)
        {
            builder.ToTable("Groups");
            
            builder.Property(e => e.GroupName)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.ApplyAuditableEntityConfiguration();
            builder.ApplyRemovableEntityConfiguration();
        }
    }
}
