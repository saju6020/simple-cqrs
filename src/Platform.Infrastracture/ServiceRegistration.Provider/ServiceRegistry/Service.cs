namespace Platform.Infrastructure.ServiceRegistry
{
    using System.Linq;
    using Newtonsoft.Json;

    public class Service
    {
        [JsonConstructor]
        public Service(string id, string name, Tenant[] tenants)
        {
            this.Id = id;
            this.Name = name;
            this.Tenants = tenants;

            foreach (var tenant in this.Tenants)
            {
                tenant.SetServiceId(this.Id);

                foreach (var vertical in tenant.Verticals)
                {
                    vertical.SetServiceId(this.Id);
                    vertical.SetTenantId(tenant.Id);

                    foreach (var app in vertical.Apps)
                    {
                        app.SetServiceId(this.Id);
                        app.SetTenantId(tenant.Id);
                        app.SetVerticalId(vertical.Id);
                    }
                }
            }

            this.Verticals = this.Tenants.SelectMany(tenant => tenant.Verticals).ToArray();

            this.Apps = this.Verticals.SelectMany(vertical => vertical.Apps).ToArray();
        }

        public string Id { get; }

        public string Name { get; }

        public Tenant[] Tenants { get; }

        public Vertical[] Verticals { get; }

        public App[] Apps { get; }
    }
}
