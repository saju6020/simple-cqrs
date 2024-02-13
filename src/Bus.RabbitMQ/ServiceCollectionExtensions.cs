namespace Platform.Infrastructure.Bus.RabbitMQ.Extensions
{
    using System;
    using System.Linq;
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Platform.Infrastructure.Bus.Abstraction.Internals;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Bus;

    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds the service bus provider.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="rabbitMqMessageBusConfigurator">The rabbit mq message bus configurator.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="ArgumentNullException">No BusConfig section is found in your appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json file.</exception>
        public static IServiceCollection AddServiceBusProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IServiceCollection, IRabbitMqBusFactoryConfigurator, IBusRegistrationConfigurator, IRabbitMqReceiveEndpointConfigurator, IConfiguration> rabbitMqMessageBusConfigurator)
        {
            services = RegisterServices(services);

            var busOptions = configuration.GetSection("BusConfig").Get<BusOptions>();

            if (busOptions == null)
            {
                throw new ArgumentNullException(
                    $"No BusConfig section is found in your appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json file.");
            }

            services.AddMassTransit(serviceCollectionBusConfigurator =>
            {
                serviceCollectionBusConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    ConfigureConnection(configurator, busOptions);

                    ConfigureFilters(context, configurator);

                    // Configure the receive endpoint if the queue name is specified. Else it would be a send only bus
                    if (string.IsNullOrWhiteSpace(busOptions.QueueName))
                    {
                        return;
                    }

                    configurator.ReceiveEndpoint(busOptions.QueueName, endpointConfigurator =>
                    {
                        SetupPrefetchCountAndRetry(endpointConfigurator, busOptions);
                        rabbitMqMessageBusConfigurator(services, configurator, serviceCollectionBusConfigurator, endpointConfigurator, configuration);
                    });
                });
            });

            return services;
        }

        /// <summary>Adds the rabbit mq service bus.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="rabbitMqMessageBusConfigurator">The rabbit mq message bus configurator.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="ArgumentNullException">No BusConfig section is found in your appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json file.</exception>
        public static IServiceCollection AddServiceBusProvider(
           this IServiceCollection services,
           IConfiguration configuration,
           Action<IRabbitMqReceiveEndpointConfigurator> rabbitMqMessageBusConfigurator)
        {
            services = RegisterServices(services);

            var busOptions = configuration.GetSection("BusConfig").Get<BusOptions>();

            if (busOptions == null)
            {
                throw new ArgumentNullException(
                    $"No BusConfig section is found in your appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json file.");
            }

            services.AddMassTransit(serviceCollectionBusConfigurator =>
            {
                serviceCollectionBusConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    ConfigureConnection(configurator, busOptions);

                    ConfigureFilters(context, configurator);

                    // Configure the receive endpoint if the queue name is specified. Else it would be a send only bus
                    if (string.IsNullOrWhiteSpace(busOptions.QueueName))
                    {
                        return;
                    }

                    configurator.ReceiveEndpoint(busOptions.QueueName, endpointConfigurator =>
                    {
                        SetupPrefetchCountAndRetry(endpointConfigurator, busOptions);

                        rabbitMqMessageBusConfigurator(endpointConfigurator);
                    });
                });
            });

            return services;
        }

        private static void SetupPrefetchCountAndRetry(IRabbitMqReceiveEndpointConfigurator endpointConfigurator, BusOptions busOptions)
        {
            if (busOptions.PrefetchCount > 0)
            {
                endpointConfigurator.PrefetchCount = Convert.ToUInt16(busOptions.PrefetchCount);
            }

            if (busOptions.RetryCount > 0 && busOptions.RetryInterval > 0)
            {
                endpointConfigurator.UseRetry(retryConfig =>
                {
                    retryConfig.Interval(busOptions.RetryCount, TimeSpan.FromSeconds(busOptions.RetryInterval));
                });
            }
        }

        /// <summary>Adds the rabbit mq service bus.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="ArgumentNullException">No BusConfig section is found in your appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json file.</exception>
        public static IServiceCollection AddServiceBusProvider(
             this IServiceCollection services,
             IConfiguration configuration)
        {
            services = RegisterServices(services);

            var busOptions = configuration.GetSection("BusConfig").Get<BusOptions>();

            if (busOptions == null)
            {
                throw new ArgumentNullException(
                    $"No BusConfig section found in configuration file.");
            }

            services.AddMassTransit(serviceCollectionBusConfigurator =>
            {
                serviceCollectionBusConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    ConfigureConnection(configurator, busOptions);

                    ConfigureFilters(context, configurator);
                });
            });

            return services;
        }

        /// <summary>Configures the filters.</summary>
        /// <param name="context">The context.</param>
        /// <param name="configurator">The configurator.</param>
        private static void ConfigureFilters(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.UseSendFilter(typeof(MassTransitGenericSendFilter<>), context);
            configurator.UsePublishFilter(typeof(MassTransitGenericPublishFilter<>), context);
            configurator.UseConsumeFilter(typeof(MassTransitGenericMessageFilter<>), context);
        }

        /// <summary>Configures the connection.</summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="busOptions">The bus options.</param>
        private static void ConfigureConnection(IRabbitMqBusFactoryConfigurator configurator, BusOptions busOptions)
        {
            configurator.Host(busOptions.ConnectionString, h =>
            {
                h.Username(busOptions.UserName);
                h.Password(busOptions.Password);
            });
        }

        /// <summary>Registers the services.</summary>
        /// <param name="services">The services.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(MassTransitGenericMessageFilter<>));
            services.AddScoped(typeof(MassTransitGenericSendFilter<>));
            services.AddScoped(typeof(MassTransitGenericPublishFilter<>));

            services.AddSingleton<IContextAccessor, ContextAccessor>();

            if (services.All(x => x.ServiceType != typeof(IUserContextProvider)))
            {
                services.AddSingleton<IUserContextProvider, UserContextProvider>();
            }

            services.AddSingleton<IBusProvider, BusProvider>();
            services.AddSingleton<IBusMessageDispatcher, BusMessageDispatcher>();

            services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();
            services.AddSingleton<IMessageCorrelationIdProvider, MessageCorrelationIdProvider>();

            services.AddSingleton<IHostedService, BusHostedService>();

            return services;
        }
    }
}
