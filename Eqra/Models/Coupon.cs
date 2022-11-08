namespace Eqra.Models
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public int Cost { get; set; }
        public string Sponsor { get; set; }
        public string Info { get; set; }
        public string Code { get; set; }
        public DateTime EndingDate { get; set; }
        public bool Used { get; set; } = false;
    }
}
