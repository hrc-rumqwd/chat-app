using ChatApp.Infrastructure.Brokers;
using ChatApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjector
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.RegisterSignalR();

            services.AddSingleton<IBroker, Broker>();
        }
    }
}
