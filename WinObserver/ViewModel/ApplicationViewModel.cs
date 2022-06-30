using Apparat.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WinObserver.Service;

namespace WinObserver.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        const string VERSION_APP = "Version: 0.1.6 - alpha";
        private string _hostname;
        private string _textBlockGeneralError;
        private string _borderTextBox = "#FFABADB3";

        private ObservableCollection<HostViewModel> _hostsCollection;

        public ApplicationViewModel()
        {
            _hostsCollection = new ObservableCollection<HostViewModel>();
        }

        public string VersionProgramm { get { return VERSION_APP; } }

        public string TextBoxHostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                OnPropertyChanged();
            }
        }

        public string TextBlockGeneralError
        {
            get
            {
                return _textBlockGeneralError;
            }
            set
            {
                _textBlockGeneralError = value;
                OnPropertyChanged();
            }
        }

        public string BorderTextBox
        {
            get
            {
                return _borderTextBox;
            }
            set
            {
                _borderTextBox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HostViewModel> HostsCollection
        {
            get => _hostsCollection;
            set { _hostsCollection = value; OnPropertyChanged(); }
        }


        private DelegateCommand _addNewHost;
        public DelegateCommand AddNewHost
        {
            get
            {
                return _addNewHost ??
                 (_addNewHost = new DelegateCommand((obj) =>
                 {
                     if (String.IsNullOrWhiteSpace(_hostname))
                     {
                         ErrorValidationTextAndAnimation();
                         return;
                     }
                     HostsCollection.Add(new HostViewModel()
                     {
                         HostnameView = _hostname
                     });
                     RemoveInfoinTextBoxPanel();
                     OnPropertyChanged();
                 }));
            }
        }

        private DelegateCommand _closeTabCommand;
        public DelegateCommand CloseTabCommand
        {
            get
            {
                return _closeTabCommand
                ?? (_closeTabCommand = new DelegateCommand(
                (obj) =>
                {
                    HostsCollection.Remove(obj as HostViewModel);
                }));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void RemoveInfoinTextBoxPanel()
        {
            TextBoxHostname = null!;
        }


        public void ErrorValidationTextAndAnimation()
        {
            Task.Factory.StartNew(() =>
            {
                TextBlockGeneralError = "Hostname not valid";
                BorderTextBox = "Red";

                RemoveInfoinTextBoxPanel();

                Task.Delay(5000).Wait();

                TextBlockGeneralError = string.Empty;
                BorderTextBox = "#FFABADB3";
            });
        }

    }
}
