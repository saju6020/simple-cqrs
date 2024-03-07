namespace Platform.Infrastructure.ServiceRegistry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class ServiceRegistryProvider : IServiceRegistryProvider
    {
        private readonly Service[] services;

        public ServiceRegistryProvider(IConfiguration configuration)
        {
            var serviceRegistrationsFilePath = configuration["ServiceRegistrationsFilePath"];

            if (string.IsNullOrWhiteSpace(serviceRegistrationsFilePath))
            {
                throw new ArgumentException("Please provide ServiceRegistrationsFilePath through appsettings.{xxx}.json file");
            }

            if (!File.Exists(serviceRegistrationsFilePath))
            {
                throw new FileNotFoundException("Service registrations file not found.", serviceRegistrationsFilePath);
            }

            var serviceRegistrationsJson = File.ReadAllText(serviceRegistrationsFilePath);

            this.services = JsonConvert.DeserializeObject<Service[]>(serviceRegistrationsJson);
        }

        public IEnumerable<Service> GetAllServices()
        {
            return this.services;
        }

        public Service GetService(string serviceId)
        {
            return this.services.First(service => service.Id.Equals(serviceId));
        }
    }
}
