using DC.Libraries.Extensions.Captcha.Contracts;
using DC.Libraries.Extensions.Captcha.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using DC.Libraries.Extensions.Captcha;

namespace Microsoft.Extensions.DependencyInjection
{
    public class CaptchaOptions
    {
        public StorageMode StorageMode { get; set; } = StorageMode.Session;

        public bool Enable { get; set; } = true;

    }
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for captcha support
        /// </summary>
        public static void AddCaptcha(this IServiceCollection services, CaptchaOptions options)
        {
            services.TryAddTransient<CaptchaImageController>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(options);
            switch (options.StorageMode)
            {
                case StorageMode.Session:
                    services.AddTransient<ICaptchaStorageProvider, SessionCaptchaStorageProvider>();
                    break;
                case StorageMode.Cookie:
                    services.AddTransient<ICaptchaStorageProvider, CookieCaptchaStorageProvider>();
                    break;
                case StorageMode.MemoryCache:
                    services.AddTransient<ICaptchaStorageProvider, MemoryCacheCaptchaStorageProvider>();
                    break;
                case StorageMode.DistributedCache:
                    services.AddTransient<ICaptchaStorageProvider, DistributedCacheCaptchaStorageProvider>();
                    break;
            }

            services.AddTransient<ICaptchaImageProvider, CaptchaImageProvider>();
            services.AddTransient<ICaptchaProtectionProvider, CaptchaProtectionProvider>();
            services.AddTransient<ICaptchaCodeGenerator, CaptchaCodeGenerator>();
            services.AddTransient<ICaptchaCodeMain, CaptchaCodeMain>();
            services.AddTransient<CaptchaTagHelper>();
        }
    }
    public enum StorageMode
    {
        Cookie = 1,
        Session = 2,
        MemoryCache = 3,
        DistributedCache = 4
    }
}
