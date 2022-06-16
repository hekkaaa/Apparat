using System.Collections.Generic;
using WinObserver.Model;

namespace Apparat.Service.Interface
{
    public interface IChartLossService
    {
        void StartUpdateChart();
        void StopUpdateChart();
    }
}