using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReversiMvcApp.Models
{
    public class Speler
    {
        [Key]
        public string Guid { get; set; }
        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }
        public string Role { get; set; }

        [NotMapped]
        public List<SelectListItem> Roles { get; } = new List<SelectListItem>()
        {
            new SelectListItem { Value = "Player", Text = "Speler" },
            new SelectListItem { Value = "Beheerder", Text = "Beheerder" },
            new SelectListItem { Value = "Mediator", Text = "Mediator" },
        };
    }
}
