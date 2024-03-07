namespace Platform.Infrastructure.Throttling
{
    using AspNetCoreRateLimit;
    using AspNetCoreRateLimit.Redis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

    public static class RateLimitExtensions
    {
        public static IServiceCollection AddRateLimitServices(this IServiceCollection services, IConfiguration configuration, string redisConnectionStringName)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            IConfigurationSection configurationSection = configuration.GetSection("ClientRateLimiting");
            ClientRateLimitOptions clientRateLimitOptions = new ClientRateLimitOptions();
            configurationSection.Bind(clientRateLimitOptions);
            services.AddSingleton(clientRateLimitOptions);

            IConfigurationSection rateLimitPoliciesSection = configuration.GetSection("ClientRateLimitPolicies");
            ClientRateLimitPolicies clientRateLimitPolicies = new ClientRateLimitPolicies();
            configurationSection.Bind(clientRateLimitPolicies);
            services.AddSingleton(clientRateLimitPolicies);

            // register stores
            // services.AddDistributedRateLimiting<StackExchangeRedisProcessingStrategy>;
            services.AddRedisRateLimiting();
            services.AddDistributedRateLimiting<RedisProcessingStrategy>();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSingleton<IRateLimitProcessor, ClientRateLimitProcessor>();
            services.AddSingleton<IRateLimitService, RateLimitService>();

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configuration.GetConnectionString(redisConnectionStringName)));
            services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString(redisConnectionStringName));

            return services;
        }
    }
}
