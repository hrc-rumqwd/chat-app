using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjector
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.AddIdentity<AppUser, AppRole>(cfg =>
            {
                cfg.Password.RequireDigit = true;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
                cfg.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<Persistence.Contexts.ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
