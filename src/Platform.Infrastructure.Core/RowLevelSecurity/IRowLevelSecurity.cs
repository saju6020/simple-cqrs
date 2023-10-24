namespace Platform.Infrastructure.Core.RowLevelSecurity
{
    using System;

    public interface IRowLevelSecurity
    {
        Guid[] IdsAllowedToRead { get; set; }

        string[] RolesAllowedToRead { get; set; }
    }
}