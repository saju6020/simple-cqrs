using Microsoft.AspNetCore.Identity;

namespace SimpleCQRS.UAM.Database
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}
