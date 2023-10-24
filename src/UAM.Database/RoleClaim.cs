using Microsoft.AspNetCore.Identity;

namespace SimpleCQRS.UAM.Database
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
