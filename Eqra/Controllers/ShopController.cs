using Eqra.Data;
using Eqra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class ShopController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public ShopController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userLogged = await _userManager.GetUserAsync(User);

            ViewBag.Points = userLogged.Points;

            var model = _context.Coupons.Where(o=> o.EndingDate > DateTime.Now && o.Used == false).ToList();

            return View(model);
        }



    }
}
