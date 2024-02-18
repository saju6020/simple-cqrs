namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Cqrs.Repository.Contracts;

    internal class MongoReadRepository : MongoCqrsRepository, IReadRepository
    {
        public MongoReadRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Read, mongoDbConnectionCache)
        {
        }
    }
}
