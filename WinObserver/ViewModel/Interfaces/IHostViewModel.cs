using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;
using WinObserver.Service;

namespace Apparat.ViewModel.Interfaces
{
    public interface IHostViewModel
    {
        string ControlBtnHost { get; set; }
        string ControlBtnName { get; set; }
        string ErrorHostnameVisibleIcon { get; set; }
        string? HostnameView { get; set; }
        DelegateCommand StartCommand { get; }
        string TextErrorToolTip { get; }
        ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;

        void ErrorNameHostname();
        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}