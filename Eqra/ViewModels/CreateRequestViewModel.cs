using static Eqra.Models.Enum;

namespace Eqra.ViewModels
{
    public class CreateRequestViewModel
    {
        public string Name { get; set; }
        public int Genre { get; set; }
        public int NumberOfBooks { get; set; }
        public string Bio { get; set; }
    }
}
