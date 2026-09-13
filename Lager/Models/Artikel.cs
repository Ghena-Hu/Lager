using System.ComponentModel.DataAnnotations;

namespace Lager.Models
{
    public class Artikel
    {
        public int Id { get; set; }

        [Required]
        public string Artikelnummer { get; set; }

        [Required]
        public string Artikelname { get; set; }

        public string? Beschreibung { get; set; }

        public ICollection<Buchung> Buchungen { get; set; } = new List<Buchung>();
    }
}