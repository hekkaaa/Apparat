using Apparat.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;

namespace Apparat.ViewModel.Interfaces
{
    public interface IHostViewModel
    {
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

        void ErrorNameHostname();
        void ManagementEnableGeneralControlBtn(bool obj);
        void WorkingProggresbarInListBoxHostanme(bool boolValue);
        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}