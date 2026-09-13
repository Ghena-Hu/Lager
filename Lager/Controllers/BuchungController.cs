using Lager.Data;
using Lager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lager.Controllers
{
    public class BuchungController : Controller
    {
        private readonly LagerContext _context;

        public BuchungController(LagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var buchungen = await _context.Buchungen
                .Include(b => b.Artikel)
                .OrderByDescending(b => b.Datum)
                .ToListAsync();

            return View(buchungen);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buchung = await _context.Buchungen
                .Include(b => b.Artikel)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (buchung == null)
            {
                return NotFound();
            }

            return View(buchung);
        }

        public async Task<IActionResult> Create()
        {
            await ArtikelListeErstellen();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ArtikelId,Datum,Menge,Preis")] Buchung buchung)
        {
            if (buchung.Menge == 0)
            {
                ModelState.AddModelError(
                    "Menge",
                    "Die Menge darf nicht 0 sein.");
            }

            if (buchung.Preis < 0)
            {
                ModelState.AddModelError(
                    "Preis",
                    "Der Preis darf nicht negativ sein.");
            }

            bool artikelExistiert = await _context.Artikel
                .AnyAsync(a => a.Id == buchung.ArtikelId);

            if (!artikelExistiert)
            {
                ModelState.AddModelError(
                    "ArtikelId",
                    "Der ausgewählte Artikel existiert nicht.");
            }

            if (ModelState.IsValid)
            {
                _context.Buchungen.Add(buchung);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await ArtikelListeErstellen(buchung.ArtikelId);

            return View(buchung);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buchung = await _context.Buchungen.FindAsync(id);

            if (buchung == null)
            {
                return NotFound();
            }

            await ArtikelListeErstellen(buchung.ArtikelId);

            return View(buchung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ArtikelId,Datum,Menge,Preis")] Buchung buchung)
        {
            if (id != buchung.Id)
            {
                return NotFound();
            }

            if (buchung.Menge == 0)
            {
                ModelState.AddModelError(
                    "Menge",
                    "Die Menge darf nicht 0 sein.");
            }

            if (buchung.Preis < 0)
            {
                ModelState.AddModelError(
                    "Preis",
                    "Der Preis darf nicht negativ sein.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buchung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuchungExists(buchung.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await ArtikelListeErstellen(buchung.ArtikelId);

            return View(buchung);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buchung = await _context.Buchungen
                .Include(b => b.Artikel)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (buchung == null)
            {
                return NotFound();
            }

            return View(buchung);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buchung = await _context.Buchungen.FindAsync(id);

            if (buchung != null)
            {
                _context.Buchungen.Remove(buchung);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task ArtikelListeErstellen(
            int? ausgewahlterArtikel = null)
        {
            var artikel = await _context.Artikel
                .OrderBy(a => a.Artikelnummer)
                .ToListAsync();

            ViewBag.ArtikelListe = new SelectList(
                artikel,
                "Id",
                "Artikelname",
                ausgewahlterArtikel);
        }

        private bool BuchungExists(int id)
        {
            return _context.Buchungen.Any(b => b.Id == id);
        }
    }
}