namespace Platform.Infrastructure.FaultTolerance.Http
{
    /// <summary>
    /// This is a static class for providing constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// An constant string used as name in HttpClientFactory.
        /// </summary>
        public const string ClientName = "Shohoz-Http";

        /// <summary>
        /// Circuit Breaker configuration key name.
        /// </summary>
        public const string CircuitBreakerConfigKey = "CircuitBreakerConfig";

        /// <summary>
        /// Retry configuration key name.
        /// </summary>
        public const string RetryConfigKey = "RetryConfig";

        /// <summary>
        /// The circuit breaker threshold.
        /// </summary>
        public const int CircuitBreakerThreshold = 5;

        /// <summary>
        /// The duration of break.
        /// </summary>
        public const int DurationOfBreak = 10;

        /// <summary>
        /// The retry count.
        /// </summary>
        public const int RetryCount = 2;

        /// <summary>
        /// The interval between retry.
        /// </summary>
        public const int IntervalBetweenRetry = 1;

        public const string Authorization = "Authorization";

        public const string Bearer = "Bearer";

        public const string Referer = "Referer";
    }
}
