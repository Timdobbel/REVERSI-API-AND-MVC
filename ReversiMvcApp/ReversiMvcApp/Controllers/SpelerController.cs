using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp
{
    [Authorize(Roles = "Beheerder,Mediator")]
    public class SpelerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReversiDbContext _context;
        private readonly APIService _apiService;
        private readonly ApplicationDbContext _dbContext;

        public SpelerController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, APIService APIService, ReversiDbContext context, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _apiService = APIService;
            _context = context;
            _dbContext = dbContext;
        }

        // GET: Speler
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Beheerder"))
            {
                return View("IndexBeheerder", await _context.Spelers.ToListAsync());
            }
            return View("IndexMediator", await _context.Spelers.ToListAsync());

        }

        // GET: Speler/Edit/5
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null)
            {
                return NotFound();
            }
            return View(speler);
        }

        // POST: Speler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Role")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Get and update identity 
                    var user = await _userManager.FindByIdAsync(id);
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    await _userManager.AddToRoleAsync(user, speler.Role);

                    //Get ReversiDB speler and change role
                    Speler updateSpeler = _context.Spelers.FirstOrDefault(s => s.Guid == id);
                    updateSpeler.Role = speler.Role;

                    _context.Update(updateSpeler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelerExists(speler.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        //GET: Speler/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        //POST: Speler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null) return NotFound();

            //Delete reversiDB player
            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null) return NotFound();
            _context.Spelers.Remove(speler);
            await _context.SaveChangesAsync();

            var spellen = _apiService.GetAll();
            Spel bestaandSpel = spellen.FirstOrDefault(spel => spel.Speler1Token == id || spel.Speler2Token == id);
            if (bestaandSpel != null)
            {
                _apiService.Delete(bestaandSpel.Token, id);
            }

            //Delete identity user
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        private bool SpelerExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }
    }
}
