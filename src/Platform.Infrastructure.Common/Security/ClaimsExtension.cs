namespace Platform.Infrastructure.Common.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsExtension
    {
        public static UserContext GetAuthData(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                return new UserContext();
            }

            var claims = claimsPrincipal.Claims.ToList();
            var verticalId = claims.GetClaimValue(ClaimTypes.VerticalId, Guid.Empty);
            var tenantId = claims.GetClaimValue(ClaimTypes.TenantId, Guid.Empty);
            var userId = claims.GetClaimValue(ClaimTypes.UserId, Guid.Empty);

            if (tenantId == Guid.Empty || userId == Guid.Empty)
            {
                return new UserContext();
            }

            var userContext = new UserContext(userId, tenantId)
            {
                VerticalId = verticalId,
                Email = claims.GetClaimValue(ClaimTypes.Email),
                LanguageCode = claims.GetClaimValue(ClaimTypes.Language),
                SiteId = claims.GetClaimValue(ClaimTypes.SiteId),
                UserName = claims.GetClaimValue(ClaimTypes.UserName),
                PhoneNumber = claims.GetClaimValue(ClaimTypes.PhoneNumber),
                ClientId = claims.GetClaimValue(ClaimTypes.ClientId),
                ServiceId = claims.GetClaimValue(ClaimTypes.ServiceId),
            };

            var roles = claims.GetClaimValues(ClaimTypes.Role);
            if (roles != null)
            {
                userContext.Roles = roles.ToArray();
            }

            return userContext;
        }

        public static ClaimsPrincipal GetPrincipal(this UserContext userContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.TenantId, userContext.TenantId.ToString()),
                new Claim(ClaimTypes.UserId, userContext.UserId.ToString()),
            };

            if (userContext.VerticalId != Guid.Empty)
            {
                claims.Add(new Claim(ClaimTypes.VerticalId, userContext.VerticalId.ToString()));
            }

            if (!string.IsNullOrEmpty(userContext.LanguageCode))
            {
                claims.Add(new Claim(ClaimTypes.Language, userContext.LanguageCode));
            }

            if (!string.IsNullOrEmpty(userContext.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, userContext.Email));
            }

            if (!string.IsNullOrEmpty(userContext.UserName))
            {
                claims.Add(new Claim(ClaimTypes.UserName, userContext.UserName));
            }

            if (!string.IsNullOrEmpty(userContext.PhoneNumber))
            {
                claims.Add(new Claim(ClaimTypes.PhoneNumber, userContext.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(userContext.TokenIssuer))
            {
                claims.Add(new Claim(ClaimTypes.TokenIssuer, userContext.TokenIssuer));
            }

            if (!string.IsNullOrEmpty(userContext.SiteId))
            {
                claims.Add(new Claim(ClaimTypes.SiteId, userContext.SiteId));
            }

            if (!string.IsNullOrEmpty(userContext.ClientId))
            {
                claims.Add(new Claim(ClaimTypes.ClientId, userContext.ClientId));
            }

            if (!string.IsNullOrEmpty(userContext.ServiceId))
            {
                claims.Add(new Claim(ClaimTypes.ServiceId, userContext.ServiceId));
            }

            if (userContext.Roles != null && userContext.Roles.Length > 0)
            {
                foreach (var role in userContext.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (userContext.Audience != null && userContext.Audience.Length > 0)
            {
                claims.Add(new Claim(ClaimTypes.Audience, userContext.Audience));
            }

            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }

        private static string GetClaimValue(this IEnumerable<Claim> claims, string typeKey, string? defaultValue = null)
        {
            if (claims == null)
            {
                return defaultValue;
            }

            var targetClaim = claims.FirstOrDefault(c => c.Type == typeKey);
            return targetClaim == null ? defaultValue : targetClaim.Value;
        }

        private static Guid GetClaimValue(this IEnumerable<Claim> claims, string typeKey, Guid defaultValue)
        {
            if (claims == null)
            {
                return Guid.Empty;
            }

            var targetClaim = claims.FirstOrDefault(c => c.Type == typeKey);
            return targetClaim == null ? Guid.Empty : Guid.Parse(targetClaim.Value);
        }

        private static List<string> GetClaimValues(this IEnumerable<Claim> claims, string typeKey, string? defaultValue = null)
        {
            if (claims == null)
            {
                return null;
            }

            var targetClaims = claims.Where(c => c.Type == typeKey).ToList();
            return targetClaims.Count <= 0 ? null : targetClaims.Select(p => p.Value).ToList();
        }
    }
}
