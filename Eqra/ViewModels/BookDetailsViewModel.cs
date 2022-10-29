using Eqra.Models;

namespace Eqra.ViewModels
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Book> SuggestedBooks { get; set; }
    }
}
