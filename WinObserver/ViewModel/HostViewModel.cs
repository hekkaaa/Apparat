using Apparat.Commands;
using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

            // Generate unique id
            GeneradeUniqueIdInPublicIdPropetry();

            // Add Events
            _HostViewModelEvents.ErrorNameHostnameEvent += ErrorNameHostname;
            _HostViewModelEvents.ManagementEnableGeneralControlBtnEventAndPreloaderVisible += ManagementEnableGeneralControlBtn;
            _HostViewModelEvents.ManagementEnableGeneralControlBtnEventAndPreloaderVisible += VisibleDatagridOrPreloaderOrStubGridInGeneralPanerTabControl;
            _HostViewModelEvents.WorkingProggresbarInListBoxHostnameEvent += WorkingProggresbarInListBoxHostname;
        }

        private DelegateCommand? _startCommand { get; }
        public DelegateCommand StartCommand
        {
            // Start - Stop
            get
            {
                return _startCommand ?? new DelegateCommand((obj) =>
                {
                    if (_statusWorkDataGrid)
                    {
                        StopStream();
                    }
                    else
                    {
                        StartStream();
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

        private string _visibleDataGridTable = "Collapsed";
        public string VisibleDataGridTable
        {
            get { return _visibleDataGridTable; }
            set { _visibleDataGridTable = value; OnPropertyChanged(); }
        }

        private string _visiblePrealoaderGrid = "Collapsed";
        public string VisiblePrealoaderGrid
        {
            get { return _visiblePrealoaderGrid; }
            set { _visiblePrealoaderGrid = value; OnPropertyChanged(); }
        }

        private string _visibleStupGrid = "Visible";
        public string VisibleStupGrid
        {
            get { return _visibleStupGrid; }
            set { _visibleStupGrid = value; OnPropertyChanged(); }
        }

        private string _visibleErrorStupGrid = "Collapsed";
        public string VisibleErrorStupGrid
        {
            get { return _visibleErrorStupGrid; }
            set { _visibleErrorStupGrid = value; OnPropertyChanged(); }
        }

        private string _textinToolTipsFromControlBtn = "Start traceroute";
        public string TextinToolTipsFromControlBtn
        {
            get { return _textinToolTipsFromControlBtn; }
            set { _textinToolTipsFromControlBtn = value; OnPropertyChanged(); }
        }

        public bool StopStream()
        {
            try
            {
                if (ErrorHostnameVisibleIcon == "Visible")
                {
                    return true;
                }

                _logger.LogWarning($"Stop traceroute {HostnameView}| ID:{PublicId}");
                VisaulChangeAtStopStream();
                _tracerService!.StopStreamTracerouteHost();
                _statusWorkDataGrid = false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Stop Stream Traceroute {HostnameView} | ID: {PublicId}");
                return false;
            }
        }

        public bool StartStream()
        {
            try
            {
                if (ErrorHostnameVisibleIcon == "Visible")
                {
                    return true;
                }

                _logger.LogWarning($"Start traceroute {HostnameView}| ID:{PublicId}");
                VisaulChangeAtStartupStream();
                ControlDatatime();
                _tracerService!.StartStreamTracerouteHost(HostnameView!, _HostViewModelEvents);
                _statusWorkDataGrid = true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Start Stream Traceroute {HostnameView} | ID: {PublicId}");
                return false;
            }

        }
        //private List<string> _test1111 = new List<string>() { "qwe", "jjss", "lss" };
        //public List<string> ListTest23
        //{
        //    get
        //    {
        //        return _test1111;
        //    }
        //}


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

            VisibleStupGrid = "Collapsed";
            VisiblePrealoaderGrid = "Collapsed";
            VisibleDataGridTable = "Collapsed";
            VisibleErrorStupGrid = "Visible";
        }

        private void GeneradeUniqueIdInPublicIdPropetry()
        {
            _publicId = Guid.NewGuid().ToString("N");
        }

        // Visual changes during events 

        private void VisaulChangeAtStartupStream()
        {
            TextinToolTipsFromControlBtn = "Stop traceroute";
            ControlBtnHost = IconeMap.Stop;
            
        }

        private void VisaulChangeAtStopStream()
        {
            ManagementEnableGeneralControlBtn(false);
            TextinToolTipsFromControlBtn = "Restart traceroute";
            ControlBtnHost = IconeMap.Restart;
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

        private void VisibleDatagridOrPreloaderOrStubGridInGeneralPanerTabControl(bool boolValue)
        {
            if (boolValue)
            {
                VisiblePrealoaderGrid = "Collapsed";
                VisibleDataGridTable = "Visible";
            }
            else
            {
                VisibleStupGrid = "Collapsed";
                VisiblePrealoaderGrid = "Visible";
                VisibleDataGridTable = "Collapsed";
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
