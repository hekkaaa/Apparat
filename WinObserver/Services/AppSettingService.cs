using Apparat.Services.Interfaces;
using Data.Entities;
using Data.Repositories.Interfaces;
using System.Collections.Generic;

namespace Apparat.Services
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IAppSettingRepository _appSettingRepository;

        public AppSettingService(IAppSettingRepository repository)
        {
            _appSettingRepository = repository;
        }

        public int AddHostInHistory(string newhost)
        {
            List<string> checkCollection = GetLastFiveHistoryHost();
            bool flag = false;

            foreach (string checkHostname in checkCollection)
            {
                if (checkHostname == newhost)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                HistoryHost newItem = new HistoryHost() { Hostname = newhost };
                return _appSettingRepository.AddNewHost(newItem);
            }

            return 0;
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

        public bool ClearAllCollectionHistoryHost()
        {
            _appSettingRepository.ClearAllTable();
            return true;
        }
    }
}
