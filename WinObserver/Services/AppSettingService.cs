using Apparat.Services.Interfaces;
using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            ObservableCollection<string> checkCollection = GetLastFiveHistoryHost();
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
                List<HistoryHost> tmpCollection = _appSettingRepository.GetLastFiveHostname();

                if(tmpCollection.Count >= 5)
                {
                    _appSettingRepository.DeleteHostname(tmpCollection.LastOrDefault()!);
                }

                HistoryHost newItem = new HistoryHost() { Hostname = newhost };
                return _appSettingRepository.AddNewHost(newItem);
            }

            return 0;
        }

        public ObservableCollection<string> GetLastFiveHistoryHost()
        {
            List<HistoryHost> res = _appSettingRepository.GetLastFiveHostname();

            ObservableCollection<string> resultCollection = new ObservableCollection<string>();

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

        public bool DeleteOneHostnameFromHistoryCollection(string deleteHostname)
        {
            List<HistoryHost> tmpCollection = _appSettingRepository.GetLastFiveHostname();
            try
            {
                HistoryHost foundObj = tmpCollection.FirstOrDefault(x => x.Hostname == deleteHostname)!;
                bool removeResult = _appSettingRepository.DeleteHostname(foundObj);
                if (removeResult)
                {
                    return true;
                }
                else
                {
                    // Error when saving data after deletion. Add a log.
                    return false;
                }
            }
            catch(ArgumentNullException)
            {   // There will be a log for an error when suddenly delete Host name will be null.
                return false;
            }
            catch (Exception)
            {
                // Unexpected error. Add a log.
                return false;
            }
        }
    }
}
