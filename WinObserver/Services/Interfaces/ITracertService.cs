using Apparat.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;

namespace Apparat.Services.Interfaces
{
    public interface ITracertService
    {
        event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string prop = "");
        void StartTraceroute(string hostname, IHostViewModel error);
        ReadOnlyObservableCollection<TracertModel> GetActualCollectionTracertValue();
        void StopTraceroute();
    }
}