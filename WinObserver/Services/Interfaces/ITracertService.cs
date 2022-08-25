using Apparat.Configuration.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinObserver.Model;

namespace Apparat.Services.Interfaces
{
    public interface ITracertService
    {
        void StartStreamTracerouteHost(string hostname, IHostViewModelEvents error, int delay);
        ReadOnlyObservableCollection<TracertModel> GetActualCollectionTracertValue();
        void StopStreamTracerouteHost();
        List<string> GetArhiveTimeRequestCollection();
        int GetDelayValue();
        void UpdateDelayValue(int newDelay);
        int GetSizePacketValue();
        void UpdateSizePacketValue(int newSize);
    }
}