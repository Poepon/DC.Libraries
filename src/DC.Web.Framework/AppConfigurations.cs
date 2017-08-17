using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace CX.Web
{
    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfiguration> ConfigurationCache;

        static AppConfigurations()
        {
            ConfigurationCache = new ConcurrentDictionary<string, IConfiguration>();
        }

        public static IConfiguration Get(string path, string environmentName = null)
        {
            var cacheKey = path + "#" + environmentName;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, environmentName)
            );
        }

        public static IConfiguration GetTheme(string path)
        {
            var cacheKey = path;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => BuildTheme(path)
            );
        }

        public static IConfiguration GetFullPath(string fullpath)
        {
            var cacheKey = fullpath;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => new ConfigurationBuilder().AddJsonFile(fullpath, true, true).Build()
            );
        }

        private static IConfiguration BuildConfiguration(string path, string environmentName = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            return builder.Build();
        }

        private static IConfiguration BuildTheme(string path)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("theme.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}