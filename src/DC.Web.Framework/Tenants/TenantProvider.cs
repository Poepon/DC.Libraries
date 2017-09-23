using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CX.Web.Tenants
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHostingEnvironment _environment;
        private Multitenancy _multitenancy;

        public TenantProvider(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public Multitenancy TenantsConfig
        {
            get
            {
                if (_multitenancy == null)
                {
                    string filePath = Path.Combine(_environment.ContentRootPath, "apptenant.json");
                    if (File.Exists(filePath))
                    {
                        var config = AppConfigurations.GetFullPath(filePath);
                        _multitenancy = config.GetSection("Multitenancy").Get<Multitenancy>();
                    }
                }
                return _multitenancy;
            }
            set
            {

            }
        }

        public AppTenant CurrenTenant
        {
            get
            {
                var item = TenantsConfig.Tenants.SingleOrDefault(t => t.Name == TenantsConfig.CurrentTenant);
                if (item == null)
                {
                    item = TenantsConfig.Tenants.First();
                }
                return item;
            }
        }
    }
}