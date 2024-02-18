using FluentValidation.Results;
using Newtonsoft.Json;
using Platform.Infrastructure.Core.Events;
using Platform.Infrastructure.Core.Models;

namespace Events
{
    public class ProductBusinessRuleViolationEvent : FailedToProcessEvent
    {
        public ProductBusinessRuleViolationEvent(
           string actionName,
           EventMessage[] eventMessages = null,
           string exception = null,
           ValidationResult validationResult = null)
           : base(exception, validationResult)
        {
            this.Action = actionName;
            this.EventMessages = eventMessages;
        }
        public string Action { get; set;}
        public EventMessage[] EventMessages { get; set; }

        public override string GetMessage()
        {
            NotificationEventMessage errorObj = new NotificationEventMessage(this.Action)
            {
                EventMessages = this.EventMessages,
            };

            return JsonConvert.SerializeObject(errorObj);
        }
    }
}
