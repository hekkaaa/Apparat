using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;
using WinObserver.Service;

namespace Apparat.ViewModel.Interfaces
{
    public interface IHostViewModel
    {
        string ControlBtnName { get; set; }
        string? HostnameView { get; set; }
        DelegateCommand StartCommand { get; }
        DelegateCommand StopCommand { get; }
        ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}