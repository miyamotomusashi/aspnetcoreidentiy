using CoreIdentity.Models.Identity;
using static CoreIdentity.Models.Enums;

namespace CoreIdentity.Models.ViewModels
{
  public class SignupViewModel
  {
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public Gender Gender { get; set; } = Gender.Unknown;
    public DateTime BirthDay { get; set; }
    public string Password { get; set; } = default!;
  }
}
