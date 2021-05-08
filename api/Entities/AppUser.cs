using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace api.Entities {
    public class AppUser : IdentityUser<int> {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}