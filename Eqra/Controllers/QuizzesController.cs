using Eqra.Data;
using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public QuizzesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> TakeQuiz(Guid id)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var book = _context.Books.Where(o => o.Id == id).FirstOrDefault();
            var questions = _context.Questions.Where(o=>o.BookId == book.Id).ToList();



            foreach (var question in questions)
            {
                question.A1 = question.A1.Remove(0, 1);
                question.A2 = question.A2.Remove(0, 1);
                question.A3 = question.A3.Remove(0, 1);
                question.A4 = question.A4.Remove(0, 1);
            }

            return View(questions);
        }

        public async Task<JsonResult> SubmitQuiz([FromBody] QuizSubmissionViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var book = _context.Books.Where(o => o.Id == model.BookId).FirstOrDefault();
            var questions = _context.Questions.Where(o => o.BookId == book.Id).ToList();

            _context.ExamLockouts.Add(new ExamLockout()
            {
                BookId = book.Id,
                UserId = userLogged.Id
            });
            _context.SaveChanges();

            List<string> answers = new List<string>()
            {
                model.Q1Answer,
                model.Q2Answer,
                model.Q3Answer,
                model.Q4Answer,
                model.Q5Answer,
            };


            for (var i = 0; i < questions.Count; i++)
            {
                if (questions[i].CorrectAnswer != answers[i])
                {
                    return Json(new { correct = false });
                }
            }

            userLogged.Points += 10;
            await _userManager.UpdateAsync(userLogged);

            return Json(new { correct = true });
        }

        // GET: QuizzesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: QuizzesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuizzesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuizzesController/Create
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

        // GET: QuizzesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuizzesController/Edit/5
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

        // GET: QuizzesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuizzesController/Delete/5
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
