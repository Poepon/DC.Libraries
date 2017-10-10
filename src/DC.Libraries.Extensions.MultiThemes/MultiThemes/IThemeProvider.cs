﻿using System.Collections.Generic;

namespace DC.Libraries.Extensions.MultiThemes
{
    public partial interface IThemeProvider
    {
        ThemeItem GetTheme(string themeName);

        IList<ThemeItem> GetThemes();

        bool ThemeExists(string themeName);

        ThemeItem GetWorkingTheme(bool isMobile, string domain);
    }
}