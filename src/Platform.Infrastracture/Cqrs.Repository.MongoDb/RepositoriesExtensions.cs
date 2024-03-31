namespace Platform.Infrastructure.Cqrs.Repository.MongoDb
{
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastracture.Cqrs.Repository.MongoDb;
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Core.Queries;
    using Platform.Infrastructure.Repository.MongoDb;
    using Platform.Infrastructure.ServiceRegistry;

    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddReadRepository(this IServiceCollection serviceCollection)
        {
            AddRepositoryDefaults(serviceCollection);
            serviceCollection.AddSingleton<IReadRepository, ReadRepository>();

            return serviceCollection;
        }

        public static IServiceCollection AddStateRepository(this IServiceCollection serviceCollection)
        {
            AddRepositoryDefaults(serviceCollection);
            serviceCollection.AddSingleton<IStateRepository, StateRepository>();

            return serviceCollection;
        }

        public static IServiceCollection AddEventRepository(this IServiceCollection serviceCollection)
        {
            AddRepositoryDefaults(serviceCollection);
            serviceCollection.AddSingleton<IEventRepository, EventRepository>();

            return serviceCollection;
        }

        public static IServiceCollection AddAggregateRootRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton(typeof(IAggregateRootRepository<>), typeof(AggregateRootRepository<>))
                .AddReadRepository()
                .AddEventRepository()
                .AddStateRepository();

            return serviceCollection;
        }

        private static void AddRepositoryDefaults(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MongoDbConnectionCache>();
            serviceCollection.AddSingleton<IServiceRegistryProvider, ServiceRegistryProvider>();
        }
    }
}
