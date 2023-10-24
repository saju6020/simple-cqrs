namespace Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class Require2faHandler : AuthorizationHandler<Require2fa>
    {
        private const string Amr = "amr";
        private const string Mfa = "2fa";

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            Require2fa requirement)
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

            if (amrClaim != null && amrClaim.Value == Mfa)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
