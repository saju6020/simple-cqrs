namespace Platform.Infrastructure.Host.WebApi
{
    /// <summary>Bootstrapper pipeline configuration.</summary>
    public class HostPipeLineConfig
    {
        public bool EnableAuthorization { get; set; } = false;

        public bool EnableServiceIdCheckerMiddleware { get; set; } = false;

        public bool EnableTenantIdCheckerMiddleware { get; set; } = false;
    }
}
