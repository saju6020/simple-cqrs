namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Cqrs.Repository.Contracts;

    internal class MongoStateRepository : MongoCqrsRepository, IStateRepository
    {
        public MongoStateRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.State, mongoDbConnectionCache)
        {
        }
    }
}
