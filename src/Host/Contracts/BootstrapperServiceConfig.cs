namespace Platform.Infrastructure.Host.Contracts
{
    /// <summary>Class to pass user define configuration to bootstrapper service</summary>
    public class BootstrapperServiceConfig
    {
        /// <summary>Initializes a new instance of the <see cref="BootstrapperServiceConfig"/> class.</summary>
        public BootstrapperServiceConfig()
        {
            this.UseEndpointProtection = false;
        }

        /// <summary>Gets or sets a value indicating whether [use endpoint protection].</summary>
        /// <value>
        /// <c>true</c> if [use endpoint protection]; otherwise, <c>false</c>.</value>
        public bool UseEndpointProtection { get; set; }

        /// <summary>Gets or sets a value indicating whether [use end point mfa protection].</summary>
        /// <value>
        ///   <c>true</c> if [use end point mfa protection]; otherwise, <c>false</c>.</value>
        public bool UseEndPointMfaProtection { get; set; }

        /// <summary>Gets or sets a value indicating whether [use end point2fa protection].</summary>
        /// <value>
        ///   <c>true</c> if [use end point2fa protection]; otherwise, <c>false</c>.</value>
        public bool UseEndPoint2faProtection { get; set; }

        /// <summary>Gets or sets a value indicating whether [use shohoz localization].</summary>
        /// <value>
        /// <c>true</c> if [use shohoz localization]; otherwise, <c>false</c>.</value>
        public bool UseShohozLocalization { get; set; }

        /// <summary>Gets or sets a value indicating whether [use hash protection].</summary>
        /// <value>
        /// <c>true</c> if [use hash protection]; otherwise, <c>false</c>.</value>
        public bool UseEndPointHashProtection { get; set; }
    }
}
