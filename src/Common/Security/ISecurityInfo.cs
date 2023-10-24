namespace Platform.Infrastructure.Common.Security
{
    public interface ISecurityInfo
    {
        UserContext UserContext { get; }

        void SetUserContext(UserContext userContext);
    }
}
