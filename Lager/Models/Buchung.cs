namespace Lager.Models
{
    public class Buchung
    {
        public int Id { get; set; }

        public int ArtikelId { get; set; }

        public Artikel? Artikel { get; set; }

        public DateTime Datum { get; set; }

        public int Menge { get; set; }

        public decimal Preis { get; set; }
    }
}