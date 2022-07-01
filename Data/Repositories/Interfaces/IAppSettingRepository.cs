using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IAppSettingRepository
    {
        int AddNewHost(HistoryHost newHostname);
        List<HistoryHost> GetLastFiveHostname();
    }
}