namespace Platform.Infrastructure.Common.Security
{
    public interface IUserContextProvider
    {
        UserContext GetUserContext();
    }
}
