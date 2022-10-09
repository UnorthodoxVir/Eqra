using Microsoft.AspNetCore.Identity;
using static Eqra.Models.Enum;

namespace Eqra.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
