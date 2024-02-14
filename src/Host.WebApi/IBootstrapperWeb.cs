namespace Platform.Infrastructure.Host.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Platform.Infrastructure.Host.Contracts;

    /// <summary>Class to bootstrapp web api.</summary>
    /// <seealso cref="Core.Infrastructure.Host.Contracts.IBootstrapper" />
    public interface IBootstrapperWeb : IBootstrapper
    {
        /// <summary>Builds the pipe line.</summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="pipeLineConfig">The pipe line configuration.</param>
        /// <returns>Application builder.</returns>
        IApplicationBuilder BuildPipeLine(IApplicationBuilder applicationBuilder, BootstrapperPipeLineConfig pipeLineConfig);
    }
}
