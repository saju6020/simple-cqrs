namespace Platform.Infrastructure.Throttling
{
    using System.Threading.Tasks;

    /// <summary>RateLImitService interaface.</summary>
    public interface IRateLimitService
    {
        /// <summary>Determines whether [is allowed to proceed].</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<RateLimitResponse> IsAllowedToProceed(string clientId);
    }
}
