namespace Platform.PushNotificationSystem.Domain.Commands
{
    using System;
    using System.Collections.Generic;

    public class NotifierPayload
    {
        public string ConnectionId { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UserId { get; set; }

        public Guid[] UserIds { get; set; }

        public List<string> Roles { get; set; }

        public List<SubscriptionFilter> SubscriptionFilters { get; set; }

        public NotificationReceiverTypes NotificationType { get; set; }

        public string DeviceType { get; set; }

        public bool KeepSilent { get; set; }

        public string DenormalizedPayload { get; set; }

        public bool EnablePersistence { get; set; }

        public bool SaveDenormalizedPayloadAsString { get; set; }
    }
}