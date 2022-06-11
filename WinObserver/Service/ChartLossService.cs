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
        private readonly RequestTimeRepository _requestTimeRepository;
        private readonly ChartLossRepository _chartLossRepository;
        private readonly ApplicationContext _applicationContext;

        public ChartLossService(ApplicationContext context)
        {
            _applicationContext = context;
            _chartLossRepository = new ChartLossRepository(_applicationContext);
            _requestTimeRepository = new RequestTimeRepository(_applicationContext);
        }

        public void AddHostname(string host)
        {
            Loss tmpItem = new Loss() { Hostname = host};
            _chartLossRepository.AddHostname(tmpItem);
        }

        public void UpdateLoss(TracertModel newValue)
        {
            var modeltest = _chartLossRepository.GetHostById(newValue.NumberHostname);
            modeltest.ListLoss += newValue.PercentLossPacket.ToString() + ",";
            _chartLossRepository.UpdateLoss(modeltest);
        }

        public void AddTimeXAxes()
        {
            DateTime date = DateTime.Now;
            RequestTime tmpDate = new RequestTime() { ListTime = date.ToString("T") };
            _requestTimeRepository.AddTime(tmpDate);
        }

        public List<string> GetAllTimeXAxes()
        {
            return new List<string>();
        }
    }
}
