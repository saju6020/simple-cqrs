namespace Platform.Infrastructure.NoSql.Repository.Contracts
{
    using System;
    using Platform.Infrastructure.NoSql.Repository.Contracts.Models;

    /// <summary>Database connection setting provider abstraction.</summary>
    public interface IDbConnectionSettingsProvider
    {
        /// <summary>Gets the connection settings.</summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>Connection settings.</returns>
        DbConnectionSettings GetConnectionSettings(Guid tenantId);

        /// <summary>Gets the connection settings.</summary>
        /// <returns>Return connection settings.</returns>
        DbConnectionSettings GetConnectionSettings();
    }
}
