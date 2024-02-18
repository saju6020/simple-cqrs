namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Cqrs.Repository.Contracts;

    internal class MongoEventRepository : MongoCqrsRepository, IEventRepository
    {
        public MongoEventRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Event, mongoDbConnectionCache)
        {
        }
    }
}
