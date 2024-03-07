namespace Platform.PushNotificationSystem.Domain.Commands
{
    public enum NotificationReceiverTypes
    {
        NoReceiverType,
        BroadcastReceiverType,
        UserSpecificReceiverType,
        FilterSpecificReceiverType,
        UserAndRoleSpecificReceiverType,
        FilterAndRoleSpecificReceiverType,
    }
}