using Apparat.Services.Interfaces;
using Data.Connect;
using Data.Entities;
using Data.Repositories;
using Data.Repositories.Interfaces;
using System.Collections.Generic;

namespace Apparat.Services
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IAppSettingRepository _appSettingRepository;

        public AppSettingService()
        {
            _appSettingRepository = new AppSettingRepository(new ApplicationSettingContext());
        }

        public void AddHostInHistory(string newhost)
        {
            HistoryHost newItem = new HistoryHost() { Hostname = newhost };
            _appSettingRepository.AddNewHost(newItem);
        }

        public List<string> GetLastFiveHistoryHost()
        {
            List<HistoryHost> res = _appSettingRepository.GetLastFiveHostname();

            List<string> resultCollection = new List<string>();

            foreach (HistoryHost host in res)
            {
                resultCollection.Add(host.Hostname);
            }

            return resultCollection;
        }
    }
}
