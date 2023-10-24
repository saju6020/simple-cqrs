namespace Platform.Infrastructure.ServiceRegistry
{
    using System;

    public class Feature
    {
        public Feature(string id, string[] roles, string[] endPoints)
        {
            this.Id = id;
            this.Roles = roles;
            this.EndPoints = endPoints;
        }

        public string Id { get; set; }

        public string[] Roles { get; set; }

        public string[] EndPoints { get; set; }

        public Guid VerticalId { get; private set; }

        public string ServiceId { get; private set; }

        public Guid TenantId { get; private set; }

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