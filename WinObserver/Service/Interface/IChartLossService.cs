using WinObserver.Model;

namespace Apparat.Service.Interface
{
    public interface IChartLossService
    {
        void AddHostname(string host);
        void UpdateLoss(TracertModel newValue);
    }
}