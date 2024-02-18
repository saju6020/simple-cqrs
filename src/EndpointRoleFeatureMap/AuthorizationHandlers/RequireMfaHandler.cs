namespace Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class RequireMfaHandler : AuthorizationHandler<RequireMfa>
    {
        private const string Amr = "amr";
        private const string AmrMfa = "amr_mfa";

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RequireMfa requirement)
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

            if (amrClaim != null && amrClaim.Value == AmrMfa)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
