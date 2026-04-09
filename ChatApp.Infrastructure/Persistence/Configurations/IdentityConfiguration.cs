using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Configurations
{
    public static class IdentityConfiguration
    {
        public static void ApplyIdentityConfiguration(this ModelBuilder builder)
        {
            _ = builder.Entity<AppUser>(cfg =>
            {
                _ = cfg.ToTable("Users");
                _ = cfg.HasKey(k => k.Id);
                _ = cfg.Property(p => p.FullName).IsRequired();
                _ = cfg.Property(p => p.Dob).IsRequired();

                cfg.ApplyAuditableEntityConfiguration();
                cfg.ApplyRemovableEntityConfiguration();
            });

            _ = builder.Entity<AppRole>(cfg =>
            {
                _ = cfg.ToTable("Roles");
                _ = cfg.HasKey(k => k.Id);
            });

            _ = builder.Entity<IdentityUserRole<long>>(cfg =>
            {
                _ = cfg.ToTable("UserRoles");
                _ = cfg.HasKey(k => new { k.UserId, k.RoleId });
            });

            _ = builder.Entity<IdentityRoleClaim<long>>(cfg =>
            {
                _ = cfg.ToTable("RoleClaims");
            });

            _ = builder.Entity<IdentityUserClaim<long>>(cfg =>
            {
                _ = cfg.ToTable("UserClaims");
            });

            _ = builder.Entity<IdentityUserLogin<long>>(cfg =>
            {
                _ = cfg.ToTable("UserLogins");
            });

            _ = builder.Entity<IdentityUserToken<long>>(cfg =>
            {
                _ = cfg.ToTable("UserTokens");
            });
        }
    }
}
