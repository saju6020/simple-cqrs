using System.Security.Claims;

namespace SimpleCQRS.UAM.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
