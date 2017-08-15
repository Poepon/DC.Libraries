
namespace CX.Web.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public interface IThemeContext
    {
        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        string WorkingThemeName { get; }
    }
}