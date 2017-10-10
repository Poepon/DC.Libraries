using Microsoft.Extensions.Configuration;
using DC.Libraries.Extensions.WeChat.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加微信配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddWeChatConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeChatSetting>(configuration.GetSection("WeChatSetting"));
        }
    }
}
