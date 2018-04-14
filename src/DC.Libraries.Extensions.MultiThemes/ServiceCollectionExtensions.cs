using DC.Libraries.Extensions.MultiThemes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="distinguishDevice">Set True distinguish PC or Mobile,Set False not distinguish</param>
        public static void AddThemes(this IServiceCollection services, IConfiguration configuration, Type themeConfigStoreType = null)
        {
            services.Configure<ThemeConfiguration>(configuration.GetSection("ThemeSetting"));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IThemeProvider, ThemeProvider>();
            if (themeConfigStoreType != null && typeof(IThemeConfigStore).IsAssignableFrom(themeConfigStoreType))
            {
                services.AddTransient(typeof(IThemeConfigStore), themeConfigStoreType);
            }
            else
            {
                services.AddTransient<IThemeConfigStore, NullThemeConfigStore>();
            }
            //themes support
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }
    }
}
