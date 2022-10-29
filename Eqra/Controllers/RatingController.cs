using Eqra.Data;
using Eqra.Models;
using Eqra.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eqra.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public RatingController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<JsonResult> RateBook([FromBody] RatingViewModel model)
        {
            var userLogged = await _userManager.GetUserAsync(User);
            var book = _context.Books.Where(o => o.Id == model.BookId).FirstOrDefault();

            if (book.AuthorId == userLogged.Id)
            {
                return Json(new { correct = false, IsAuthor = true, AlreadyRated = false });
            }
            else if(_context.Ratings.Where(o=>o.UserId == userLogged.Id && o.BookId == book.Id).FirstOrDefault() != null)
            {
                return Json(new { correct = false, IsAuthor = false, AlreadyRated = true });
            }


            _context.Ratings.Add(new Rating()
            {
                BookId = book.Id,
                Value = model.Rating,
                UserId = userLogged.Id
            });
            _context.SaveChanges();

            var OverallRating = _context.Ratings.Where(o => o.BookId == book.Id).ToList().Sum(o=>o.Value) / _context.Ratings.Where(o => o.BookId == book.Id).ToList().Count;

            book.Rating = OverallRating;
            _context.Books.Update(book);
            _context.SaveChanges();

            return Json(new { correct = true, overallRating = OverallRating });


        }

    }
}
