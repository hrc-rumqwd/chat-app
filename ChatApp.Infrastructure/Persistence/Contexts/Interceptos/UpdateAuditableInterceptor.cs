using ChatApp.Shared.Models.Commons.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChatApp.Infrastructure.Persistence.Contexts.Interceptos
{
    public class UpdateAuditableInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DateTime now = DateTime.UtcNow;
            if (eventData.Context is not null)
            {
                IEnumerable<EntityEntry<IAuditableEntity>> auditEntities = eventData.Context.ChangeTracker.Entries<IAuditableEntity>();
                foreach (EntityEntry<IAuditableEntity> entity in auditEntities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        entity.Entity.CreatedAt = now;
                        entity.Entity.UpdatedAt = now;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        entity.Entity.UpdatedAt = now;
                    }
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
