using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Model;

namespace API.Controllers
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


        // GET api/spel
        [HttpGet]
        public ActionResult<IEnumerable<Spel>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            return Ok(
                iRepository.GetSpellen().Where(i => i.Speler2Token == null)
            );
        }

        // GET api/spel/{token}
        [HttpGet]
        [Route("{Token}")]
        public IActionResult Spel()
        {
            Spel spel = iRepository.GetSpel(RouteData.Values["Token"].ToString());
            if (spel != null)
                return Ok(spel);
            else
                return NotFound();
        }

        [HttpGet("/api/SpelSpeler/{id}")]
        public ActionResult<IEnumerable<Spel>> GetSpelVanSpeler(string id)
        {
            return Ok(
                iRepository.GetSpellen().Where(spel => spel.Speler1Token == id || spel.Speler2Token == id)
            );
        }

        // GET api/spel/beurt/{token}   
        [HttpGet]
        [Route("beurt/{token}")]
        public ActionResult<Kleur> Beurt(string token)
        {
            Spel spel = iRepository.GetSpel(token);
            if (spel != null)
                return Ok(spel.AandeBeurt);
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<Spel> CreateSpel([FromForm] string spelerToken, [FromForm] string omschrijving)
        {
            Spel spel = new Spel();
            spel.Token = Guid.NewGuid().ToString();
            spel.Speler1Token = spelerToken;
            spel.Omschrijving = omschrijving;
            iRepository.AddSpel(spel);
            return Ok(spel);
        }


        [HttpDelete("/api/spel/{id}")]
        public ActionResult<Spel> DeleteSpel(string id, [FromQuery] string token)
        {
            if (token == null) return Unauthorized("Dit is niet jouw spel.");
            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();

            //if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != token) return Unauthorized("Spel kan alleen verwijderd worden wanneer jij aan de beurt bent.");

            iRepository.Delete(spel);
            iRepository.Save();

            return Ok(spel);
        }

    }
}
