namespace Platform.Infrastructure.EndpointRoleFeatureMap.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Models;

    internal class EndpointRoleFeatureMapProvider
    {
        private const string AccessRoleIdFormat = "{0}-{1}-{2}-{3}";
        private const string EndpointRoleFeatureMapFilePath = "EndpointRoleFeatureMapFilePath";

        private readonly string serviceId;
        private readonly IDictionary<string, string[]> serviceEndpointAccessRoles;

        public EndpointRoleFeatureMapProvider(IConfiguration configuration, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            this.serviceId = configuration["ServiceId"];

            var featuresFilePath = configuration[EndpointRoleFeatureMapFilePath];

            if (string.IsNullOrWhiteSpace(featuresFilePath))
            {
                throw new ArgumentException("Please provide EndpointRoleFeatureMapFilePath through appsettings.{xxx}.json file");
            }

            if (!File.Exists(featuresFilePath))
            {
                throw new FileNotFoundException($"{featuresFilePath} not found.", featuresFilePath);
            }

            var serviceEndPoints =
                actionDescriptorCollectionProvider
               .ActionDescriptors.Items.OfType<ControllerActionDescriptor>()
               .Select(action => $"{action.ControllerName}/{action.ActionName}").ToArray();

            this.serviceEndpointAccessRoles = this.GetEndPointAccessRoles(serviceEndPoints, featuresFilePath);
        }

        public bool CanAccess(string endPoint, ClaimsIdentity claimsIdentity)
        {
            var serviceId = this.serviceId ?? claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.ServiceId)).Value;

            var tenantId = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.TenantId)).Value;

            var verticalId = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.VerticalId)).Value;

            var roles = claimsIdentity.Claims.Where(c => c.Type == Common.Security.ClaimTypes.Role).Select(c => c.Value).ToArray();

            var accessRoleId = string.Format(AccessRoleIdFormat, serviceId, tenantId, verticalId, endPoint);

            var accessRoleDefined = this.serviceEndpointAccessRoles.TryGetValue(accessRoleId, out string[] accessRole);

            return accessRoleDefined && accessRole.Any(role => roles.Contains(role));
        }

        private IDictionary<string, string[]> GetEndPointAccessRoles(string[] serviceEndPoints, string featuresFilePath)
        {
            var roleFeatureMapsJson = File.ReadAllText(featuresFilePath);

            var serviceFeatures = JsonConvert.DeserializeObject<ServiceFeatures[]>(roleFeatureMapsJson);

            var dictionary = new SortedDictionary<string, string[]>();

            foreach (var serviceEndPoint in serviceEndPoints)
            {
                foreach (var serviceFeature in serviceFeatures)
                {
                    foreach (var feature in serviceFeature.Features)
                    {
                        if (feature.EndPoints.Any(ep => ep.Equals(serviceEndPoint, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var accessRoleId = string.Format(AccessRoleIdFormat, serviceFeature.Id, feature.TenantId, feature.VerticalId, serviceEndPoint);

                            dictionary.Add(accessRoleId, feature.Roles);
                        }
                    }
                }
            }

            return dictionary;
        }
    }
}
