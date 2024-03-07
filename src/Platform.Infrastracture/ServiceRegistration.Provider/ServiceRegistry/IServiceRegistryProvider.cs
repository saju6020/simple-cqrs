namespace Platform.Infrastructure.ServiceRegistry
{
    using System.Collections.Generic;

    public interface IServiceRegistryProvider
    {
        Service GetService(string serviceId);

        IEnumerable<Service> GetAllServices();
    }
}
