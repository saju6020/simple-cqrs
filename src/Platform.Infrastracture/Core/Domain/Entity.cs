namespace Platform.Infrastructure.Domain
{
    using System;
    using Platform.Infrastructure.Common.Security;

    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; }

        public Guid CreatedBy { get; protected set; }

        public DateTime CreatedDate { get; protected set; }

        public string Language { get; protected set; }

        public DateTime LastUpdatedDate { get; protected set; }

        public Guid LastUpdatedBy { get; protected set; }

        public Guid TenantId { get; protected set; }

        public string[] Tags { get; protected set; }

        public Guid VerticalId { get; protected set; }

        public string ServiceId { get; protected set; }

        public bool IsMarkedToDelete { get; protected set; }

        protected Entity()
        {
            this.Id = Guid.NewGuid();
        }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            this.Id = id;
        }

        public virtual void SetDefaultValue(UserContext userContext)
        {
            DateTime currentTime = DateTime.UtcNow;
            if (this.CreatedBy == Guid.Empty)
            {
                this.CreatedBy = userContext.UserId;
                this.CreatedDate = currentTime;
            }

            this.TenantId = userContext.TenantId;
            this.VerticalId = userContext.VerticalId;
            this.ServiceId = userContext.ServiceId;

            this.LastUpdatedDate = currentTime;
            this.LastUpdatedBy = userContext.UserId;
        }
    }
}
