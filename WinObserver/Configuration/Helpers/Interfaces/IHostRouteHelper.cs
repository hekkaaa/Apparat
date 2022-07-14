using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;

namespace Apparat.Helpers.Interfaces
{
    public interface IHostRouteHelper
    {
        event PropertyChangedEventHandler? PropertyChanged;

        IEnumerable<string> CreateNewRouteCollection(string hostname);
        void FillingNewRoute(ref ObservableCollection<TracertModel> hostnameCollection, IEnumerable<string> collectionIpAddres);
        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}