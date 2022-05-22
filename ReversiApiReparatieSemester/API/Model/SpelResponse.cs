using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Model
{
    public class SpelResponse
    {
        public Spel Spel { get; set; }
        public bool Afgelopen { get; set; }
        public SpelResponse(Spel spel, bool afgelopen)
        {
            Spel = spel;
            Afgelopen = afgelopen;
        }
    }
}
