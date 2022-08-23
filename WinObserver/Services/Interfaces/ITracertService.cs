using Apparat.Configuration.Events;
using Apparat.ViewModel.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinObserver.Model;

namespace Apparat.Services.Interfaces
{
    public interface ITracertService
    {
        void StartStreamTracerouteHost(string hostname, IHostViewModelEvents error);
        ReadOnlyObservableCollection<TracertModel> GetActualCollectionTracertValue();
        void StopStreamTracerouteHost();
        List<string> GetArhiveTimeRequestCollection();
        //void StartStreamTracerouteHost(string v, IHostViewModelEvents hostViewModelEvents);
    }
}