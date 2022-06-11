using Data.Entities;
using Data.Repositories.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RequestTimeRepository
    {
        private ApplicationContext _context;

        public RequestTimeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddTime(RequestTime newDate)
        {
            _context.RequestsTimes.Add(newDate);
        }

        public List<string> GetAllTime()
        {
            return _context.RequestsTimes.Select(x => x.ListTime).ToList();
        }
    }
}
