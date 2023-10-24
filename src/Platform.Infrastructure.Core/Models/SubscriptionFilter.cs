namespace Platform.PushNotificationSystem.Domain.Commands
{
    public class SubscriptionFilter
    {
        public string Context { get; set; }

        public string ActionName { get; set; }

        public string Value { get; set; }
    }
}