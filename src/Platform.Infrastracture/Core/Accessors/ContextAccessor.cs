namespace Platform.Infrastructure.Core.Accessors
{
    using System;
    using System.Threading;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Models;

    public class ContextAccessor : IContextAccessor
    {
        private static readonly UserContext EmptyUserContext = new UserContext();

        private static readonly SecurityContext NullSecurityContext = new SecurityContext(Guid.Empty.ToString(), EmptyUserContext);

        private static readonly AsyncLocal<(string messageId, SecurityContext context)> CurrentContext = new AsyncLocal<(string messageId, SecurityContext context)>();

        public SecurityContext Context
        {
            get
            {
                var value = CurrentContext.Value;

                if (value == (null, null))
                {
                    return NullSecurityContext;
                }

                return value.messageId == value.context?.MessageId ? value.context : null;
            }

            set
            {
                CurrentContext.Value = (value?.MessageId, value);
            }
        }
    }
}
