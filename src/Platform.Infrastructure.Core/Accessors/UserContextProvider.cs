namespace Platform.Infrastructure.Core.Accessors
{
    using System;
    using Platform.Infrastructure.Common.Security;

    public class UserContextProvider : IUserContextProvider
    {
        private readonly IContextAccessor contextAccessor;

        public UserContextProvider(IContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public UserContext GetUserContext()
        {
            return this.contextAccessor.Context.UserContext;
        }
    }
}
