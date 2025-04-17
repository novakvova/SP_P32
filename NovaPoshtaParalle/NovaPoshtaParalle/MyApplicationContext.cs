using Microsoft.EntityFrameworkCore;
using NovaPoshtaParalle.Entities;

namespace NovaPoshtaParalle
{
    public class MyApplicationContext : DbContext
    {
        /// <summary>
        /// У БД буде табличка tbl_categories
        /// </summary>
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=readDataNovaPoshra;Username=postgres;Password=123456");
        }
    }
}
