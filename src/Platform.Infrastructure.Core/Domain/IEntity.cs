namespace Platform.Infrastructure.Domain
{
    using System;
    using Platform.Infrastructure.Common.Security;

    public interface IEntity
    {
        Guid Id { get; }

        Guid CreatedBy { get; }

        DateTime CreatedDate { get; }

        string Language { get; }

        DateTime LastUpdatedDate { get; }

        Guid LastUpdatedBy { get; }

        Guid TenantId { get; }

        string[] Tags { get; }

        Guid VerticalId { get; }

        bool IsMarkedToDelete { get; }

        void SetDefaultValue(UserContext userContext);
    }
}
