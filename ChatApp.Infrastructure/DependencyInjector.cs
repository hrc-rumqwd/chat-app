using ChatApp.Infrastructure.Brokers;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjector
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.RegisterDbContext(configuration);
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string must not be empty");
            services.AddDbContext<ApplicationDbContext>(cfg =>
                cfg.UseNpgsql(connectionString));

            services.AddSingleton<IBroker, Broker>();
        }
    }
}
