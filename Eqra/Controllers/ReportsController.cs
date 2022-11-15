using Eqra.Data;
using Eqra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class ReportsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public ReportsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult MakeReport()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeReport(Report model)
        {
            var userLogged = await _userManager.GetUserAsync(User);

            _context.Reports.Add(new Report()
            {
                Content = model.Content,
                Title = model.Title,
                UserId = userLogged.Id
            });
            _context.SaveChanges();

            return RedirectToAction("Index", "Home", _context.Books.ToList());
        }
    }
}
