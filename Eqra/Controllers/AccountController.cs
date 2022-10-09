using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var role = new User
            {
                Name = "User"
            };

            return View();
        }



        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterViewModel model)
        {
            var user = new User() { 
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                Name = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(currentUser, "مستخدم");
            }


            return Json(new {correct = true});
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            await _signInManager.SignInAsync(user, isPersistent: true);
            return Redirect("/Home/");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "Account");
        }
    }
}
