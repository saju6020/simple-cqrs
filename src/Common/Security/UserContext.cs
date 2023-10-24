namespace Platform.Infrastructure.Common.Security
{
    using System;

    /// <summary>User context class to present user information that comes thorugh request context.</summary>
    public class UserContext
    {
        private readonly Guid anonymousUserId = Guid.Parse("20c69484-b359-4a48-9155-d877198d5db4");

        public UserContext()
        {
            this.UserId = this.anonymousUserId;
        }

        public UserContext(Guid userId, Guid tenantId)
        {
            this.UserId = userId;
            this.TenantId = tenantId;
        }

        public UserContext(Guid userId, Guid tenantId, Guid verticalId)
        {
            this.UserId = userId;
            this.TenantId = tenantId;
            this.VerticalId = verticalId;
        }

        public Guid VerticalId { get; set; }

        public string ServiceId { get; set; }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public string SiteId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string[] Roles { get; set; } = new[] { "anonymous" };

        public string ClientId { get; set; }

        public string Audience { get; set; }

        public string TokenIssuer { get; set; }

        public string LanguageCode { get; set; }

        public string DisplayName { get; set; }

        public string DeviceId { get; set; }

        public void Set(UserContext context)
        {
            this.UserId = context.UserId;
            this.TenantId = context.TenantId;
            this.ServiceId = context.ServiceId;
            this.Roles = context.Roles;
            this.ClientId = context.ClientId;
            this.Audience = context.Audience;
            this.TokenIssuer = context.TokenIssuer;
            this.LanguageCode = context.LanguageCode;
            this.VerticalId = context.VerticalId;
            this.SiteId = context.SiteId;
            this.Email = context.Email;
            this.UserName = context.UserName;
            this.PhoneNumber = context.PhoneNumber;
            this.DisplayName = context.DisplayName;
            this.DeviceId = context.DeviceId;
        }

        public bool IsAnonymous() => this.UserId == this.anonymousUserId;
    }
}
