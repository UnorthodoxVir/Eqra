using System.ComponentModel.DataAnnotations.Schema;

namespace Eqra.Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Enum.Genre Genre { get; set; }
        public int NumberOfBooks { get; set; }
        public string Bio { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
