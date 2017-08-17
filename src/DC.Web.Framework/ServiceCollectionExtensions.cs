using System;
using System.IO;
using CX.Web.Captcha;
using CX.Web.Captcha.Contracts;
using CX.Web.Captcha.Providers;
using CX.Web.Tenants;
using CX.Web.Themes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CX.Web
{
    public class FrameworkOption
    {
        /// <summary>
        /// The IdleTimeout indicates how long the session can be idle before its contents are abandoned. Each session access
        /// resets the timeout. Note this only applies to the content of the session, not the cookie.
        /// </summary>
        public TimeSpan SessionIdleTimeout { get; set; }

        /// <summary>
        /// The maximim amount of time allowed to load a session from the store or to commit it back to the store.
        /// Note this may only apply to asynchronous operations. This timeout can be disabled using <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.
        /// </summary>
        public TimeSpan SessionIOTimeout { get; set; }

        /// <summary>
        /// Set True distinguish PC or Mobile,Set False not distinguish,default True
        /// </summary>
        public bool DistinguishDevice { get; set; } = true;
    }
    public static class ServiceCollectionExtensions
    {
        public static void AddFramework(this IServiceCollection services, FrameworkOption frameworkOption = null)
        {

            services.AddMemoryCache();

            services.AddSession(option =>
            {
                option.IdleTimeout = frameworkOption?.SessionIdleTimeout ?? option.IdleTimeout;
                option.IOTimeout = frameworkOption?.SessionIOTimeout ?? option.IOTimeout;
            });

            AddThemes(services, frameworkOption?.DistinguishDevice ?? true);

            AddCaptcha(services);

        }

        public static IApplicationBuilder UseFramework(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            return app.UseSession();
        }


        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="distinguishDevice">Set True distinguish PC or Mobile,Set False not distinguish</param>
        public static void AddThemes(this IServiceCollection services, bool distinguishDevice)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddTransient<IThemeProvider, ThemeProvider>();
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<IThemeContext, ThemeContext>();
            DeviceSupport.DistinguishDevice = distinguishDevice;
            //themes support
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }

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
            services.AddTransient<CaptchaTagHelper>();
        }

        /// <summary>
        /// Initializing static pages
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static void InitStaticPage(this IServiceCollection services, IHostingEnvironment env)
        {
            var dir = Path.Combine(env.ContentRootPath, "wwwroot", "staticPages");

            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
        }

    }
}

