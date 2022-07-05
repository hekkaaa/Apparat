using Data.Entities;
using System.Collections.Generic;

namespace Apparat.Services.Interfaces
{
    public interface IAppSettingService
    {
        int AddHostInHistory(string newhost);
        List<string> GetLastFiveHistoryHost();
        bool ClearAllCollectionHistoryHost();
    }
}