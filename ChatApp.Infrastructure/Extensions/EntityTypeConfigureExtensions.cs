using ChatApp.Shared.Models.Commons.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Extensions
{
    public static class EntityTypeConfigureExtensions
    {
        public static void ApplyAuditableEntityConfiguration<TEntity>(this EntityTypeBuilder<TEntity> modelBuilder)
            where TEntity : class, IAuditableEntity
        {
            _ = modelBuilder.Property(e => e.CreatedAt)
                .IsRequired();

            _ = modelBuilder.Property(e => e.UpdatedAt)
                .IsRequired();

            _ = modelBuilder.Property(e => e.CreatedBy)
                .IsRequired(false);

            _ = modelBuilder.Property(e => e.UpdatedBy)
                .IsRequired(false);
        }

        public static void ApplyRemovableEntityConfiguration<TEntity>(this EntityTypeBuilder<TEntity> modelBuilder)
            where TEntity : class, IRemovableEntity
        {
            _ = modelBuilder.Property(e => e.IsDeleted)
                .IsRequired();

            _ = modelBuilder.Property(e => e.IsActived)
                .IsRequired();
        }
    }
}
