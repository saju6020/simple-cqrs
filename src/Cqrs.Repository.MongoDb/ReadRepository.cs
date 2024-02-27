namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Core.Queries;

    internal class ReadRepository : Repository, IReadRepository
    {
        public ReadRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Read, mongoDbConnectionCache)
        {
        }
    }
}
