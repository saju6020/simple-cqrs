using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core;
using System.Collections;

namespace GenericCommandWeb.Domain
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository _repository;
        private readonly Hashtable configList = new Hashtable();
        public AuthorizationService(IRepository repository) { 
            _repository = repository;
            _ = LoadCommandConfigurations();
        }
        public async Task<bool> HassAccess(string commandType, UserContext userContext)
        {
            if(configList == null || configList.Count == 0)
            {
                _ = LoadCommandConfigurations();
            }
            var commandList = (List<CommandRoles>)configList[$"{userContext.TenantId}-{userContext.VerticalId}"];

            var command = commandList.Where(c => c.CommandName == commandType).SingleOrDefault();

            foreach (string role in userContext.Roles)
            {
                if (command.Roles.Contains(role))
                {
                    return true;
                }
                
            }

            return false;

        }

       private async Task LoadCommandConfigurations()
        {
            var commandConfigurations = await this._repository.GetItemsAsync<CommandConfiguration>();

            foreach (var commandConfiguration in commandConfigurations)
            {
                configList.Add($"{commandConfiguration.TenantId}-{commandConfiguration.VerticalId}", commandConfiguration.CommandRoles);
            }
        }
    }
}
