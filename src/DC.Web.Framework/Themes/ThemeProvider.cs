using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace CX.Web.Themes
{
    public partial class ThemeProvider : IThemeProvider
    {
        #region Fields

        private readonly IList<ThemeConfiguration> _themeConfigurations = new List<ThemeConfiguration>();
        private readonly string _basePath;

        #endregion

        #region Constructors

        public ThemeProvider(IHostingEnvironment env)
        {
            var path = "~/Themes/";
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            _basePath = Path.Combine(env.ContentRootPath, path);
            LoadConfigurations();
        }

        #endregion

        #region Utility

        private void LoadConfigurations()
        {
            foreach (string themeName in Directory.GetDirectories(_basePath))
            {
                var configuration = CreateThemeConfiguration(themeName);
                if (configuration != null)
                {
                    _themeConfigurations.Add(configuration);
                }
            }
        }

        private ThemeConfiguration CreateThemeConfiguration(string themePath)
        {
            var themeDirectory = new DirectoryInfo(themePath);
            var themeConfigFile = new FileInfo(Path.Combine(themeDirectory.FullName, "theme.json"));

            if (themeConfigFile.Exists)
            {
                return new ThemeConfiguration(themeDirectory.Name, themeDirectory.FullName);
            }

            return null;
        }

        #endregion

        #region Methods

        public ThemeConfiguration GetThemeConfiguration(string themeName)
        {
            return _themeConfigurations.SingleOrDefault(x => x.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IList<ThemeConfiguration> GetThemeConfigurations()
        {
            return _themeConfigurations;
        }

        public bool ThemeConfigurationExists(string themeName)
        {
            return GetThemeConfigurations().Any(configuration => configuration.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}