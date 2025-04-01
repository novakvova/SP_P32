using Microsoft.EntityFrameworkCore;

namespace ThreadAndDatabase
{
    public class ThreadAppContext : DbContext
    {
        public ThreadAppContext()
        {

        }
        public ThreadAppContext(DbContextOptions<ThreadAppContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-nameless-bread-a2d62hce-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_PNmMi04SyJRb");
        }

        public DbSet<Banan> Banans { get; set; }
    }
}
