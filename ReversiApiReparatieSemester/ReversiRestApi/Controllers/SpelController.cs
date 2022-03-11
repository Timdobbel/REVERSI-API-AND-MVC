using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReversiRestApi.Model;

namespace ReversiRestApi.Controllers
{
    [Route("api/Spel")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository iRepository;

        public SpelController(ISpelRepository repository)
        {
            iRepository = repository;
        }


        //// GET api/spel
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        //{
        //    return iRepository.GetSpellen()
        //        .Where(i => i.Speler2Token == null)
        //        .Select(i => i.Omschrijving)
        //        .ToArray();
        //}

        [HttpGet]
        public ActionResult<string> test()
        {
            return "kut reversi";
        }

        // ...

    }

}
