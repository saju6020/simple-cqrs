namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using Platform.Infrastructure.Common.Security;

    public interface IMessage : ISecurityInfo
    {
        Guid CorrelationId { get; set; }

        Guid ScopeCorrelationId { get; set; }

        DateTime TimeStamp { get; set; }
    }
}
