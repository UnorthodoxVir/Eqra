using System.ComponentModel.DataAnnotations.Schema;

namespace Eqra.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
