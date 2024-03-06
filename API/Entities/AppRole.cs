using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
