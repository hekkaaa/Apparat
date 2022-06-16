using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Connect
{
    public class ClearDb : DbContext
    {
        public DbSet<Loss> Losses { get; set; }
        public DbSet<RequestTime> RequestsTimes { get; set; }

        public ClearDb()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
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
