using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.Models.Identity
{
  public class AppRole : IdentityRole
  {
        public DateTime CreatedOn { get; set; }
    }
}
