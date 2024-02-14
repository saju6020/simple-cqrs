namespace Platform.Infrastructure.Host.WebApi
{
    /// <summary>Bootstrapper pipeline configuration.</summary>
    public class BootstrapperPipeLineConfig
    {
        public bool EnableAuthorization { get; set; } = false;

        public bool EnableServiceIdCheckerMiddleware { get; set; } = false;

        public bool EnableVerticalIdCheckerMiddleware { get; set; } = false;
    }
}
