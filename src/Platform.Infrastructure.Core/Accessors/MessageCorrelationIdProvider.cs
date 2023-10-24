namespace Platform.Infrastructure.Core.Accessors
{
    using System;
    using Platform.Infrastructure.Core;

    public class MessageCorrelationIdProvider : IMessageCorrelationIdProvider
    {
        private readonly ICorrelationIdAccessor correlationIdAccessor;

        public MessageCorrelationIdProvider(ICorrelationIdAccessor correlationIdAccessor)
        {
            this.correlationIdAccessor = correlationIdAccessor;
        }

        public Guid GetMessageCorrelationId()
        {
            return this.correlationIdAccessor.Id;
        }

        public Guid GetMessageScopeCorrelationId()
        {
            return this.correlationIdAccessor.ScopeId;
        }
    }
}
