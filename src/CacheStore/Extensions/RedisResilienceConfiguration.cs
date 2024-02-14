namespace Platform.Infrastructure.CacheStore.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;
    using StackExchange.Redis.Resilience;

    /// <summary>Configure Redis.</summary>
    [ExcludeFromCodeCoverage]
    public static class RedisResilienceConfiguration
    {
        /// <summary>Configure the redis.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureResilientRedis(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var multiplexer = new ResilientConnectionMultiplexer(
                () =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")),
                new ResilientConnectionConfiguration()
                {
                    ReconnectMinFrequency = TimeSpan.FromSeconds(10),
                    ReconnectErrorThreshold = TimeSpan.FromSeconds(5),
                });
            serviceCollection.AddTransient<IDatabase>((sp) =>
                multiplexer.GetDatabase(configuration.GetValue<int>("DbNumber")));
        }
    }
}