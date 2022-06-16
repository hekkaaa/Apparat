using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Connect
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Loss> Losses { get; set; }
        public DbSet<RequestTime> RequestsTimes { get; set; }

        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=testbaseChart.db");
            optionsBuilder.UseLazyLoadingProxies();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        }
    }
}
