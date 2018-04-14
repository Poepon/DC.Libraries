using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using DC.Libraries.Extensions.MultiThemes;

namespace DC.Libraries.Extensions.Controllers
{
    public class ThemeController : Controller
    {
        private readonly IThemeProvider _themeProvider;

        public ThemeController(IThemeProvider themeProvider)
        {
            _themeProvider = themeProvider;
        }

        public IActionResult ChangeTheme(string themeName, string returnUrl = "")
        {
            _themeProvider.SetWorkingTheme(Request.Host.Host, themeName);
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/"); //TODO: Go to app root
        }
    }
}
