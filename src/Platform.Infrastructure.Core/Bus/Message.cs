namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using System.Collections.Generic;
    using Platform.Infrastructure.Common.Security;

    public abstract class Message : IMessage
    {
        public DateTime? ScheduledEnqueueTimeUtc { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public UserContext? UserContext { get; private set; }

        public Guid CorrelationId { get; set; }

        public Guid ScopeCorrelationId { get; set; }

        public void SetUserContext(UserContext userContext) => this.UserContext = userContext;

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
