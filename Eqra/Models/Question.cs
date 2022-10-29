using System.ComponentModel.DataAnnotations.Schema;
using static Eqra.Models.Enum;

namespace Eqra.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }

        public string CorrectAnswer { get; set; }

        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
