namespace Platform.Infrastracture.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Repository.MongoDb;

    internal class EventRepository : Repository, IEventRepository
    {
        public EventRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Event, mongoDbConnectionCache)
        {
        }
    }
}
