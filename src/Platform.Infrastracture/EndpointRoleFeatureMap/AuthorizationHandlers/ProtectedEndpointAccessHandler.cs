namespace Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Platform.Infrastructure.EndpointRoleFeatureMap.Providers;

    internal class ProtectedEndpointAccessHandler : AuthorizationHandler<ProtectedEndpointAccessRequirement>
    {
        private const string VerticalIdHeaderName = "vertical-id";

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly EndpointRoleFeatureMapProvider serviceEndPointAccessRoleCache;

        public ProtectedEndpointAccessHandler(IHttpContextAccessor httpContextAccessor, EndpointRoleFeatureMapProvider serviceEndPointAccessRoleCache)
        {
            this.httpContextAccessor = httpContextAccessor;

            this.serviceEndPointAccessRoleCache = serviceEndPointAccessRoleCache;
        }

        private ClaimsIdentity OverrideVerticalidIfAvailableInHeader(ClaimsIdentity claimsIdentity)
        {
            var verticalIdHeader = this.httpContextAccessor.HttpContext.Request.Headers[VerticalIdHeaderName];

            var verticalHeaderStr = verticalIdHeader.ToString();
            var verticalClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == Common.Security.ClaimTypes.VerticalId);
            if (verticalClaim != null && !string.IsNullOrWhiteSpace(verticalHeaderStr))
            {
                claimsIdentity.RemoveClaim(verticalClaim);
            }

            claimsIdentity.AddClaim(new Claim(Common.Security.ClaimTypes.VerticalId, verticalIdHeader.ToString().Trim()));

            return claimsIdentity;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProtectedEndpointAccessRequirement requirement)
        {
            if (!(context.User.Identity is ClaimsIdentity claimsIdentity) || claimsIdentity.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            claimsIdentity = this.OverrideVerticalidIfAvailableInHeader(claimsIdentity);

            var routeData = this.httpContextAccessor.HttpContext.GetRouteData();

            var endPoint = $"{routeData.Values["controller"]}/{routeData.Values["action"]}";

            var hasAccess = this.serviceEndPointAccessRoleCache.CanAccess(endPoint, claimsIdentity);

            if (hasAccess)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
