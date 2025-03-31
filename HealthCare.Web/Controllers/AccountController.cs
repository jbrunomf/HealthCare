using HealthCare.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _manager;

        public AccountController(SignInManager<IdentityUser> manager)
        {_manager = manager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await _manager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt!");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _manager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
