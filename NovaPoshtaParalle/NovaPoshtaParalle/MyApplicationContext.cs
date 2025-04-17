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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bober.girl");
        }
    }
}
