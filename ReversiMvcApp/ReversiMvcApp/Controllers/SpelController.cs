using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp
{
    public class SpelController : Controller
    {
        private readonly APIService _service;
        private readonly ReversiDbContext _context;

        public SpelController(ReversiDbContext context, APIService apiService)
        {
            _service = apiService;
            _context = context;
        }

        // GET: Spel
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(_service.GetAll());
        }

        // GET: Leaderboard
        [Authorize]
        public async Task<IActionResult> Leaderboard()
        {
            return View(await _context.Spelers.ToListAsync());
        }

        // GET: Spel/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Omschrijving,Token,aanDeBeurt,Speler1Token,Speler2Token")] Spel spel)
        {
            Spel resObject = null;
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ModelState.IsValid)
            {
                resObject = _service.MaakSpel(currentUserId, spel.Omschrijving);
            }

            return RedirectToAction(nameof(Speel), new { id = resObject.Token });
        }

        public IActionResult Join(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Spel spel = _service.Deelnemen(id, currentUserId);

            return RedirectToAction(nameof(Speel), new { id = spel.Token });
        }


        // GET: Spel/Delete/5
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Delete(string id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var res = _service.Delete(id, currentUserId);

            if (res)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

        [Authorize]
        public IActionResult Speel(string id)
        {
            if (id == null) return NotFound();

            Spel spel = _service.GetSpel(id);

            if (spel == null) return RedirectToAction("Index", "Home");

            return View(spel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpel(string id)
        {
            if (id == null) return NotFound();
            return Ok(_service.GetSpel(id));
        }


        [HttpPut("/api/spel/{id}/zet")]
        public async Task<IActionResult> DoeZet(string id, [FromQuery] string token, [FromQuery] int rij, [FromQuery] int kolom)
        {
            var spel = _service.DoeZet(id, token, kolom, rij);

            return Ok(spel);
        }

        [HttpGet]
        public async Task<IActionResult> Afgelopen(string id)
        {
            bool res = _service.CheckIfAfgelopen(id);
            return Ok(res);
        }


        [HttpGet]
        public async Task<IActionResult> Finish(string id)
        {
            if (id == null) return NotFound();

            Spel spel = _service.GetSpel(id);

            if (spel == null) return NotFound();


            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Speler speler1 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler1Token);
            if (speler1 == null) return NotFound();

            Speler speler2 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler2Token);
            if (speler2 == null) return NotFound();

            if (currentUserId != spel.beurt) return Unauthorized();

            int scoreP1 = spel.bord.Count(v => v.Value == 1);
            int scoreP2 = spel.bord.Count(v => v.Value == 2);

            if (scoreP1 > scoreP2)
            {
                speler1.AantalGewonnen++;
                speler2.AantalVerloren++;
            }
            else if (scoreP2 > scoreP1)
            {
                speler2.AantalGewonnen++;
                speler1.AantalVerloren++;
            }
            else
            {
                speler1.AantalGelijk++;
                speler2.AantalGelijk++;
            }

            await _context.SaveChangesAsync();

            if (!_service.Delete(id, currentUserId)) return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Verlaten(string id, [FromQuery] string spelerToken)
        {
            if (id == null || spelerToken == null) return NotFound();

            Spel spel = _service.GetSpel(id);

            if (spel == null) return NotFound();

            Speler speler1 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler1Token);
            Speler speler2 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler2Token);

            //No second player. Delete game without updating score.
            if (speler2 == null)
            {
                if (!_service.Delete(id, spelerToken)) return BadRequest();
                return Ok();
            }

            if (spelerToken == speler1.Guid)
            {
                speler1.AantalVerloren++;
                speler2.AantalGewonnen++;
            }
            else if (spelerToken == speler2.Guid)
            {
                speler1.AantalGewonnen++;
                speler2.AantalVerloren++;
            }

            await _context.SaveChangesAsync();

            if (!_service.Delete(id, spelerToken)) return BadRequest();

            return Ok();
        }

        private bool SpelExists(int id)
        {
            return _context.Spel.Any(e => e.ID == id);
        }
    }
}
