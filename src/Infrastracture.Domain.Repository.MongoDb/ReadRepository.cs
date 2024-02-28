namespace Platform.Infrastracture.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Core.Queries;

    using Platform.Infrastructure.Repository.MongoDb;

    internal class ReadRepository : Repository, IReadRepository
    {
        public ReadRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Read, mongoDbConnectionCache)
        {
        }
    }
}
