using System.Collections.ObjectModel;

namespace Apparat.Services.Interfaces
{
    public interface IAppSettingService
    {
        int AddHostInHistory(string newhost);
        ObservableCollection<string> GetLastFiveHistoryHost();
        bool ClearAllCollectionHistoryHost();
        bool DeleteOneHostnameFromHistoryCollection(string deleteHostname);
    }
}