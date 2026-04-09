using ChatApp.Application.Contracts.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly));
            _ = services.AddScoped<IBroker, Broker>();

            return services;
        }
    }
}
