using Apparat.Commands;
using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel;
using Apparat.ViewModel.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WinObserver.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged, IApplicationViewModel
    {
        const string VERSION_APP = "Version: 1.0.0 - pre-Alpha";
        private string _hostname = String.Empty;
        private string _textBlockGeneralError = String.Empty;
        private string _borderTextBox = "#FFABADB3";

        private ObservableCollection<ExplorerViewModel> _collectionFoldersInExplorer;
        private ObservableCollection<HostViewModel> _hostsCollection;
        private readonly IAppSettingService _appSettingService;

        ILogger<IApplicationViewModel> _logger;
        ILogger<IHostViewModel> _hostVMlog;

        public ApplicationViewModel(IAppSettingService appService,
            ILogger<IApplicationViewModel> log,
            ILogger<IHostViewModel> hostVMlog)
        {
            _hostsCollection = new ObservableCollection<HostViewModel>();
            _logger = log;
            _hostVMlog = hostVMlog;

            // Test Start Info
            _collectionFoldersInExplorer = new ObservableCollection<ExplorerViewModel>() {
                new ExplorerViewModel() {
                    FolderName = "Default",
                    IsNewCreateObj = false,
                    HostVMCollection = new ObservableCollection<HostViewModel>() {
                        new HostViewModel(_hostVMlog){ HostnameView = "vk.com"},
                    new HostViewModel(_hostVMlog){ HostnameView = "ya.ru"}},

                },
                new ExplorerViewModel(){
                    FolderName = "ИП Усатый",
                    IsNewCreateObj = false,
                    HostVMCollection = new ObservableCollection<HostViewModel>() {
                        new HostViewModel(_hostVMlog){ HostnameView = "metanit.com"},
                    new HostViewModel(_hostVMlog){ HostnameView = "github.com"},
                    new HostViewModel(_hostVMlog){ HostnameView = "Monro.ru"},}
                }
            };

            // init object class  
            _appSettingService = appService;
            UpdateCollectionHistoryHostInCombobox();
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

        HostViewModel _selectedGroup = null!;
        public HostViewModel SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value; OnPropertyChanged(); }
        }

        ExplorerViewModel _selectedGroupExplorerVM = null!;
        public ExplorerViewModel SelectedGroupExplorerVM
        {
            get { return _selectedGroupExplorerVM; }
            set { _selectedGroupExplorerVM = value; OnPropertyChanged(); }
        }


        public ObservableCollection<ExplorerViewModel> CollectionFoldersInExplorer
        {
            get
            {
                return _collectionFoldersInExplorer;
            }
            set
            {
                _collectionFoldersInExplorer = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _collectionRecentHost = null!;
        public ObservableCollection<string> CollectionRecentHost
        {
            get
            {
                return _collectionRecentHost;
            }
            set
            {
                _collectionRecentHost = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HostViewModel> HostsCollection
        {
            get => _hostsCollection;
            set { _hostsCollection = value; OnPropertyChanged(); }
        }

        private string _StartValueInVisibleWithGeneralWindowsApp = "Collapsed";
        public string StartValueInVisibleWithGeneralWindowsApp
        {
            get { return _StartValueInVisibleWithGeneralWindowsApp; }
            set { _StartValueInVisibleWithGeneralWindowsApp = value; OnPropertyChanged(); }
        }


        private DelegateCommand _addNewHost = null!;
        public DelegateCommand AddNewHost
        {
            get
            {
                return _addNewHost ??
                 (_addNewHost = new DelegateCommand((obj) =>
                 {
                     string editedHostname = ValidationParseHelper.RemovingSpacesFromText(_hostname);
                     bool result = ValidationParseHelper.ValidationCheck(_hostname);

                     if (!result)
                     {
                         ErrorValidationTextAndAnimation(_hostname);
                         return;
                     }

                     HostViewModel newObject = new HostViewModel(_hostVMlog) { HostnameView = editedHostname };

                     // Add new hostname in default folder View.
                     CollectionFoldersInExplorer.First().HostVMCollection.Add(newObject);

                     _appSettingService.AddHostInHistory(editedHostname);
                     UpdateCollectionHistoryHostInCombobox();
                     RemoveInfoinTextBoxPanel();

                     _logger.LogWarning($"Create new Hostname {editedHostname} | ID:{newObject.PublicId}");
                     OnPropertyChanged();

                 }));
            }
        }

        private DelegateCommand _closeTabCommand = null!;
        public DelegateCommand CloseTabCommand
        {
            get
            {
                return _closeTabCommand
                ?? (_closeTabCommand = new DelegateCommand(
                (obj) =>
                {
                    HostViewModel? deleteObject = obj as HostViewModel;
                    if (deleteObject != null)
                    {
                        foreach (var item in CollectionFoldersInExplorer)
                        {
                            HostViewModel res = item.HostVMCollection.FirstOrDefault(x => x.PublicId == deleteObject.PublicId)!;
                            if (res != null)
                            {
                                if (res.StopStream())
                                {
                                    item.HostVMCollection.Remove(deleteObject);
                                    StartValueInVisibleWithGeneralWindowsApp = "Collapsed";
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        _logger.LogWarning($"Delete object tracert {deleteObject.HostnameView} | ID:{deleteObject.PublicId}");
                    }
                }));
            }
        }

        private DelegateCommand _clearAllCollectionHost = null!;
        public DelegateCommand ClearAllCollectionHost
        {
            get
            {
                return _clearAllCollectionHost
                ?? (_clearAllCollectionHost = new DelegateCommand(
                (obj) =>
                {
                    _appSettingService.ClearAllCollectionHistoryHost();
                    UpdateCollectionHistoryHostInCombobox();
                    _logger.LogWarning($"Clear all collection history");
                }));
            }
        }

        private DelegateCommand _deleteOneItemHistoryHostname = null!;
        public DelegateCommand DeleteOneItemHistoryHostname
        {
            get
            {
                return _deleteOneItemHistoryHostname
                ?? (_deleteOneItemHistoryHostname = new DelegateCommand(
                (obj) =>
                {
                    string deleteObject = obj as string;

                    bool removeResult = _appSettingService.DeleteOneHostnameFromHistoryCollection(deleteObject);
                    if (removeResult)
                    {
                        _logger.LogWarning($"Remove {deleteObject} from collection history hostname");
                        UpdateCollectionHistoryHostInCombobox();
                    }
                }));
            }
        }

        private DelegateCommand _addNewFolderBtn = null!;
        public DelegateCommand AddNewFolderBtn
        {
            get
            {
                return _addNewFolderBtn
                ?? (_addNewFolderBtn = new DelegateCommand(
                (obj) =>
                {
                    var checkOpenNewFolderWindow = _collectionFoldersInExplorer.Where(x => x.IsNewCreateObj == true).ToList();
                    if (checkOpenNewFolderWindow.Count == 0)
                    {
                        _collectionFoldersInExplorer.Add(new ExplorerViewModel()
                        {
                            FolderName = "",
                            SizeElement = "43",
                            VisibleLabelNameFolder = "Collapsed",
                            VisibleTextBoxNameFolder = "Visible",
                            HostVMCollection = new ObservableCollection<HostViewModel>()
                        });
                    }
                    else
                    {
                        return;
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void UpdateCollectionHistoryHostInCombobox()
        {
            CollectionRecentHost = _appSettingService.GetLastFiveHistoryHost();
        }


        private void RemoveInfoinTextBoxPanel()
        {
            TextBoxHostname = null!;
        }


        private void ErrorValidationTextAndAnimation(string errorHostname)
        {
            _logger.LogError($"invalid hostname: '{errorHostname}'");

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
