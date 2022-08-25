using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Connect
{
    public class ApplicationSettingFolderandHostContext : DbContext
    {
        public DbSet<FolderState> Folders { get; set; }
        public DbSet<StateObjectTraceroute> TracerouteHost { get; set; }

        public ApplicationSettingFolderandHostContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationSettingFolderandHostContext(DbContextOptions<ApplicationSettingFolderandHostContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(
                     @"Data Source=Files\database\SettingStateFoldersDb.db");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }
    }
}
