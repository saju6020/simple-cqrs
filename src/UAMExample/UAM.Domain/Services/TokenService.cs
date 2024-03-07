using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using UAM.WebService.Configuration;

namespace SimpleCQRS.UAM.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;

        public TokenService(JwtBearerTokenSettings jwtBearerTokenSettings) 
        { 
            this._jwtBearerTokenSettings = jwtBearerTokenSettings;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var x509 = new X509Certificate2(File.ReadAllBytes(_jwtBearerTokenSettings.CertificatePath));
            return new X509SigningCredentials(x509, SecurityAlgorithms.HmacSha256Signature);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            DateTime now = DateTime.UtcNow;

            SigningCredentials signingCredentials = GetSigningCredentials();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = signingCredentials,
                Issuer = _jwtBearerTokenSettings.Issuer,               
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string strToken = tokenHandler.WriteToken(token);

            return strToken;
        }
    }
}
