using System.Collections.Generic;
using WinObserver.Model;

namespace Apparat.Service.Interface
{
    public interface IChartLossService
    {
        void AddHostname(string host);
        void UpdateLoss(TracertModel newValue);
        void AddTimeXAxes();
        List<string> GetAllTimeXAxes();
    }
}