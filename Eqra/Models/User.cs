using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using static Eqra.Models.Enum;

namespace Eqra.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public int Points { get; set; } = 0;
    }
}
