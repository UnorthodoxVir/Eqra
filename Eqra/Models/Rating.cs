namespace Eqra.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public double Value { get; set; }
        public string UserId { get; set; }
    }
}
