namespace Platform.Infrastructure.ServiceRegistry
{
    using System;

    public class Tenant
    {
        public Tenant(Guid id, string name, Vertical[] verticals)
        {
            this.Id = id;
            this.Name = name;
            this.Verticals = verticals;
        }

        public Guid Id { get; }

        public string ServiceId { get; private set; }

        public string Name { get; }

        public Vertical[] Verticals { get; }

        public void SetServiceId(string serviceId)
        {
            this.ServiceId = serviceId;
        }
    }
}