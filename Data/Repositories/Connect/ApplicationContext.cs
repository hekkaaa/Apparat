using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Connect
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Loss> Loss { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=data\\testbaseChart.db");
            optionsBuilder.UseLazyLoadingProxies();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        }
    }
}
