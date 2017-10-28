using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DC.Libraries.Extensions.Monitor
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMonitor(this IServiceCollection services)
        {
            services.TryAddTransient<WebSiteController>();
        }
    }
}
