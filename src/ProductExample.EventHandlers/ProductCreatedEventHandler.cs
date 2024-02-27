using Events;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExample.EventHandlers
{
    internal class ProductCreatedEventHandler : IEventHandlerAsync<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IRepo
    }
}
