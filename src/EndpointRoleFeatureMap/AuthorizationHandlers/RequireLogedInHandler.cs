namespace Shohoz.Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class RequireLogedInHandler : AuthorizationHandler<RequireLogedIn>
    {
        private const string Amr = "amr";
        private const string ShohozAuthenticateSite = "authenticate_site";

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RequireLogedIn requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (requirement == null)
            {
                throw new ArgumentNullException(nameof(requirement));
            }

            var amrClaim =
                context.User.Claims.FirstOrDefault(t => t.Type == Amr);

            if (amrClaim != null && amrClaim.Value != ShohozAuthenticateSite)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
