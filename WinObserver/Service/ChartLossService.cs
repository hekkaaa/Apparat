using Apparat.Service.Interface;
using Data.Entities;
using Data.Repositories;
using Data.Repositories.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinObserver.Model;

namespace Apparat.Service
{
    public class ChartLossService : IChartLossService
    {
        private readonly ChartLossRepository _chartLossRepository;
        private readonly ApplicationContext _applicationContext;

        public ChartLossService(ApplicationContext context)
        {
            _applicationContext = context;
            _chartLossRepository = new ChartLossRepository(_applicationContext);
        }

        public void AddHostname(string host)
        {
            Loss tmpItem = new Loss() { Hostname = host};
            _chartLossRepository.AddHostname(tmpItem);
        }

        public void UpdateLoss(TracertModel newValue)
        {
            var modeltest = newValue;
            var t = "s";
        }
    }
}
