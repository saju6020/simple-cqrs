namespace Platform.Infrastructure.Core.Bus
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Platform.Infrastructure.Core.Commands;

    /// <summary>Command handler abstraction.</summary>
    /// <typeparam name="T">Command.</typeparam>
    /// <seealso cref="Platform.Infrastructure.Core.Commands.ICommandHandlerAsync{T}" />
    public abstract class CommandHandlerAsync<T> : ICommandHandlerAsync<T>
        where T : class, ICommand
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        protected CommandHandlerAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public abstract Task<CommandResponse> HandleAsync(T command);
    }
}
