using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Eqra.Models.Enum;

namespace Eqra.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public int Pages { get; set; }
        public int Views { get; set; }
        public DateTime ReleaseDate { get; set; }
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public BookLanuage BookLanuage { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public double Rating { get; set; } = 5.0;
    }
}
