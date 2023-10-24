namespace Platform.Infrastructure.Authentication
{
    public class TokenValidationOptions
    {
        public string JwtTokenVerificationPublicCertificatePath { get; set; }

        public string JwtTokenVerificationPublicCertificatePassword { get; set; }

        public string TokenIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public string Audience { get; set; }
    }
}
