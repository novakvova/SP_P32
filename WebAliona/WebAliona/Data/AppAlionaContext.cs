using Microsoft.EntityFrameworkCore;

namespace WebAliona.Data
{
    public class AppAlionaContext : DbContext
    {
        public AppAlionaContext(DbContextOptions<AppAlionaContext> options)
            : base(options) { }

        public DbSet<Banan> Banans { get; set; }
    }
}
