namespace Platform.Infrastructure.Core.Models
{
    using System;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.RowLevelSecurity;
    using Platform.Infrastructure.Domain;

    public abstract class ViewModelBase : Entity, IRowLevelSecurity
    {
        public Guid[] IdsAllowedToRead { get; set; }

        public string[] RolesAllowedToRead { get; set; }

        public int Version { get; protected set; }

        public void SetDefaultRowLevelSecurity(UserContext context)
        {
            Guid[] currentUser = new Guid[] { context.UserId };

            this.IdsAllowedToRead = (this.IdsAllowedToRead == null || this.IdsAllowedToRead.Length == 0) ? currentUser : this.IdsAllowedToRead;
        }

        public override void SetDefaultValue(UserContext userContext)
        {
            base.SetDefaultValue(userContext);

            this.Version++;
        }
    }
}
