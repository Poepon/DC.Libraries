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
        public static void AddCaptcha(this IServiceCollection services, StorageMode storageMode = StorageMode.Session)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            switch (storageMode)
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
