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

            services.AddScoped<UpdateAuditableInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, cfg) =>
                cfg.UseNpgsql(connectionString)
                    .AddInterceptors(sp.GetRequiredService<UpdateAuditableInterceptor>()));

            return services;
        }
    }
}
