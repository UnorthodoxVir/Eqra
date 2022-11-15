using Eqra.Data;
using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Eqra.Models.Enum;

namespace Eqra.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index(Genre? genre, DateTime? From, DateTime? To)
        {
            var userLogged = await _userManager.GetUserAsync(User);

            ViewBag.Name = userLogged.Name;

            var SD = new DateTime();
            var ED = new DateTime();

            if (From == null || To == null)
            {
                SD = new DateTime(2021, 01, 01);
                ED = DateTime.Now;
            }
            else
            {
                SD = From.Value;
                ED = To.Value;
            }
            
            var currentUser = await _userManager.GetUserAsync(User);
            var books = _context.Books.Where(o=> o.ReleaseDate >= SD && o.ReleaseDate < ED.AddDays(1)).ToList();
            var model = new HomeViewModel();
            model.Books = books;
            model.MostViewed = _context.Books.OrderByDescending(o => o.Views).Take(3).ToList();
            if (genre != 0 && genre != null)
            {
                model.Books = model.Books.Where(o => o.Genre == genre).ToList();
            }

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var book = _context.Books.Where(o => o.Id == id).FirstOrDefault();
            book.Author = _context.Users.Where(o => o.Id == book.AuthorId).FirstOrDefault();
            var model = new BookDetailsViewModel();
            model.Book = book;
            model.SuggestedBooks = _context.Books.Where(o => o.Genre == book.Genre && !(o.Id == model.Book.Id)).ToList();
            model.Reviews = _context.Reviews.Where(o => o.BookId == book.Id).ToList();

            foreach(var review in model.Reviews)
            {
                review.User = await _userManager.FindByIdAsync(review.UserId);
                review.Book = book;
            }

            if(!(userLogged.Id == book.AuthorId))
            {
                book.Views += 1;
                _context.Books.Update(book);
                _context.SaveChanges();
            }

            ViewBag.UserId = userLogged.Id;
            ViewBag.BookId = book.Id;

            return View(model);
        }

        [Authorize(Roles ="مشرف")]
        public IActionResult Private()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChatPartial()
        {

            var messages = _context.Messages.ToList();

            return PartialView("_Chat", messages);
        }
        
        public async Task<JsonResult> ChatMessage([FromBody] MessageViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);

            var message = new Message()
            {
                Content = model.Content,
                Date = DateTime.Now,
                UserName = userLogged.Name
            };

            _context.Messages.Add(message);
            _context.SaveChanges();


            return Json(new {message = message});
        }

    }
}