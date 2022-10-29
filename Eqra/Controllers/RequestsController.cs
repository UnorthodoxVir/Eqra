using Eqra.Data;
using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class RequestsController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RequestsController(UserManager<User> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [Authorize(Roles = "مشرف")]
        public ActionResult Index()
        {
            return View(_context.Requests.ToList());
        }

        [Authorize(Roles = "مشرف")]
        // GET: RequestsController/Details/5
        public ActionResult Details(Guid id)
        {
            var request = _context.Requests.Where(o => o.Id == id).FirstOrDefault();

            return View(request);
        }

        public async Task<ActionResult> RequestAccept(Guid id)
        {
            var request = _context.Requests.Where(o => o.Id == id).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            await _userManager.AddToRoleAsync(user, "كاتب");

            _context.Requests.Remove(request);
            _context.SaveChanges();

            return RedirectToAction("Index", _context.Requests.ToList());
            
        }
        public async Task<ActionResult> RequestDecline(Guid id)
        {
            var request = _context.Requests.Where(o => o.Id == id).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());



            _context.Requests.Remove(request);
            _context.SaveChanges();

            return RedirectToAction("Index", _context.Requests.ToList());
        }


        // GET: RequestsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestsController/Create
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] CreateRequestViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);

            var result = _context.Requests.Where(o => o.UserId == userLogged.Id).FirstOrDefault();

            if(result != null)
            {
                return Json(new {correct = false});
            }

            var request = new Request()
            {
                Email = userLogged.Email,
                Bio = model.Bio,
                PhoneNumber = userLogged.PhoneNumber,
                Name = model.Name,
                NumberOfBooks = model.NumberOfBooks,
                Genre = (Models.Enum.Genre)model.Genre,
                UserId = userLogged.Id
            };

            _context.Requests.Add(request);
            _context.SaveChanges();

            return Json(new {correct = true});
        }

        // GET: RequestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RequestsController/Edit/5
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

        // GET: RequestsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RequestsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
