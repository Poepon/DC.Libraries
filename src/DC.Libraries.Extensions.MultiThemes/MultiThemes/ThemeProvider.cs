using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace DC.Libraries.Extensions.MultiThemes
{
    public partial class ThemeProvider : IThemeProvider
    {
        #region Fields

        private readonly ThemeConfiguration _themeConfiguration = null;

        #endregion

        #region Constructors

        public ThemeProvider(IOptionsMonitor<ThemeConfiguration> options)
        {
            _themeConfiguration = options.CurrentValue;
        }

        #endregion


        #region Methods

        public ThemeItem GetWorkingTheme(bool isMobile, string domain)
        {
            if (_themeConfiguration.Themes == null || _themeConfiguration.Themes.Count == 0)
            {
                return null;
            }
            var query = _themeConfiguration.Themes.OrderBy(t => t.Order).Where(t =>
                  t.SupportDomainAdapter && t.Domains.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                      .Any(s => s == domain || t.SupportRegex && new Regex(s).IsMatch(domain)));
            ThemeItem curTheme = curTheme = query.FirstOrDefault(t => t.Order == 1);
            if (curTheme == null)
            {
                if (isMobile)
                {
                    curTheme = query.FirstOrDefault(t => t.SupportMobile == true);
                }
                else
                {
                    curTheme = query.FirstOrDefault(t => t.SupportPC == true);
                }
            }
            if (curTheme == null)
            {
                if (isMobile)
                {
                    curTheme = _themeConfiguration.Themes.FirstOrDefault(t => t.SupportMobile == true);
                }
                else
                {
                    curTheme = _themeConfiguration.Themes.FirstOrDefault(t => t.SupportPC == true);
                }
            }
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