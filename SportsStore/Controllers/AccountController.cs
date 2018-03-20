using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        
        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr) {
            this._userManager = userMgr;
            this._signInManager = signInMgr;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl) {
            return View(new LoginViewModel {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel) {
            if (ModelState.IsValid) {
                IdentityUser user = await _userManager.FindByNameAsync(loginModel.Name);
                if (user != null) {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded) {
                        return Redirect(loginModel.ReturnUrl);
                    }
                }
            }
            ModelState.AddModelError("", "Login credentials invalid.");
            return View(loginModel);
        }

        [Authorize]
        public async Task<RedirectResult> Logout(string returnUrl = "/") {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}