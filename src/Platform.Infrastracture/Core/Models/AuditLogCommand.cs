namespace Platform.AuditLog.Domain.Commands
{
    using System;
    using Platform.Infrastructure.Core.Commands;

    public class AuditLogCommand : Command
    {
        public string ItemId { get; set; }

        public string Action { get; set; }

        public long Clock { get; set; }

        public string Controller { get; set; }

        public Guid UserId { get; set; }

        public long EndTS { get; set; }

        public string Ip { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string PayLoad { get; set; }

        public string[] Roles { get; set; }

        public string ServiceId { get; set; }

        public long StartTS { get; set; }

        public int StatusCode { get; set; }

        public Guid TenantId { get; set; }

        public string Verb { get; set; }

        public Guid VerticalId { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public DateTime LogTime { get; set; }
    }
}
