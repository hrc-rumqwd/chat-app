using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Infrastructure.Persistence.Contexts.Interceptos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string must not be empty");

            _ = services.AddScoped<UpdateAuditableInterceptor>();

            _ = services.AddDbContext<ApplicationDbContext>((sp, cfg) =>
            {
                _ = cfg.UseNpgsql(connectionString)
                    .AddInterceptors(sp.GetRequiredService<UpdateAuditableInterceptor>());
                _ = cfg.EnableSensitiveDataLogging();
                _ = cfg.EnableDetailedErrors();
            });

            return services;
        }
    }
}
