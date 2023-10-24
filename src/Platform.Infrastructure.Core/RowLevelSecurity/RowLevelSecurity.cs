namespace Platform.Infrastructure.Core.RowLevelSecurity
{
    using System;

    internal class RowLevelSecurity : IRowLevelSecurity
    {
        public string[] RolesAllowedToRead { get; set; }

        public Guid[] IdsAllowedToRead { get; set; }
    }
}
