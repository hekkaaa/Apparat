using Data.Connect;
using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AppSettingRepository : IAppSettingRepository
    {
        private ApplicationSettingContext _context;

        public AppSettingRepository(ApplicationSettingContext context)
        {
            _context = context;
        }

        public int AddNewHost(HistoryHost newHostname)
        {
            _context.History.Add(newHostname);
            _context.SaveChanges();
            return newHostname.Id;
        }

        public List<HistoryHost> GetLastFiveHostname()
        {
            return _context.History.OrderByDescending(x => x).Take(5).ToList();
        }

        public bool ClearAllTable()
        {
            _context.History.RemoveRange(_context.History);
            _context.SaveChanges();
            return true;
        }
    }
}
