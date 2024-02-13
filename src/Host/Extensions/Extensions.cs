namespace Platform.Infrastructure.Host.Extensions
{
    using Platform.Infrastructure.Host.Contracts;

    /// <summary>Class to contain bootstrapper extensions</summary>
    public static class Extensions
    {
        /// <summary>Uses AppInSight log.</summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>bootstrapper.</returns>
        public static IBootstrapper UseAppInSightLog(this IBootstrapper bootstrapper)
        {
            bootstrapper.LogConfiguration.UseAppInSight();
            return bootstrapper;
        }

        /// <summary>Uses File log.</summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>bootstrapper.</returns>
        public static IBootstrapper UseFileLog(this IBootstrapper bootstrapper)
        {
            bootstrapper.LogConfiguration.UseFile();
            return bootstrapper;
        }

        /// <summary>Uses AzureBlob log.</summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>bootstrapper.</returns>
        public static IBootstrapper UseAzureBlobLog(this IBootstrapper bootstrapper)
        {
            bootstrapper.LogConfiguration.UseAzureBlob();
            return bootstrapper;
        }

        /// <summary>
        /// Uses Amazon Cloud watch.
        /// </summary>
        /// <param name="bootstrapper">IBootstrapper.</param>
        /// <returns>bootstrapper.</returns>
        public static IBootstrapper UseAmazonCloudWatch(this IBootstrapper bootstrapper)
        {
            bootstrapper.LogConfiguration.UserAwsCloud();
            return bootstrapper;
        }

        /// <summary>
        /// Uses Seq server.
        /// </summary>
        /// <param name="bootstrapper">IBootstrapper.</param>
        /// <returns>IBootstrapper.</returns>
        public static IBootstrapper UseSeq(this IBootstrapper bootstrapper)
        {
            bootstrapper.LogConfiguration.UseSeq();
            return bootstrapper;
        }
    }
}
