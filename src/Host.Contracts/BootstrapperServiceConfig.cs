namespace Shohoz.Platform.Infrastructure.Host.Contracts
{
    /// <summary>Class to pass user define configuration to bootstrapper service</summary>
    public class BootstrapperServiceConfig
    {
        /// <summary>Initializes a new instance of the <see cref="BootstrapperServiceConfig"/> class.</summary>
        public BootstrapperServiceConfig()
        {
            this.UseJwtBearerAuthentication = false;
            this.UseEndpointProtection = false;
        }

        /// <summary>Gets or sets a value indicating whether [use JWT bearer authentication].</summary>
        /// <value>
        /// <c>true</c> if [use JWT bearer authentication]; otherwise, <c>false</c>.</value>
        public bool UseJwtBearerAuthentication { get; set; }

        /// <summary>Gets or sets a value indicating whether [use endpoint protection].</summary>
        /// <value>
        /// <c>true</c> if [use endpoint protection]; otherwise, <c>false</c>.</value>
        public bool UseEndpointProtection { get; set; }
    }
}
