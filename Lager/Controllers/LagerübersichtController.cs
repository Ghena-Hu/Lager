using Lager.Data;
using Lager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lager.Controllers
{
    public class LagerübersichtController : Controller
    {
        private readonly LagerContext _context;

        public LagerübersichtController(LagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artikel = await _context.Artikel
                .Include(a => a.Buchungen)
                .OrderBy(a => a.Artikelnummer)
                .ToListAsync();

            var lagerübersicht = artikel
                .Select(a =>
                {
                    var bestand = a.Buchungen.Sum(b => b.Menge);

                    var eingänge = a.Buchungen
                        .Where(b => b.Menge > 0)
                        .ToList();

                    var eingangsmenge = eingänge.Sum(b => b.Menge);

                    var eingangswert = eingänge
                        .Sum(b => b.Menge * b.Preis);

                    decimal durchschnittspreis = 0;

                    if (eingangsmenge > 0)
                    {
                        durchschnittspreis =
                            eingangswert / eingangsmenge;
                    }

                    return new LagerübersichtViewModel
                    {
                        Artikelnummer = a.Artikelnummer,
                        Artikelname = a.Artikelname,
                        Bestand = bestand,
                        Durchschnittspreis = durchschnittspreis,
                        Gesamtwert = bestand * durchschnittspreis
                    };
                })
                .ToList();

            return View(lagerübersicht);
        }
    }
}