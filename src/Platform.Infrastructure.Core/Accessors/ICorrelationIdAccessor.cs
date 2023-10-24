namespace Platform.Infrastructure.Core.Accessors
{
    using System;

    public interface ICorrelationIdAccessor
    {
        Guid Id { get; set; }

        Guid ScopeId { get; set; }
    }
}
