using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Spel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public int aanDeBeurt { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public bool SpelerVerlaten { get; set; }


        [NotMapped]
        public Dictionary<string, int> bord { get; set; }


        public string beurt
        {
            get => aanDeBeurt == 1 ? Speler1Token : Speler2Token;
        }
    }
}
