using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DC.Libraries.Extensions.WeChat.Models;

namespace DC.Libraries.Extensions.WeChat
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWeChat(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeChatSetting>(configuration.GetSection("WeChatSetting"));
        }
    }
}
