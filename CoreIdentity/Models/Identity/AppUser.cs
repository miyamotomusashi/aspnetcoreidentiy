using Microsoft.AspNetCore.Identity;
using static CoreIdentity.Models.Enums;

namespace CoreIdentity.Models.Identity
{
    public class AppUser : IdentityUser
  {
    public Gender Gender { get; set; }
    public DateTime CreatedOn { get; set; }
    public TwoFactorType TwoFactorType { get; set; }
    public DateTime BirthDay { get; set; }
  }
}
