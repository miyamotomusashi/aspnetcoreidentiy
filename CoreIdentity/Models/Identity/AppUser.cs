using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.Models.Identity
{
  public enum Gender { Unknown, Male, Female }
  public class AppUser : IdentityUser
  {
        public Gender Gender { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
