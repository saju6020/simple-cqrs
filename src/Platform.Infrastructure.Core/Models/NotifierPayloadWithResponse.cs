namespace Platform.PushNotificationSystem.Domain.Commands
{
    public class NotifierPayloadWithResponse : NotifierPayload
    {
        public string ReferenceId { get; set; }

        public string ResponseKey { get; set; }

        public string ResponseValue { get; set; }

        public bool IsMessageOnlyNotification { get; set; }

        public object Message { get; set; }

        public bool IsViaFirebase { get; set; }

        public bool SaveAsOfflineNotification { get; set; }
    }
}