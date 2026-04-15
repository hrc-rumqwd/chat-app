using ChatApp.Data.Entities;
using ChatApp.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure.Identity
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedIdentityDataAsync(this IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            RoleManager<AppRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] requiredRoles = [AppRoles.Admin, AppRoles.User];

            foreach (string roleName in requiredRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    _ = await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }

            List<AppUser> users = userManager.Users.OrderBy(x => x.Id).ToList();
            foreach (AppUser user in users)
            {
                IList<string> roles = await userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                {
                    _ = await userManager.AddToRoleAsync(user, AppRoles.User);
                }
            }

            bool hasAdmin = await userManager.GetUsersInRoleAsync(AppRoles.Admin) is { Count: > 0 };
            if (!hasAdmin && users.Count > 0)
            {
                AppUser firstUser = users[0];
                _ = await userManager.AddToRoleAsync(firstUser, AppRoles.Admin);
            }
        }
    }
}
