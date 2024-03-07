namespace Platform.Infrastructure.Bus.RabbitMQ
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MassTransit;
    using Platform.Infrastructure.Core.Bus;

    /// <summary>
    ///   <br />
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    [ExcludeFromCodeCoverage]
    public class CommandConsumerAdapter<TCommand> : IConsumer<TCommand>, IConsumer
        where TCommand : Message
    {
        public Task Consume(ConsumeContext<TCommand> context)
        {
            return Task.CompletedTask;
        }
    }
}
