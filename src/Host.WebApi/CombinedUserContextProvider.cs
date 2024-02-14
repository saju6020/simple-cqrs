namespace Platform.Infrastructure.Host.WebApi
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Accessors;

    public class CombinedUserContextProvider : IUserContextProvider
    {
        private readonly IContextAccessor contextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CombinedUserContextProvider(IContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.contextAccessor = contextAccessor;
            this.httpContextAccessor = httpContextAccessor;
        }

        public UserContext GetUserContext()
        {
            if (this.httpContextAccessor != null && this.httpContextAccessor.HttpContext != null)
            {
                return this.GetHttpUserContext();
            }

            return this.contextAccessor.Context.UserContext;
        }

        private UserContext GetHttpUserContext()
        {
            if (!(this.httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity claimsIdentity) || claimsIdentity.IsAuthenticated == false)
            {
                return new UserContext();
            }

            var userContext = new UserContext
            {
                Email = claimsIdentity.HasClaim(claim => claim.Type.Equals(Common.Security.ClaimTypes.Email)) ? claimsIdentity.Claims.First(c => c.Type == Common.Security.ClaimTypes.Email).Value.ToLower() : "info@shohoz.com",
                LanguageCode = "en-US",
                PhoneNumber = claimsIdentity.HasClaim(claim => claim.Type.Equals(Common.Security.ClaimTypes.PhoneNumber)) ? claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.PhoneNumber)).Value : "no-phone",
                Roles = claimsIdentity.Claims.Where(c => c.Type == Common.Security.ClaimTypes.Role).Select(c => c.Value).ToArray(),
                SiteId = claimsIdentity.HasClaim(claim => claim.Type.Equals(Common.Security.ClaimTypes.SiteId)) ? claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.SiteId)).Value : "anonymous",
                TenantId = Guid.Parse(claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.TenantId)).Value),
                UserId = Guid.Parse(claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.UserId)).Value),
                UserName = claimsIdentity.HasClaim(claim => claim.Type.Equals(Common.Security.ClaimTypes.UserName)) ? claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.UserName)).Value : "anonymous",
                VerticalId = Guid.Parse(claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.VerticalId)).Value),
                ClientId = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.ClientId)).Value,
                ServiceId = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.ServiceId)).Value,
                Audience = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.Audience)).Value,
                TokenIssuer = claimsIdentity.Claims.First(c => c.Type.Equals(Common.Security.ClaimTypes.TokenIssuer)).Value,
            };

            return userContext;
        }
    }
}
