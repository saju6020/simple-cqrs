namespace Platform.Infrastructure.Core.Events
{
    using System;

    public class EventDocument
    {
        public Guid Id { get; set; }

        public Guid AggregateId { get; set; }

        public string AggregateType { get; set; }

        public long Sequence { get; set; }

        public string Type { get; set; }

        public object Data { get; set; }

        public DateTime TimeStamp { get; set; }

        public Guid UserId { get; set; }
    }
}
