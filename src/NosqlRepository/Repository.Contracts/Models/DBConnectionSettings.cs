namespace Platform.Infrastructure.NoSql.Repository.Contracts.Models
{
    using System;

    /// <summary>Class to contain db connection settings.</summary>
    public class DbConnectionSettings
    {
        /// <summary>Gets or sets the connection string.</summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }

        /// <summary>Gets or sets the name of the database.</summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; set; }

        /// <summary>Gets or sets the tenant identifier.</summary>
        /// <value>The tenant identifier.</value>
        public Guid TenantId { get; set; }

        /// <summary>Gets or sets the vertical identifier.</summary>
        /// <value>The vertical identifier.</value>
        public string VerticalId { get; set; }
    }
}
