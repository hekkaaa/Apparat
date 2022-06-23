using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Connect
{
    public class DublicateContext : DbContext
    {
        public DbSet<Loss> Losses { get; set; }
        public DbSet<RequestTime> RequestsTimes { get; set; }

        public DublicateContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=DataBaseChart.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
