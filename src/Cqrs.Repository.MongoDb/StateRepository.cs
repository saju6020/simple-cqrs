using Platform.Infrastructure.Core.Domain;

namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    internal class StateRepository : Repository, IStateRepository
    {
        public StateRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.State, mongoDbConnectionCache)
        {
        }
    }
}
