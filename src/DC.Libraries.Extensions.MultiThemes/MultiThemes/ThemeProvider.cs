using System;
using System.Collections.Concurrent;
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
        private readonly IThemeConfigStore _themeConfigStore;
        private ConcurrentDictionary<string, ThemeItem> keyTheme;
        protected readonly object SyncObj = new object();
        #endregion

        #region Constructors

        public ThemeProvider(IOptionsMonitor<ThemeConfiguration> options, IThemeConfigStore themeConfigStore)
        {
            _themeConfiguration = options.CurrentValue;
            _themeConfigStore = themeConfigStore;
            keyTheme = new ConcurrentDictionary<string, ThemeItem>();
        }

        #endregion


        #region Methods

        public ThemeItem GetWorkingTheme(string domain)
        {
            string key = domain;
            if (keyTheme.ContainsKey(key))
            {
                return keyTheme[key];
            }

            lock (SyncObj)
            {
                if (keyTheme.ContainsKey(key))
                {
                    return keyTheme[key];
                }
                ThemeItem curTheme = null;

                if (GetThemes() != null && GetThemes().Count > 0)
                {
                    var query = GetThemes().OrderBy(t => t.Order).Where(t =>
                        t.SupportDomainAdapter && t.Domains.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Any(s => s == domain || t.SupportRegex && new Regex(s).IsMatch(domain)));
                    curTheme = curTheme = query.FirstOrDefault(t => t.Order == 1);
                    if (curTheme == null)
                    {
                        curTheme = query.FirstOrDefault();
                    }

                    if (curTheme == null)
                    {
                        curTheme = GetThemes().FirstOrDefault();
                    }
                    if (curTheme == null)
                    {
                        curTheme = GetThemes().FirstOrDefault(t =>
                            t.ThemeName == _themeConfiguration.DefaultTheme);
                    }
                }

                keyTheme[key] = curTheme;
                return curTheme;
            }
        }

        public void SetWorkingTheme(string domain, string themeName)
        {
            string key = domain;
            lock (SyncObj)
            {
                keyTheme[key] = GetThemes().SingleOrDefault(t => t.ThemeName == themeName);
            }
        }

        public ThemeItem GetTheme(string themeName)
        {
            return GetThemes().SingleOrDefault(x => x.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IList<ThemeItem> GetThemes()
        {
            return _themeConfiguration.Themes;
        }

        public bool ThemeExists(string themeName)
        {
            return GetThemes().Any(configuration => configuration.ThemeName.Equals(themeName, StringComparison.CurrentCultureIgnoreCase));
        }

        public string GetWorkingThemeDirPath(string domain, bool isMob)
        {
            var theme = GetWorkingTheme(domain);
            if (theme != null)
            {
                if (isMob && !string.IsNullOrWhiteSpace(theme.MobThemeDirPath))
                {
                    return theme.MobThemeDirPath;
                }

                return theme.ThemeDirPath;
            }

            return string.Empty;
        }

        #endregion
    }
}