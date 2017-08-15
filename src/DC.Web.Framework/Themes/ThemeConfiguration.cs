using Microsoft.Extensions.Configuration;

namespace CX.Web.Themes
{
    public class ThemeConfiguration
    {
        public ThemeConfiguration(string themeName, string path)
        {
            ThemeName = themeName;
            Path = path;
            var config = AppConfigurations.GetTheme(path);
            var section = config.GetSection("Theme");
            if (section != null)
            {
                ConfigurationNode = section;
                var attribute = section["title"];
                ThemeTitle = attribute ?? string.Empty;
                attribute = section["previewText"];
                PreviewText = attribute ?? string.Empty;
            }
        }

        public IConfigurationSection ConfigurationNode { get; protected set; }

        public string Path { get; protected set; }

        public string PreviewText { get; protected set; }

        public string ThemeName { get; protected set; }

        public string ThemeTitle { get; protected set; }

    }
}