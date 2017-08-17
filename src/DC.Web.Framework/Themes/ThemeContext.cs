using CX.Web.Tenants;

namespace CX.Web.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly ITenantProvider _tenantProvider;


        public ThemeContext(
            ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        public string WorkingThemeName => _tenantProvider.CurrenTenant.Theme;

    }
}