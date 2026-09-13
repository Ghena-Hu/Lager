using Lager.Models;
using Microsoft.EntityFrameworkCore;

namespace Lager.Data
{
    public class LagerContext : DbContext
    {
        public LagerContext(DbContextOptions<LagerContext> options)
            : base(options)
        {
        }

        public DbSet<Artikel> Artikel { get; set; }

        public DbSet<Buchung> Buchungen { get; set; }
    }
}