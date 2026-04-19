using ChatApp.Application.Contracts.Encoders;
using ChatApp.Application.Contracts.Services;
using ChatApp.Application.Contracts.Tokens;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Caching;
using ChatApp.Infrastructure.Encoders;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Presence;
using ChatApp.Infrastructure.Tokens;
using ChatApp.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjector
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.RegisterDbContext(configuration);
            _ = services.AddIdentity<AppUser, AppRole>(cfg =>
            {
                cfg.Password.RequireDigit = true;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
                cfg.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<Persistence.Contexts.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            _ = services.AddSingleton<IPresenceTracker, PresenceTracker>();


            // Caching setup
            _ = services.AddMemoryCache();
            _ = services.AddSingleton<ICacheService, MemoryCacheService>();

            services.AddKeyedSingleton<IApplicationEncoder, Crc32Encoder>(EncoderServiceKeys.CRC_32);
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
        }
    }
}
