using Data.Entities;
using Data.Repositories.Connect;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RequestTimeRepository : IRequestTimeRepository
    {
        private ApplicationContext _context;

        public RequestTimeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddTime(RequestTime newDate)
        {
            _context.RequestsTimes.Add(newDate);
            _context.SaveChanges();
        }

        public List<string> GetAllTime()
        {
            return _context.RequestsTimes.Select(x => x.ListTime).ToList();
        }

        public void ClearTable()
        {
            var tmp = _context.RequestsTimes.ToList();
            if (tmp.Count != 0)
            {
                _context.RequestsTimes.RemoveRange(tmp);
                _context.SaveChanges();
            }
        }
    }
}
