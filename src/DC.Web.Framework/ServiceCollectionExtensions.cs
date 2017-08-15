using System;
using System.IO;
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
    public static class ServiceCollectionExtensions
    {
        public static void AddFramework(this IServiceCollection services)
        {

            services.AddMemoryCache();

            services.AddSession(option => { option.IdleTimeout = TimeSpan.FromMinutes(30); });

            AddThemes(services);

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
        public static void AddThemes(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IThemeProvider, ThemeProvider>();
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<IThemeContext, ThemeContext>();

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
