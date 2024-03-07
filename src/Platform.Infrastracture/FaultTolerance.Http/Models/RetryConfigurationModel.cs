namespace Platform.Infrastructure.FaultTolerance.Http.Models
{
    /// <summary>
    /// Model for RetryConfiguration.
    /// </summary>
    public class RetryConfigurationModel
    {
        /// <summary>
        /// Gets or sets number of time retry should occur default is 2.
        /// </summary>
        public int RetryCount { get; set; } = Constants.RetryCount;

        /// <summary>
        /// Gets or sets interval time between each retry default is 1.
        /// </summary>
        public int IntervalBetweenRetry { get; set; } = Constants.IntervalBetweenRetry;
    }
}
