using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.Model;
using WinObserver.Service;

namespace Apparat.ViewModel
{
    public class HostViewModel : INotifyPropertyChanged, IHostViewModel
    {
        private readonly ITracertService? _tracerService;
        private bool _statusWorkDataGrid = false;
        public ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }
        private string? _hostnameView;

        public string? HostnameView
        {
            get { return _hostnameView; }
            set { _hostnameView = value; OnPropertyChanged(); }
        }

        public HostViewModel()
        {
            _tracerService = new TracertService();
            TracertObject = _tracerService.GetActualCollectionTracertValue();
        }

        private DelegateCommand? _startCommand { get; }
        public DelegateCommand StartCommand
        {
            get
            {
                return _startCommand ?? new DelegateCommand((obj) =>
                {
                    if (_statusWorkDataGrid)
                    {
                        ControlBtnHost = IconeMap.Restart;
                        _tracerService!.StopStreamTracerouteHost();
                        _statusWorkDataGrid = false;
                    }
                    else
                    {
                        //RestartInfoInDataGrid();
                        ControlBtnHost = IconeMap.Stop;
                        _tracerService!.StartStreamTracerouteHost(HostnameView!, this);
                        _statusWorkDataGrid = true;
                        //RemoveInfoinTextBoxPanel();
                    }
                    OnPropertyChanged();
                });
            }
        }

        private GeneralPanelModel? _generalPanelModel;
        public string ControlBtnName
        {
            get { return _generalPanelModel!.NameControlBtn; }
            set { _generalPanelModel!.NameControlBtn = value; OnPropertyChanged(); }
        }

        private string _TextErrorToolTip = "The hostname is entered incorrectly";
        public string TextErrorToolTip
        {
            get { return _TextErrorToolTip; }
        }

        private string _errorHostnameVisibleIcon = "Collapsed";
        public string ErrorHostnameVisibleIcon
        {
            get { return _errorHostnameVisibleIcon; }
            set { _errorHostnameVisibleIcon = value; OnPropertyChanged(); }
        }

        private string _controlBtnHost = IconeMap.Start;
        public string ControlBtnHost
        {
            get { return _controlBtnHost; }
            set { _controlBtnHost = value; OnPropertyChanged(); }
        }

        private string _settingOpacityControlBtn = "1.0";
        public string SettingOpacityControlBtn
        {
            get { return _settingOpacityControlBtn; }
            set { _settingOpacityControlBtn = value; OnPropertyChanged(); }
        }

        private string _settingIsEnableControlBtn = "True";
        public string SettingIsEnableControlBtn
        {
            get { return _settingIsEnableControlBtn; }
            set { _settingIsEnableControlBtn = value; OnPropertyChanged(); }
        }

        private string _valueVisibleProgressBar = "Hidden";
        public string ValueVisibleProgressBar
        {
            get { return _valueVisibleProgressBar; }
            set { _valueVisibleProgressBar = value; OnPropertyChanged(); }
        }

        public void ErrorNameHostname()
        {
            ControlBtnHost = IconeMap.Start;
            ValueVisibleProgressBar = "Collapsed";
            ErrorHostnameVisibleIcon = "Visible";
            SettingIsEnableControlBtn = "False";
            SettingOpacityControlBtn = "0.5";
        }


        public void WorkingProggresbarInListBoxHostanme(bool boolValue)
        {
            if (boolValue)
            {
                ValueVisibleProgressBar = "Visible";
            }
            else
            {
                ValueVisibleProgressBar = "Hidden";
            }
        }

        public void ManagementEnableGeneralControlBtn(bool obj)
        {
            if (obj)
            {
                SettingIsEnableControlBtn = "True";
                SettingOpacityControlBtn = "1";
            }
            else
            {
                SettingIsEnableControlBtn = "False";
                SettingOpacityControlBtn = "0.5";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
