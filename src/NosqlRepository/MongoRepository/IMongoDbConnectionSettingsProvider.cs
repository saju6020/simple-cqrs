namespace Platform.Infrastructure.NoSql.Repository
{
    using Platform.Infrastructure.NoSql.Repository.Contracts;

    /// <summary>Mongo connection settings provider.</summary>
    /// <seealso cref="IDbConnectionSettingsProvider" />
    public interface IMongoDbConnectionSettingsProvider : IDbConnectionSettingsProvider
    {
    }
}
