﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ReversiMvcApp.Data;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIService _apiService;
        private ReversiDbContext _context;

        public HomeController(ILogger<HomeController> logger, APIService apiService, ReversiDbContext context)
        {
            _logger = logger;
            _apiService = apiService;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserEmail = currentUser.FindFirst(ClaimTypes.Email)?.Value;

            List<Spel> spellen = new();

            if (currentUserID != null)
            {
                Speler speler = _context.Spelers.FirstOrDefault(s => s.Guid == currentUserID);

                if (speler == null)
                {
                    speler = new Speler { Guid = currentUserID, Naam = "Onbekend" };
                    _context.Spelers.Add(speler);
                    _context.SaveChanges();
                }

                //Changed this so that it fetches open games
                //spellen = _apiService.GetSpellenDoorSpelerToken(currentUserID);
                //spellen = _apiService.GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler();

                spellen = _apiService.GetAll();
                Spel bestaandSpel = spellen.FirstOrDefault(spel => spel.Speler1Token == currentUserID || spel.Speler2Token == currentUserID);

                if (bestaandSpel != null) return RedirectToAction("Speel", "Spel", new { id = bestaandSpel.Token });
            }

            return View(_apiService.GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
