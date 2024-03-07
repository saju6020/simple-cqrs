namespace Platform.Infrastructure.Core.Models
{
    using Platform.Infrastructure.Common.Security;

    public class SecurityContext
    {
        public SecurityContext(string messageId, UserContext userContext)
        {
            this.MessageId = messageId;
            this.UserContext = userContext;
        }

        public string MessageId { get; }

        public UserContext UserContext { get; }
    }
}
