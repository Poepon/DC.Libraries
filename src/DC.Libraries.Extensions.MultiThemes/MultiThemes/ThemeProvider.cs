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
        private readonly IThemeConfigStore themeConfigStore;

        #endregion

        #region Constructors

        public ThemeProvider(IOptionsMonitor<ThemeConfiguration> options, IThemeConfigStore themeConfigStore)
        {
            _themeConfiguration = options.CurrentValue;
            this.themeConfigStore = themeConfigStore;
        }

        #endregion


        #region Methods

        public ThemeItem GetWorkingTheme(bool isMobile, string domain)
        {
            if (_themeConfiguration.Themes != null && _themeConfiguration.Themes.Count > 0 || themeConfigStore.GetThemes().Count > 0)
            {
                ThemeItem curTheme = null;
                if (curTheme == null)
                {
                    curTheme = GetStoreTheme(isMobile,domain);
                }
                if (curTheme == null && _themeConfiguration.Themes != null && _themeConfiguration.Themes.Count > 0)
                {
                    var query = _themeConfiguration.Themes.OrderBy(t => t.Order).Where(t =>
                    t.SupportDomainAdapter && t.Domains.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Any(s => s == domain || t.SupportRegex && new Regex(s).IsMatch(domain)));
                    curTheme = curTheme = query.FirstOrDefault(t => t.Order == 1);
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
                }
                return curTheme;
            }
            return null;
        }

        private ThemeItem GetStoreTheme(bool isMobile, string domain)
        {
            return themeConfigStore.GetThemes().FirstOrDefault(t => ((isMobile && t.SupportMobile) || (!isMobile && t.SupportPC)) && t.Domains.Contains(domain));
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