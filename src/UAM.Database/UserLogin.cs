using Microsoft.AspNetCore.Identity;

namespace SimpleCQRS.UAM.Database
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}
