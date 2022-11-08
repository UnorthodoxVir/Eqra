using Eqra.Models;
using static Eqra.Models.Enum;

namespace Eqra.ViewModels
{
    public class BookSearchViewModel
    {
        public string Name { get; set; }
        public BookLanuage Lanuage { get; set; }
        public int Year { get; set; }
        public Genre Genre { get; set; }
        public int Views { get; set; }
        public int Pages { get; set; }
        public int Rating { get; set; }
        public int Visiting { get; set; } = 1;
        public List<Book> Books { get; set; }

    }
}
