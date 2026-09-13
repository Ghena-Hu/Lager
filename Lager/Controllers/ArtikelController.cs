using Lager.Data;
using Lager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lager.Controllers
{
    public class ArtikelController : Controller
    {
        private readonly LagerContext _context;

        public ArtikelController(LagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artikel = await _context.Artikel
                .ToListAsync();

            return View(artikel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artikel = await _context.Artikel
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artikel == null)
            {
                return NotFound();
            }

            return View(artikel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Artikelnummer,Artikelname,Beschreibung")] Artikel artikel)
        {
            bool artikelnummerVorhanden = await _context.Artikel
                .AnyAsync(a => a.Artikelnummer == artikel.Artikelnummer);

            if (artikelnummerVorhanden)
            {
                ModelState.AddModelError(
                    "Artikelnummer",
                    "Diese Artikelnummer existiert bereits.");
            }

            if (ModelState.IsValid)
            {
                _context.Artikel.Add(artikel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(artikel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artikel = await _context.Artikel.FindAsync(id);

            if (artikel == null)
            {
                return NotFound();
            }

            return View(artikel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Artikelnummer,Artikelname,Beschreibung")] Artikel artikel)
        {
            if (id != artikel.Id)
            {
                return NotFound();
            }

            bool artikelnummerVorhanden = await _context.Artikel
                .AnyAsync(a =>
                    a.Artikelnummer == artikel.Artikelnummer &&
                    a.Id != artikel.Id);

            if (artikelnummerVorhanden)
            {
                ModelState.AddModelError(
                    "Artikelnummer",
                    "Diese Artikelnummer existiert bereits.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artikel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtikelExists(artikel.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(artikel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artikel = await _context.Artikel
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artikel == null)
            {
                return NotFound();
            }

            return View(artikel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artikel = await _context.Artikel.FindAsync(id);

            if (artikel != null)
            {
                _context.Artikel.Remove(artikel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ArtikelExists(int id)
        {
            return _context.Artikel.Any(a => a.Id == id);
        }
    }
}