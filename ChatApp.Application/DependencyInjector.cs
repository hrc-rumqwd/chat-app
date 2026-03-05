using ChatApp.Application.Contracts;
using ChatApp.Application.Contracts.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly));
            services.AddScoped<IBroker, Broker>();

            return services;
        }
    }
}
