using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

                spellen = _apiService.GetSpellenDoorSpelerToken(currentUserID);
            }

            return View(spellen);
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
