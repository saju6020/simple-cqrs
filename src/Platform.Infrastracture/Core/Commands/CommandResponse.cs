namespace Platform.Infrastructure.Core.Commands
{
    using System.Collections.Generic;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Validation;

    /// <summary>Command response model.</summary>
    public class CommandResponse
    {
        public CommandResponse()
        {
        }

        public CommandResponse(ValidationResponse validationResult, object result)
        {
            this.ValidationResult = validationResult;
            this.Result = result;
        }

        public ValidationResponse ValidationResult { get; set; }

        public object Result { get; set; }

        public IEnumerable<IEvent> Events { get; set; }
    }
}
