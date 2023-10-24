namespace Platform.Infrastructure.Core.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Extensions;
    using Platform.Infrastructure.Core.Queries;

    /// <summary>Service collection extensions.</summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, params Type[] types)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IResolver, Resolver>();
            services.AddScoped<IHandlerResolver, HandlerResolver>();
            services.AddScoped<IDispatcher, Dispatcher>();

            services.AddScoped<IDomainEventProcessor, DomainEventProcessor>();
            services.AddScoped(typeof(IDomainRepository<>), typeof(DomainRepository<>));
            services.AddSingleton<IBusMessageDispatcher, BusMessageDispatcher>();
            services.AddScoped<ICommandSender, CommandSender>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IQueryProcessor, QueryProcessor>();

            return services;
        }

        public static IServiceCollection AddDefaultBusProvider(this IServiceCollection services)
        {
            services.AddSingleton<IBusProvider, DefaultBusProvider>();
            return services;
        }

        public static IServiceCollection AddDefaultDomainStore(this IServiceCollection services)
        {
            services.AddScoped<IDomainStore, DefaultDomainStore>();
            return services;
        }
    }
}
