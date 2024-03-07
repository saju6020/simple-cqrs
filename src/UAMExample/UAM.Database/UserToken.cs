using Microsoft.AspNetCore.Identity;

namespace SimpleCQRS.UAM.Database
{
    public class UserToken: IdentityUserToken<Guid>
    {
        public virtual User User { get; set; }
    }
}
