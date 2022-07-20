using Apparat.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;

namespace Apparat.ViewModel.Interfaces
{
    public interface IHostViewModel
    {
        string PublicId { get;}
        string ControlBtnHost { get; set; }
        string ControlBtnName { get; set; }
        string ErrorHostnameVisibleIcon { get; set; }
        string? HostnameView { get; set; }
        string SettingIsEnableControlBtn { get; set; }
        string SettingOpacityControlBtn { get; set; }
        DelegateCommand StartCommand { get; }
        string TextErrorToolTip { get; }
        ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;
        bool StopStream();
        bool StartStream();
        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}