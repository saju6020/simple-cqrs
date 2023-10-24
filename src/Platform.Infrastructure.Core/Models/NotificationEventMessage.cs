namespace Platform.Infrastructure.Core.Models
{
    using Platform.Infrastructure.Core.Events;

    public class NotificationEventMessage
    {
        public NotificationEventMessage()
        {
        }

        public NotificationEventMessage(string actionName)
        {
            this.ActionName = actionName;
        }

        public NotificationEventMessage(string actionName, string entityId)
            : this(actionName)
        {
            this.EntityId = entityId;
        }

        public NotificationEventMessage(string actionName, string entityId, string entityName)
            : this(actionName, entityId)
        {
            this.EntityName = entityName;
        }

        public string ActionName { get; set; }

        public string EntityName { get; set; }

        public string EntityId { get; set; }

        public EventMessage[] EventMessages { get; set; }

        public static NotificationEventMessage CreateSuccessEventMessage(string entityId, string actionName, string? entityName = null)
        {
            return new NotificationEventMessage()
            {
                EntityId = entityId,
                ActionName = actionName,
                EntityName = entityName,
                EventMessages = new EventMessage[] { new EventMessage(0, EventMessageType.Success), },
            };
        }
    }
}
