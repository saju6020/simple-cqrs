namespace Platform.Infrastructure.CacheStore.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

    /// <summary>Configure Redis.</summary>
    [ExcludeFromCodeCoverage]
    public static class RedisConfiguration
    {
        /// <summary>Configure the redis.</summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureRedis(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            serviceCollection.AddTransient<IDatabase>((sp) =>
                connectionMultiplexer.GetDatabase(configuration.GetValue<int>("DbNumber")));
        }
    }
}
