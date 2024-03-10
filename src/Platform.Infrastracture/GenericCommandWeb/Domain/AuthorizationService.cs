using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core;

namespace GenericCommandWeb.Domain
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository _repository;
        public AuthorizationService(IRepository repository) { 
            _repository = repository;
        }
        public Task<bool> HassAccess(string commandType, UserContext userContext)
        {
            throw new NotImplementedException();
        }

       // private 
    }
}
