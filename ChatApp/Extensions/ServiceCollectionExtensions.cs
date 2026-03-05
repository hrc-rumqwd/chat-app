namespace ChatApp.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }
    }
}
