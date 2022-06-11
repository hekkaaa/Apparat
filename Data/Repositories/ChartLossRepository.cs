using Data.Entities;
using Data.Repositories.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ChartLossRepository
    {
        private ApplicationContext _context;

        public ChartLossRepository(ApplicationContext context)
        {
            _context = context;
        }

        public int AddHostname(Loss newHost)
        {
            _context.Losses.Add(newHost);
            _context.SaveChanges();
            return newHost.Id;
        }

        public void UpdateLoss(Loss newValue)
        {
            _context.Losses.Update(newValue);
            _context.SaveChanges();
        }
    }
}
