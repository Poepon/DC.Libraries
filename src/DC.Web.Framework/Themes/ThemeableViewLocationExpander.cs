using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using CX.Web.WebExtensions;

namespace CX.Web.Themes
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string ThemeKey = "cx.themename";
        private const string DeviceKey = "cx.device";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var themeContext = (IThemeContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeContext));
            context.Values[ThemeKey] = themeContext.WorkingThemeName;
            context.Values[DeviceKey] = context.ActionContext.HttpContext.Request.IsMobileDevice() ? "Mobile" : "PC";
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme = null;
            var a = context.Values.TryGetValue(ThemeKey, out theme);
            string device = null;
            var b = context.Values.TryGetValue(DeviceKey, out device);
            if (a && b)
            {
                viewLocations = new[] {
                        $"/Themes/{theme}/{device}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/{device}/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}