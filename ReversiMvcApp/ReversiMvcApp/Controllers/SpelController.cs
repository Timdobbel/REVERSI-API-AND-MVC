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

        // GET: Spel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        ////TODO 
        ////Zie level 7-4 controleer ook of speler al een een spel verbonden is.
        //public IActionResult Join(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    //return RedirectToAction(nameof(Play), new { id = spel.token });
        //    return RedirectToAction(nameof(Index));
        //}

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
        public async Task<IActionResult> Delete(string id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var res = _service.Delete(id, currentUserId);

            if (res)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }


        public IActionResult Speel(string id)
        {
            if (id == null) return NotFound();

            Spel spel = _service.GetSpel(id);

            if (spel == null) return NotFound();

            return View(spel);
        }



        private bool SpelExists(int id)
        {
            return _context.Spel.Any(e => e.ID == id);
        }
    }
}
