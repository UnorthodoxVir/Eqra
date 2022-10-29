namespace Eqra.Models
{
    public class ExamLockout
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
