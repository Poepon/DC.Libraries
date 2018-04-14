using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DC.Libraries.Extensions.MultiThemes;

namespace TestApp.Views.Shared.Components.ThemeChange
{
    public class ThemeChangeViewComponent : ViewComponent
    {
        private readonly IThemeProvider _themeProvider;

        public ThemeChangeViewComponent(IThemeProvider themeProvider)
        {
            _themeProvider = themeProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var theme = _themeProvider.GetWorkingTheme(Request.Host.Host);
            var list = _themeProvider.GetThemes();
            var model = new ThemeViewModel() {CurThemeName = theme.ThemeName, Themes = list};
            return View(model);
        }
    }

    public class ThemeViewModel
    {
        public string CurThemeName { get; set; }
        public IList<ThemeItem> Themes { get; set; }
    }
}
