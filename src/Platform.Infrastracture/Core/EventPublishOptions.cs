using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infrastructure.Core
{
    public enum EventPublishOption
    {
        DoNotPublish = 0,
        InMemoryPublishOnly = 1,
        InMemoryAndQueuePublish = 2,
        QueuePublishOnly = 3

    }
}
