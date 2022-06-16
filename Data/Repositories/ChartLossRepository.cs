using Data.Entities;
using Data.Repositories.Connect;
using Microsoft.EntityFrameworkCore;
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
        private object locker = new();  // объект-заглушка

        public ChartLossRepository()
        {
            _context = new ApplicationContext();
           
        }

        //public ChartLossRepository(ApplicationContext context)
        //{
        //    _context = context;
        //}

        public int AddHostname(Loss newHost)
        {
            lock (locker)
            {
                _context.Losses.Add(newHost);
                _context.SaveChanges();
                return newHost.Id;
            }
        }

        public void UpdateLoss(Loss newValue)
        {
            lock (locker)
            {
                _context.Losses.Update(newValue);
                _context.SaveChanges();
            }
        }

        public Loss GetHostById(int id)
        {
            lock (locker)
            {
                return _context.Losses.FirstOrDefault(x => x.Id == id);
            }
        }

        public Task<List<Loss>> GetAllHostInfo()
        {   
            using(var connectT = new ClearDb())
            {
                return connectT.Losses.ToListAsync();
            }
        }
    }
}
