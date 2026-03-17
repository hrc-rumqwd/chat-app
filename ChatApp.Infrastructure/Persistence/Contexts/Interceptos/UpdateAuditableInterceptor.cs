using ChatApp.Shared.Models.Commons.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChatApp.Infrastructure.Persistence.Contexts.Interceptos
{
    public class UpdateAuditableInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                var auditEntities = eventData.Context.ChangeTracker.Entries<IAuditableEntity>();
                foreach (var entity in auditEntities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        entity.Entity.CreatedAt = DateTime.UtcNow;
                        entity.Entity.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        entity.Entity.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
