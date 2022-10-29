using Eqra.Models;
using static Eqra.Models.Enum;

namespace Eqra.ViewModels
{
    public class HomeViewModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Genre? Genre { get; set; }
        public List<Book> MostViewed { get; set; }
        public List<Book> Books { get; set; }
    }
}
