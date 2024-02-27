namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Platform.Infrastructure.Core.Domain;

    internal class EventRepository : Repository, IEventRepository
    {
        public EventRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.Event, mongoDbConnectionCache)
        {
        }
    }
}
