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
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                LockoutEnabled = false
            };

            var emails = _userManager.Users.Select(o => o.Email).ToList();

            if (emails.Contains(user.Email))
            {
                return Json(new { correct = false });
            }

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
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            return Redirect("/Home/");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "Account");
        }


        public async Task<IActionResult> Profile()
        {
            var userLogged = await _userManager.GetUserAsync(User);

            var model = new UserProfileViewModel()
            {
                Email = userLogged.Email,
                Phone = userLogged.PhoneNumber,
                Name = userLogged.Name
            };

            if (User.IsInRole("مستخدم"))
                model.UserType = "مستخدم";
            if (User.IsInRole("كاتب"))
                model.UserType = "كاتب";
            if (User.IsInRole("مشرف"))
                model.UserType = "مشرف";

            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateInfo([FromBody] UpdateInfoViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var PasswordResult = new IdentityResult();
            var EmailResult = new IdentityResult();
            if(model.NewPassword != null)
            {
                PasswordResult = await _userManager.ChangePasswordAsync(userLogged, model.CurrentPassword, model.NewPassword);

                
            }
            if(model.Email != userLogged.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(userLogged, model.Email);
                EmailResult = await _userManager.ChangeEmailAsync(userLogged, model.Email, token);

            }
            
            if(PasswordResult.Succeeded)
            {
                return Json(new { correct = true });
            }
            else
            {
                return Json(new { correct = false });
            }

        }

    }
}
