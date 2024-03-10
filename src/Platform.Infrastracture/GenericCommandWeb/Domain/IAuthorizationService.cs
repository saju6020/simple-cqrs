using Platform.Infrastructure.Common.Security;

namespace GenericCommandWeb.Domain
{
    public interface IAuthorizationService
    {
        public Task<bool> HassAccess(string commandType, UserContext userContext);
    }
}
