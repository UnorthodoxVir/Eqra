using Eqra.Data;
using Eqra.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public ReviewsController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ReviewsController
        public ActionResult Index()
        {
            return View();
        }



        // POST: ReviewsController/Create
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] Review model)
        {
            model.Date = DateTime.Now;
            _context.Reviews.Add(model);
            _context.SaveChanges();

            model.User = await _userManager.GetUserAsync(User);

            return Json(new {review = model});
        }

        // GET: ReviewsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReviewsController/Edit/5
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

        // GET: ReviewsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewsController/Delete/5
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
