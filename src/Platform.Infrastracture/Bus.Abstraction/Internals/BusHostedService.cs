namespace Platform.Infrastructure.Bus.Abstraction.Internals
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public class BusHostedService : IHostedService
    {
        private readonly IBusControl busControl;

        public BusHostedService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StopAsync(cancellationToken);
        }
    }
}
