using Data.Entities;
using System.Collections.Generic;

namespace Apparat.Services.Interfaces
{
    public interface IAppSettingService
    {
        void AddHostInHistory(string newhost);
        List<string> GetLastFiveHistoryHost();
    }
}