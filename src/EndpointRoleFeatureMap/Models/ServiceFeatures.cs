namespace Platform.Infrastructure.EndpointRoleFeatureMap.Models
{
    using System.Linq;
    using Newtonsoft.Json;
    using Platform.Infrastructure.ServiceRegistry;

    internal class ServiceFeatures
    {
        public string Id { get; }

        public Tenant[] Tenants { get; }

        public Vertical[] Verticals { get; }

        public Feature[] Features { get; }

        [JsonConstructor]
        public ServiceFeatures(string id, Tenant[] tenants)
        {
            this.Id = id;

            this.Tenants = tenants;

            foreach (var tenant in this.Tenants)
            {
                tenant.SetServiceId(this.Id);

                foreach (var vertical in tenant.Verticals)
                {
                    vertical.SetServiceId(this.Id);
                    vertical.SetTenantId(tenant.Id);

                    foreach (var feature in vertical.Features)
                    {
                        feature.SetServiceId(this.Id);
                        feature.SetTenantId(tenant.Id);
                        feature.SetVerticalId(vertical.Id);
                    }
                }
            }

            this.Verticals = this.Tenants.SelectMany(tenant => tenant.Verticals).ToArray();

            this.Features = this.Verticals.SelectMany(vertical => vertical.Features).ToArray();
        }
    }
}
