using Microsoft.EntityFrameworkCore;

namespace LorincMarton_WebAPI.Modells
{
    public class PartnerSzerzodesContext:DbContext
    {
        public PartnerSzerzodesContext(DbContextOptions<PartnerSzerzodesContext> options) : base(options)
        {
        }
        public DbSet<Partner> Partnerek { get; set; }
        public DbSet<Szerzodes> Szerzodesek { get; set; }
    }
}
