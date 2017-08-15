namespace CX.Web.Tenants
{
    public class AppTenant
    {
        public string Name { get; set; }

        public string Theme { get; set; }

    }

    public class Multitenancy
    {
        public AppTenant[] Tenants { get; set; }

        public string CurrentTenant { get; set; }
    }
}
