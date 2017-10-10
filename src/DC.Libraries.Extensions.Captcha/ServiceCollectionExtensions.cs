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
            services.TryAddSingleton<ICaptchaStorageProvider, SessionCaptchaStorageProvider>();
            services.TryAddSingleton<IHumanReadableIntegerProvider, HumanReadableIntegerProvider>();
            services.TryAddSingleton<IRandomNumberProvider, RandomNumberProvider>();
            services.TryAddSingleton<ICaptchaImageProvider, CaptchaImageProvider>();
            services.TryAddSingleton<ICaptchaProtectionProvider, CaptchaProtectionProvider>();
            services.TryAddSingleton<ICaptchaCodeGenerator, CaptchaCodeGenerator>();
            services.TryAddSingleton<ICaptchaCodeMain, CaptchaCodeMain>(); 
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<CaptchaTagHelper>();
        }
    }
}
