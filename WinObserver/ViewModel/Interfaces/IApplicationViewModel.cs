using Apparat.Commands;
using Apparat.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinObserver.ViewModel
{
    public interface IApplicationViewModel
    {
        DelegateCommand AddNewHost { get; }
        string BorderTextBox { get; set; }
        DelegateCommand ClearAllCollectionHost { get; }
        DelegateCommand CloseTabCommand { get; }
        ObservableCollection<ExplorerViewModel> CollectionFoldersInExplorer { get; set; }
        ObservableCollection<string> CollectionRecentHost { get; set; }
        DelegateCommand DeleteOneItemHistoryHostname { get; }
        //ObservableCollection<HostViewModel> HostsCollection { get; set; }
        string TextBlockGeneralError { get; set; }
        string TextBoxHostname { get; set; }
        string VersionProgramm { get; }

        event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string prop = "");
    }
}