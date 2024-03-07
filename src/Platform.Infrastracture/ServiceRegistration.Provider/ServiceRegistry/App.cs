namespace Platform.Infrastructure.ServiceRegistry
{
    using System;

    public class App
    {
        public string Id { get; }

        public Guid VerticalId { get; private set; }

        public string ServiceId { get; private set; }

        public Guid TenantId { get; private set; }

        public string Secret { get; }

        public string Name { get; }

        public void SetServiceId(string serviceId)
        {
            this.ServiceId = serviceId;
        }

        public void SetTenantId(Guid tenantId)
        {
            this.TenantId = tenantId;
        }

        public void SetVerticalId(Guid verticalId)
        {
            this.VerticalId = verticalId;
        }
    }
}