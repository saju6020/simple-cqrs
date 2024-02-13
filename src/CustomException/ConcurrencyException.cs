namespace Platform.Infrastructure.CustomException
{
    using System;

    /// <summary>ConcurrencyException class.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.CustomException.BaseException" />
    public class ConcurrencyException : BaseException
    {
        public ConcurrencyException(Guid aggregateRootId, int expectedVersion, int actualVersion)
            : base(BuildErrorMessage(aggregateRootId, expectedVersion, actualVersion))
        {
        }

        private static string BuildErrorMessage(Guid aggregateRootId, int expectedVersion, int actualVersion)
        {
            return "Concurrency Exception" +
                   $" | AggregateRootId: {aggregateRootId.ToString()}" +
                   $" | Expected version: {expectedVersion}" +
                   $" | Actual version: {actualVersion}";
        }
    }
}