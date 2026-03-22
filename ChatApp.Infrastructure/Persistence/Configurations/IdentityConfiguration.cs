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
            builder.Entity<AppUser>(cfg =>
            {
                cfg.ToTable("Users");
                cfg.HasKey(k => k.Id);
                cfg.Property(p => p.FullName).IsRequired();
                cfg.Property(p => p.Dob).IsRequired();

                cfg.ApplyAuditableEntityConfiguration();
                cfg.ApplyRemovableEntityConfiguration();
            });

            builder.Entity<AppRole>(cfg =>
            {
                cfg.ToTable("Roles");
                cfg.HasKey(k => k.Id);
            });

            builder.Entity<IdentityUserRole<long>>(cfg =>
            {
                cfg.ToTable("UserRoles");
                cfg.HasKey(k => new {k.UserId, k.RoleId});
            });

            builder.Entity<IdentityRoleClaim<long>>(cfg =>
            {
                cfg.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserClaim<long>>(cfg =>
            {
                cfg.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<long>>(cfg =>
            {
                cfg.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<long>>(cfg =>
            {
                cfg.ToTable("UserTokens");
            });
        }
    }
}
