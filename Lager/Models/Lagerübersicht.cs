namespace Lager.Models
{
    public class LagerübersichtViewModel
    {
        public string Artikelnummer { get; set; } = "";

        public string Artikelname { get; set; } = "";

        public int Bestand { get; set; }

        public decimal Gesamtwert { get; set; }

        public decimal Durchschnittspreis { get; set; }
    }
}
