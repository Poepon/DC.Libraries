
namespace CX.Web.Tenants
{
    public interface ITenantProvider 
    {
        Multitenancy TenantsConfig { get; set; }

        AppTenant CurrenTenant { get; }

    }
}