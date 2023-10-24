namespace Platform.Infrastructure.Core.Accessors
{
    using System;

    public interface IMessageCorrelationIdProvider
    {
        Guid GetMessageCorrelationId();

        Guid GetMessageScopeCorrelationId();
    }
}
