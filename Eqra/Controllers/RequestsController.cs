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
        private readonly ApplicationDbContext _context;

        public RequestsController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [Authorize(Roles = "مشرف")]
        public ActionResult Index()
        {
            return View(_context.Requests.ToList());
        }

        // GET: RequestsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

            var request = new Request()
            {
                Email = userLogged.Email,
                Bio = model.Bio,
                PhoneNumber = userLogged.PhoneNumber,
                Name = model.Name,
                NumberOfBooks = model.NumberOfBooks,
                Genre = Models.Enum.Genre.دراما,
                UserId = userLogged.Id
            };

            _context.Requests.Add(request);
            _context.SaveChanges();

            return Json(new {});
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
