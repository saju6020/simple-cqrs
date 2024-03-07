namespace Platform.Infrastructure.Throttling
{
    public class RateLimitResponse
    {
        public bool IsAllowedToProceed { get; set; }

        public string Message { get; set; }
    }
}
