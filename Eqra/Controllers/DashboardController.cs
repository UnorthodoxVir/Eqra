using Eqra.Data;
using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eqra.Controllers
{
    [Authorize(Roles = "مشرف")]
    public class DashboardController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public DashboardController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context; 
        }

        // GET: DashboardController
        public async Task<ActionResult> Index()
        {

            var users = _userManager.Users.ToList();
            var model = new List<UsersViewModel>();

            foreach (var user in users)
            {
                model.Add(new UsersViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Name,
                    Role = string.Join(",", _userManager.GetRolesAsync(user).Result.ToArray())
                });
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageBooks()
        {

            var books = _context.Books.ToList();

            foreach(var book in books)
            {
                book.Author = await _userManager.FindByIdAsync(book.AuthorId);
            }

            return View(books);
        }

        [HttpGet]
        public async Task<ActionResult> Reports()
        {
            var reports = _context.Reports.ToList();

            foreach(var report in reports)
            {
                report.User = await _userManager.FindByIdAsync(report.UserId);
            }

            return View(reports);
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(Guid id)
        {
            var report = _context.Reports.Where(o => o.Id == id).FirstOrDefault();
            _context.Reports.Remove(report);
            _context.SaveChanges();
            return RedirectToAction("Reports");
        }

        // POST: DashboardController/Delete/5

        public async Task<JsonResult> EditRole([FromBody] EditRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var roles = await _userManager.GetRolesAsync(user);

            var books = _context.Books.Where(o => o.AuthorId == user.Id).ToList();

            if(books.Count != 0)
            {
                return Json(new { correct = false });
            }

            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            await _userManager.AddToRoleAsync(user, model.Role);
          
            return Json(new {correct = true});
        }


    }
}
