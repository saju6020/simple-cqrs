using FluentValidation.Results;
using Platform.Infrastructure.Core.Domain;

namespace Events
{
    public abstract class FailedToProcessEvent:DomainEvent
    {
        public FailedToProcessEvent(string exception = null, ValidationResult validationResult = null)
        {
            if (exception != null)
            {
                this.Exception = exception;
            }

            if (validationResult != null)
            {
                this.ValidationResult = validationResult;
            }
        }

        public string Exception { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public abstract string GetMessage();
    }
}
