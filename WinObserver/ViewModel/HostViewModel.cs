using Apparat.Commands;
using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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
        private IHostViewModelEvents _HostViewModelEvents = new HostViewModelEvents();
        private ILogger<IHostViewModel> _logger;


        public string? HostnameView
        {
            get { return _hostnameView; }
            set { _hostnameView = value; OnPropertyChanged(); }
        }

        public HostViewModel(ILogger<IHostViewModel> log)
        {
            _logger = log;
            _tracerService = new TracertService();
            TracertObject = _tracerService.GetActualCollectionTracertValue();
            _publicId = Guid.NewGuid().ToString("N");

            // Add Events
            _HostViewModelEvents.ErrorNameHostnameEvent += ErrorNameHostname;
            _HostViewModelEvents.ManagementEnableGeneralControlBtnEvent += ManagementEnableGeneralControlBtn;
            _HostViewModelEvents.WorkingProggresbarInListBoxHostnameEvent += WorkingProggresbarInListBoxHostname;
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
                        _logger.LogWarning($"Stop traceroute {HostnameView}| ID:{PublicId}");
                        ControlBtnHost = IconeMap.Restart;
                        _tracerService!.StopStreamTracerouteHost();
                        _statusWorkDataGrid = false;
                    }
                    else
                    {
                        _logger.LogWarning($"Start traceroute {HostnameView}| ID:{PublicId}");
                        ControlBtnHost = IconeMap.Stop;
                        ControlDatatime();
                        _tracerService!.StartStreamTracerouteHost(HostnameView!, _HostViewModelEvents);
                        _statusWorkDataGrid = true;
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

        private string _publicId = string.Empty;
        public string PublicId
        {
            get { return _publicId; }
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

        private string _startDatatime = string.Empty;
        public string StartDatatime
        {
            get { return _startDatatime; }
            set { _startDatatime = value; OnPropertyChanged(); }
        }

        private string _visibleDatatimeTextBlock = "Collapsed";
        public string VisibleDatatimeTextBlock
        {
            get { return _visibleDatatimeTextBlock; }
            set { _visibleDatatimeTextBlock = value; OnPropertyChanged(); }
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void ControlDatatime()
        {
            DateTime dt = DateTime.Now;
            StringBuilder sb = new StringBuilder("Start Time: ");
            sb.Append(dt.ToString("T"));
            StartDatatime = sb.ToString();
            VisibleDatatimeTextBlock = "Visible";
        }

        private void ErrorNameHostname()
        {
            ControlBtnHost = IconeMap.Start;
            StartDatatime = "Error";
            ValueVisibleProgressBar = "Collapsed";
            ErrorHostnameVisibleIcon = "Visible";
            SettingIsEnableControlBtn = "False";
            SettingOpacityControlBtn = "0.5";
        }

        // Events
        private void WorkingProggresbarInListBoxHostname(bool boolValue)
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

        private void ManagementEnableGeneralControlBtn(bool boolValue)
        {
            if (boolValue)
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
    }
}
