using Apparat.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using WinObserver.Model;

namespace Apparat.Services.Interfaces
{
    public interface ITracertService
    {
        void StartStreamTracerouteHost(string hostname, IHostViewModel error);
        ReadOnlyObservableCollection<TracertModel> GetActualCollectionTracertValue();
        void StopStreamTracerouteHost();
    }
}