namespace Platform.Infrastructure.FaultTolerance.Http.Models
{
    /// <summary>
    /// Model for CircuitBreakerConfiguration.
    /// </summary>
    public class CircuitBreakerConfigurationModel
    {
        /// <summary>
        /// Gets or sets number of time after which circuit should be open default is 5.
        /// </summary>
        public int CircuitBreakerThreshold { get; set; } = Constants.CircuitBreakerThreshold;

        /// <summary>
        /// Gets or sets duration till which circuit should be open default is 10.
        /// </summary>
        public int DurationOfBreak { get; set; } = Constants.DurationOfBreak;
    }
}
