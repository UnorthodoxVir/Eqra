using Eqra.Migrations;
using Eqra.Models;
using Microsoft.EntityFrameworkCore;
using static Eqra.Models.Enum;

namespace Eqra.ViewModels
{
    public class CreateBookViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public int Pages { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile CoverPath { get; set; }
        public IFormFile ContentPath { get; set; }

        public Question Q1 { get; set; }
        public Question Q2 { get; set; }
        public Question Q3 { get; set; }
        public Question Q4 { get; set; }
        public Question Q5 { get; set; }
    }
}
