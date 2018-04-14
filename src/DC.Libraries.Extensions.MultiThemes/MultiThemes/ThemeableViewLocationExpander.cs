using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DC.Libraries.Extensions.MultiThemes
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string ThemeKey = "cx.themename";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var themeContext = (IThemeProvider)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeProvider));
            context.Values[ThemeKey] = themeContext.GetWorkingThemeDirPath(context.ActionContext.HttpContext.Request.Host.Host, context.ActionContext.HttpContext.Request.IsMobileDevice());
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string themeDirPath = null;
            context.Values.TryGetValue(ThemeKey, out themeDirPath);

            if (!string.IsNullOrWhiteSpace(themeDirPath))
            {
                viewLocations = new[]
                    {
                        $"{themeDirPath}/Views/{{1}}/{{0}}.cshtml",
                        $"{themeDirPath}/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}