
namespace Platform.Infrastracture.Domain.Repository.MongoDb
{
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Repository.MongoDb;
    public class StateRepository : Repository, IStateRepository
    {
        public StateRepository(MongoDbConnectionCache mongoDbConnectionCache)
            : base(DatabaseType.State, mongoDbConnectionCache)
        {
        }
    }
}
