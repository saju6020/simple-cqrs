using Microsoft.AspNetCore.Identity;

namespace SimpleCQRS.UAM.Database
{
    public class User: IdentityUser<Guid>
    { 
        public string PreferedLanguage { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastLoginAt { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
  