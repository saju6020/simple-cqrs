namespace Platform.Infrastructure.FaultTolerance.Http
{
    using System;
    using System.Net.Http;
    using Polly;
    using Polly.Extensions.Http;

    /// <summary>
    /// This class Provides policy for CircuitBreaker and Retry .
    /// </summary>
    public class PolicyHandler
    {
        /// <summary>
        /// Configuration for Circuit Breaker policy.
        /// </summary>
        /// <param name="circuitBreakerThreshold"> Number of time after which circuit breaker opens the circuit. </param>
        /// <param name="durationOfBreak"> Duration of time circuit breaker will remain open.</param>
        /// <param name="onBreak"> Action on circuit break.</param>
        /// <param name="onReset">Action on circuit Reset. </param>
        /// <returns> returns IAsyncPolicy. </returns>
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
            int circuitBreakerThreshold,
            TimeSpan durationOfBreak,
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak,
            Action onReset)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .CircuitBreakerAsync<HttpResponseMessage>(circuitBreakerThreshold, durationOfBreak, onBreak, onReset);
        }

        /// <summary>
        /// Configuration for retry policy.
        /// </summary>
        /// <param name="retryCount"> Number of retry.</param>
        /// <param name="intervalBetweenRetry"> Interval between each retry. </param>
        /// <param name="onTry"> Action on each retry. </param>
        /// <returns> returns IAsyncPolicy. </returns>
        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(
            int retryCount,
            TimeSpan intervalBetweenRetry,
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onTry)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                retryCount,
                retryAttempt => intervalBetweenRetry,
                onTry);
        }
    }
}
