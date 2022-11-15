using Eqra.Data;
using Eqra.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Eqra.ViewModels;

namespace Eqra.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public BooksController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, UserManager<User> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _userManager = userManager;
        }

        // GET: BooksController
        [Authorize(Roles = "كاتب")]
        public async Task<ActionResult> Index()
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var books = _context.Books.Where(o => o.AuthorId == userLogged.Id).ToList();
            return View(books);
        }

        // GET: BooksController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var book = _context.Books.FirstOrDefault(o => o.Id == id);
            var userLogged = await _userManager.GetUserAsync(User);
            ViewBag.CanTakeExam = true;

            if (_context.ExamLockouts.Where(o=>o.UserId == userLogged.Id && o.BookId == book.Id).FirstOrDefault() != null)
            {
                ViewBag.CanTakeExam = false;
            }
            

            return View(book);
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            return View();
        }

        public IActionResult BookSearch(BookSearchViewModel model)
        {
            if (model.Visiting == 1)
            {
                model.Books = new List<Book>();
                return View(model);
            }


            var books = _context.Books.ToList();


            if(model.Name != null)
            {
                books = books.Where(o => o.Name == model.Name).ToList();
            }
            if(model.Lanuage != 0)
            {
                books = books.Where(o=>o.BookLanuage == model.Lanuage).ToList();
            }
            if(model.Genre != 0)
            {
                books = books.Where(o => o.Genre == model.Genre).ToList();
            }
            if(model.Year != 0)
            {
                books = books.Where(o=>o.ReleaseDate.Year == model.Year).ToList();
            }
            if(model.Views != 0)
            {
                if(model.Views == 1)
                {
                    books = books.Where(o=>o.Views <= 10).ToList();
                }
                if (model.Views == 2)
                {
                    books = books.Where(o => o.Views >= 10).ToList();
                }
                if (model.Views == 3)
                {
                    books = books.Where(o => o.Views >= 50).ToList();
                }
                if (model.Views == 4)
                {
                    books = books.Where(o => o.Views >= 100).ToList();
                }
                if (model.Views == 5)
                {
                    books = books.Where(o => o.Views >= 150).ToList();
                }
                if (model.Views == 6)
                {
                    books = books.Where(o => o.Views >= 200).ToList();
                }
            }

            if(model.Pages != 0)
            {
                if(model.Pages == 1)
                {
                    books = books.Where(o=>o.Pages <= 100).ToList();
                }
                if (model.Pages == 2)
                {
                    books = books.Where(o => o.Pages >= 100).ToList();
                }
                if (model.Pages == 3)
                {
                    books = books.Where(o => o.Pages >= 150).ToList();
                }
                if (model.Pages == 4)
                {
                    books = books.Where(o => o.Pages >= 200).ToList();
                }
                if (model.Pages == 5)
                {
                    books = books.Where(o => o.Pages >= 250).ToList();
                }
                if (model.Pages == 6)
                {
                    books = books.Where(o => o.Pages >= 300).ToList();
                }
            }



            model.Books = books;

            return View(model);
        }

        // POST: BooksController/Create
        [HttpPost]
        [Authorize(Roles = "كاتب")]
        public async Task<ActionResult> Create(CreateBookViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);


            string coverFileName = "";
            if (model.CoverPath != null)
            {
                string dir = Path.Combine(_webHostEnvironment.WebRootPath, "Books", "Covers");
                coverFileName = Guid.NewGuid().ToString() + "-" + model.CoverPath.FileName;
                string path = Path.Combine(dir, coverFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.CoverPath.CopyTo(fileStream);
                }
            }

            string contentFileName = "";
            if (model.CoverPath != null)
            {
                string dir = Path.Combine(_webHostEnvironment.WebRootPath, "Books");
                contentFileName = Guid.NewGuid().ToString() + "-" + model.ContentPath.FileName;
                string path = Path.Combine(dir, contentFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    model.ContentPath.CopyTo(fileStream);
                }
            }



            var book = new Book()
            {
                AuthorId = userLogged.Id,
                ReleaseDate = DateTime.Now,
                Genre = model.Genre,
                Pages = model.Pages,
                Name = model.Name,
                Views = 0,
                Cover = coverFileName,
                Content = contentFileName,
                BookLanuage = model.Lanuage
            };

            _context.Books.Add(book);
            _context.SaveChanges();


            var questions = new List<Question>()
            {
                new Question(){Content = model.Q1.Content, A1 = model.Q1.A1, A2 = model.Q1.A2, A3 = model.Q1.A3, A4 = model.Q1.A4, CorrectAnswer = model.Q1.CorrectAnswer, BookId = book.Id},
                new Question(){Content = model.Q2.Content, A1 = model.Q2.A1, A2 = model.Q2.A2, A3 = model.Q2.A3, A4 = model.Q2.A4, CorrectAnswer = model.Q2.CorrectAnswer, BookId = book.Id},
                new Question(){Content = model.Q3.Content, A1 = model.Q3.A1, A2 = model.Q3.A2, A3 = model.Q3.A3, A4 = model.Q3.A4, CorrectAnswer = model.Q3.CorrectAnswer, BookId = book.Id},
                new Question(){Content = model.Q4.Content, A1 = model.Q4.A1, A2 = model.Q4.A2, A3 = model.Q4.A3, A4 = model.Q4.A4, CorrectAnswer = model.Q4.CorrectAnswer, BookId = book.Id},
                new Question(){Content = model.Q5.Content, A1 = model.Q5.A1, A2 = model.Q5.A2, A3 = model.Q5.A3, A4 = model.Q5.A4, CorrectAnswer = model.Q5.CorrectAnswer, BookId = book.Id}
            };

            foreach(var question in questions)
            {
                question.A1 = "1" + question.A1;
                question.A2 = "2" + question.A2;
                question.A3 = "3" + question.A3;
                question.A4 = "4" + question.A4;
                _context.Questions.Add(question);
                _context.SaveChanges();
            }

            return View();
        }

        // GET: BooksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BooksController/Edit/5
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

        [HttpPost]
        public JsonResult Delete([FromBody] BookDelViewModel model)
        {
            var book = _context.Books.Where(o => o.Id == model.Id).FirstOrDefault();
            var reviews = _context.Reviews.Where(o => o.BookId == book.Id).ToList();
            var examLockouts = _context.ExamLockouts.Where(o => o.BookId == book.Id).ToList();
            var questions = _context.Questions.Where(o => o.BookId == book.Id).ToList();

            foreach(var review in reviews)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
            foreach(var examLockout in examLockouts)
            {
                _context.ExamLockouts.Remove(examLockout);
                _context.SaveChanges();
            }
            foreach(var question in questions)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            return Json(new { });
        }


    }
}
