using System;
using System.IO;
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

        }

        public static IApplicationBuilder UseFramework(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            return app.UseSession();
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

