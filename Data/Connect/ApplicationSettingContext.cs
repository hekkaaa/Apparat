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

        public ApplicationSettingContext(DbContextOptions<ApplicationSettingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(
                     @"Data Source=Files\database\SettingDb.db");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }
    }
}
