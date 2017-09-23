using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CX.Web.Themes
{
    public partial class ThemeProvider : IThemeProvider
    {
        #region Fields

        private readonly ThemeConfiguration _themeConfiguration = null;

        #endregion

        #region Constructors

        public ThemeProvider(IHostingEnvironment env)
        {
            var config = AppConfigurations.GetFullPath(Path.Combine(env.ContentRootPath, "appsettings.json"));
            _themeConfiguration = config.GetSection("ThemeSetting").Get<ThemeConfiguration>();
        }

        #endregion


        #region Methods

        public ThemeItem GetWorkingTheme(bool isMobile, string domain)
        {

            var query = _themeConfiguration.Themes.Where(t =>
            ((t.SupportMobile == true && isMobile == true) || (t.SupportPC == true && isMobile == false)));

            var curTheme = query.FirstOrDefault(t =>
            t.SupportDomainAdapter && t.Domains.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Contains(domain));

            if (curTheme == null)
            {
                curTheme = _themeConfiguration.Themes.FirstOrDefault(t =>
                    t.ThemeName == _themeConfiguration.DefaultTheme);
            }

            return curTheme;
        }

        public ThemeItem GetTheme(string themeName)
        {
            return _themeConfiguration.Themes.SingleOrDefault(x => x.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IList<ThemeItem> GetThemes()
        {
            return _themeConfiguration.Themes;
        }

        public bool ThemeExists(string themeName)
        {
            return GetThemes().Any(configuration => configuration.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}