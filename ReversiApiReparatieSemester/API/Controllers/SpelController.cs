using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        [HttpGet("/api/Afgelopen/{id}")]
        public ActionResult<bool> Afgelopen(string id)
        {
            Spel spel = iRepository.GetSpel(id);
            //Asume that game is finished if null
            if (spel == null) return Ok(true);

            //If Geen kleur return false
            if (spel.AandeBeurt == Kleur.Geen) return Ok(false);

            bool notAbleToMakeMove = spel.Afgelopen();

            //If white unable to make move give turn to black
            if (spel.AandeBeurt == Kleur.Wit)
            {
                if (notAbleToMakeMove)
                {
                    spel.AandeBeurt = Kleur.Zwart;
                    iRepository.Save();
                }
                return Ok(false);
            }

            //Check if black's turn if unable to play 
            if (spel.AandeBeurt == Kleur.Zwart)
            {
                if (notAbleToMakeMove) return Ok(true);
            }

            return Ok(false);
        }

        [HttpGet("/api/SpelSpeler/{id}")]
        public ActionResult<IEnumerable<Spel>> GetSpelVanSpeler(string id)
        {
            return Ok(
                iRepository.GetSpellen().Where(spel => spel.Speler1Token == id || spel.Speler2Token == id)
            );
        }

        [HttpGet("/api/spellen")]
        public ActionResult<IEnumerable<Spel>> GetSpellen()
        {
            return Ok(
                iRepository.GetSpellen()
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

        [HttpPut("/api/spel/{id}/join")]
        public ActionResult<Spel> JoinSpel(string id, [FromQuery] string token)
        {
            if (token == null) return Unauthorized();

            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();

            //if (spel.Status != Status.Wachtend) return BadRequest("Dit spel is al bezig");
            if (spel.Speler2Token != null) return BadRequest("Geen ruimte in dit spel");
            spel.Speler2Token = token;
            spel.AandeBeurt = Kleur.Zwart;

            iRepository.Save();

            return Ok(spel);
        }

        [HttpPut("/api/spel/{id}/zet")]
        public ActionResult<Spel> DoeEenZet(string id, [FromQuery] string token, [FromQuery] int rij, [FromQuery] int kolom)
        {
            if (token == null) return Unauthorized();

            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();
            //if (spel.Status != Status.Bezig) return BadRequest("Dit potje is niet bezig");

            if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != token) return Unauthorized("Niet jou beurt");
            if (!spel.DoeZet(rij, kolom)) return BadRequest("Geen valide zet");

            iRepository.Save();

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
