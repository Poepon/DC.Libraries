using System.Collections.Generic;

namespace DC.Libraries.Extensions.MultiThemes
{
    public partial interface IThemeProvider
    {
        ThemeItem GetTheme(string themeName);

        IList<ThemeItem> GetThemes();

        bool ThemeExists(string themeName);

        ThemeItem GetWorkingTheme(string domain);

        string GetWorkingThemeDirPath(string domain,bool isMob);

        void SetWorkingTheme(string domain, string themeName);
    }
}