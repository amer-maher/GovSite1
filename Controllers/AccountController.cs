using GovSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GovSite.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signIn;
        private readonly UserManager<AppUser> _users;

        public AccountController(SignInManager<AppUser> s, UserManager<AppUser> u)
        { _signIn = s; _users = u; }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
            => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _users.FindByEmailAsync(vm.Email);
            if (user == null) { ModelState.AddModelError("", "الحساب غير موجود."); return View(vm); }

            var res = await _signIn.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);
            if (res.Succeeded) return Redirect(string.IsNullOrWhiteSpace(vm.ReturnUrl) ? "/admin" : vm.ReturnUrl);
            if (res.IsLockedOut) ModelState.AddModelError("", "الحساب مقفول مؤقتًا.");
            else if (res.IsNotAllowed) ModelState.AddModelError("", "الدخول غير مسموح.");
            else ModelState.AddModelError("", "بيانات الدخول غير صحيحة.");
            return View(vm);
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
