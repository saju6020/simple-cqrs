namespace Platform.Infrastructure.Core.RowLevelSecurity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RowLevelSecurityBuilder
    {
        private readonly RowLevelSecurity rowLevelSecurity;

        public RowLevelSecurityBuilder(Guid userId, string[] roles)
        {
            this.rowLevelSecurity = new RowLevelSecurity
            {
                RolesAllowedToRead = roles,
                IdsAllowedToRead = new[] { userId },
            };
        }

        public RowLevelSecurityBuilder WithRolesAllowedToRead(IEnumerable<string> rolesAllowedToRead)
        {
            this.rowLevelSecurity.RolesAllowedToRead = rolesAllowedToRead.ToArray();

            return this;
        }

        public RowLevelSecurityBuilder WithIdsAllowedToRead(IEnumerable<Guid> idsAllowedToRead)
        {
            this.rowLevelSecurity.IdsAllowedToRead = idsAllowedToRead.ToArray();

            return this;
        }

        public IRowLevelSecurity Build()
        {
            return this.rowLevelSecurity;
        }

        public IRowLevelSecurity BuildDefault()
        {
            return this.rowLevelSecurity;
        }
    }
}
