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

    public IActionResult Login(string? returnUrl)
    {
      if (returnUrl != null)
      {
        TempData["ReturnUrl"] = returnUrl;
      }
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(SignInViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(viewModel.Email);
        if (user != null)
        {
          await _signInManager.SignOutAsync();

          var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, true);

          if (result.Succeeded)
          {
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);

            var returnUrl = TempData["ReturnUrl"];
            if (returnUrl != null)
            {
              return Redirect(returnUrl.ToString() ?? "/");
            }
            return RedirectToAction("Index", "Admin");
          }
          else if (result.RequiresTwoFactor)
          {
            return RedirectToAction("TwoFactorLogin", new { ReturnUrl = TempData["ReturnUrl"] });
          }
          else if (result.IsLockedOut)
          {
            var lockoutEndUtc = await _userManager.GetLockoutEndDateAsync(user);
            var timeLeft = lockoutEndUtc.Value - DateTime.UtcNow;
            ModelState.AddModelError(string.Empty, $"This account has been locked out, please try again {timeLeft.Minutes} minutes later.");
          }
          else if (result.IsNotAllowed)
          {
            ModelState.AddModelError(string.Empty, "You need to confirm your e-mail address.");
          }
          else
          {
            ModelState.AddModelError(string.Empty, "Invalid e-mail or password.");
          }
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Invalid e-mail or password.");
        }
      }
      return View(viewModel);
    }


  }
}
