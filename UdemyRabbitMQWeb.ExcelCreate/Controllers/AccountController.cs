using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UdemyRabbitMQWeb.ExcelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(string Email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(Email);

            if (hasUser == null)
            {
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);

            if (!signInResult.Succeeded)
            {
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}