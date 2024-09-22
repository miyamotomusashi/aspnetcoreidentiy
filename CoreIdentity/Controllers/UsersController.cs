using CoreIdentity.Helpers;
using CoreIdentity.Models.Identity;
using CoreIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CoreIdentity.Models.Enums;

namespace CoreIdentity.Controllers
{
  public class UsersController : Controller
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly EMailHelper _eMailHelper;

    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EMailHelper malihelper)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _eMailHelper = malihelper;

    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(SignupViewModel model)
    {
      if (ModelState.IsValid)
      {
        var newUser = new AppUser
        {
          UserName = model.UserName,
          Email = model.Email,
          Gender = model.Gender,
          BirthDay = model.BirthDay,
          TwoFactorType = TwoFactorType.None
        };

        var result = await _userManager.CreateAsync(newUser);
        if (result.Succeeded)
        {
          var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
          var confirmationLink = Url.Action("ConfirmatimEmail", "User", new
          {
            userId = newUser.Id,
            token = confirmationToken
          }, HttpContext.Request.Scheme);

          await _eMailHelper.SendEmail(new()
          {
            Subject = "Confirm e-mail",
            Body = $"Please <a href='{confirmationLink}'>click</a> to confirm your e-mail address.",
            To = newUser.Email
          });

          return RedirectToAction("Login");

        }
        result.Errors.ToList().ForEach(f => ModelState.AddModelError(string.Empty, f.Description));
      }

      return View(model);

    }


    public IActionResult Index()
    {
      return View();
    }
  }
}
