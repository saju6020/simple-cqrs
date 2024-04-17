namespace Platform.Infrastructure.FaultTolerance.Http
{
    using System;
    using Platform.Infrastructure.FaultTolerance.Http.Contracts;
    using Platform.Infrastructure.FaultTolerance.Http.Models;
    using Platform.Infrastructure.FaultTolerance.Http.Service;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class provides two extension on IServiceCollection for configuring circuit breaker and retry.
    /// </summary>
    public static class HttpFaultTolerance
    {
        /// <summary>
        /// Extension for using http fault tolerance with  retry policy added.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns> IServiceCollection. </returns>
        public static IServiceCollection UseHttpFaultTolerance(
            this IServiceCollection services,
            IConfiguration configuration, ILogger logger)
        {
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();

            PolicyHandler policyHandler = new PolicyHandler();
            CircuitBreakerConfigurationModel circuitBreakerConfig = new CircuitBreakerConfigurationModel();
            RetryConfigurationModel retryConfig = new RetryConfigurationModel();

            configuration.GetSection(Constants.CircuitBreakerConfigKey).Bind(circuitBreakerConfig);
            configuration.GetSection(Constants.RetryConfigKey).Bind(retryConfig);

            services.AddHttpClient(Constants.ClientName)
                .AddPolicyHandler(policyHandler.GetRetryPolicy(
                    retryConfig.RetryCount,
                    TimeSpan.FromSeconds(retryConfig.IntervalBetweenRetry),
                    DefaultActionProvider.GetOnTryAction(logger)))
                .AddPolicyHandler(policyHandler.GetCircuitBreakerPolicy(
                    circuitBreakerConfig.CircuitBreakerThreshold,
                    TimeSpan.FromSeconds(circuitBreakerConfig.DurationOfBreak),
                    DefaultActionProvider.GetOnBreakAction(),
                    DefaultActionProvider.GetOnResetAction()));
            return services;
        }

        /// <summary>
        /// Uses the HTTP fault tolerance with no retry.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns> returns IServiceCollection.</returns>
        public static IServiceCollection UseHttpFaultToleranceWithNoRetry(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();

            PolicyHandler policyHandler = new PolicyHandler();
            CircuitBreakerConfigurationModel circuitBreakerConfig = new CircuitBreakerConfigurationModel();

            configuration.GetSection(Constants.CircuitBreakerConfigKey).Bind(circuitBreakerConfig);

            services.AddHttpClient(Constants.ClientName)
                .AddPolicyHandler(policyHandler.GetCircuitBreakerPolicy(
                    circuitBreakerConfig.CircuitBreakerThreshold,
                    TimeSpan.FromSeconds(circuitBreakerConfig.DurationOfBreak),
                    DefaultActionProvider.GetOnBreakAction(),
                    DefaultActionProvider.GetOnResetAction()));
            return services;
        }
    }
}
