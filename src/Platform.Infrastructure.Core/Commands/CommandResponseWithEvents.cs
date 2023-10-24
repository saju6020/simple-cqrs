namespace Platform.Infrastructure.Core.Commands
{
    using System.Collections.Generic;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Validation;

    /// <summary>Command response model with events.</summary>
    public class CommandResponseWithEvents
    {
        public ValidationResponse ValidationResult { get; set; }

        public object Result { get; set; }

        public IEnumerable<IEvent> Events { get; set; }
    }
}
