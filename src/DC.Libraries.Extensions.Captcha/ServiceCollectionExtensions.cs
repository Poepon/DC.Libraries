using DC.Libraries.Extensions.Captcha.Contracts;
using DC.Libraries.Extensions.Captcha.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using DC.Libraries.Extensions.Captcha;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for captcha support
        /// </summary>
        public static void AddCaptcha(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICaptchaStorageProvider, SessionCaptchaStorageProvider>();
            services.AddTransient<ICaptchaImageProvider, CaptchaImageProvider>();
            services.AddTransient<ICaptchaProtectionProvider, CaptchaProtectionProvider>();
            services.AddTransient<ICaptchaCodeGenerator, CaptchaCodeGenerator>();
            services.AddTransient<ICaptchaCodeMain, CaptchaCodeMain>(); 
            services.AddTransient<CaptchaTagHelper>();
        }
    }
}
