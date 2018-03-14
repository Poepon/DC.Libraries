using System;
using System.Collections.Generic;

namespace DC.Libraries.Extensions.MultiThemes
{
    public interface IThemeConfigStore
    {
        List<ThemeItem> GetThemes();
    }
    public class NullThemeConfigStore : IThemeConfigStore
    {
        public List<ThemeItem> GetThemes()
        {
            return new List<ThemeItem>();
        }
    }
}