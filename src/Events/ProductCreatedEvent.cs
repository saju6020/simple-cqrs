using Platform.Infrastructure.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class ProductCreatedEvent: DomainEvent
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
