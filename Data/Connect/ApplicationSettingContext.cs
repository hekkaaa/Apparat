using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Connect
{
    public class ApplicationSettingContext : DbContext
    {
        public DbSet<HistoryHost> History { get; set; }

        public ApplicationSettingContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=SettingDb.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
