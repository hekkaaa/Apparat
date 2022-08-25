using Apparat.Commands;
using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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
        private ILogger _logger;

        public string? HostnameView
        {
            get { return _hostnameView; }
            set { _hostnameView = value; OnPropertyChanged(); }
        }

        public HostViewModel(ILogger log)
        {
            _logger = log;
            _tracerService = new TracertService(_logger);
            TracertObject = _tracerService.GetActualCollectionTracertValue();

            // Generate unique id
            GeneradeUniqueIdInPublicIdPropetry();

            // Add Events
            _HostViewModelEvents.ErrorNameHostnameEvent += ErrorNameHostname;
            _HostViewModelEvents.ManagementEnableGeneralControlBtnEventAndPreloaderVisible += ManagementEnableGeneralControlBtn;
            _HostViewModelEvents.ManagementEnableGeneralControlBtnEventAndPreloaderVisible += VisibleDatagridOrPreloaderOrStubGridInGeneralPanerTabControl;
            _HostViewModelEvents.WorkingProggresbarInListBoxHostnameEvent += WorkingProggresbarInListBoxHostname;


            _xAxisGraph1 = DefaultValueXaXies();
            _xAxisGraph2 = DefaultValueXaXies();

            _logger.LogWarning($"Successful creation {HostnameView}. ID: {PublicId}");
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

        private DelegateCommand? _ApplySetting { get; }
        public DelegateCommand ApplySetting
        {
            // Update setting in Work Stream.
            get
            {
                return _ApplySetting ?? new DelegateCommand((obj) =>
                {
                    if (_statusWorkDataGrid)
                    {
                        _tracerService.UpdateDelayValue(_delayInRequestsToUpdateStatistics);
                        _tracerService.UpdateSizePacketValue(_sizePacketInRequestsToUpdateStatistics);
                        _logger.LogWarning($"Update Delay in host {HostnameView}. ID: {PublicId}");
                    }
                    Task.Factory.StartNew(() =>
                    {
                        TextinSettingBtn = "Save wait";
                        Task.Delay(500).Wait();
                        TextinSettingBtn = "Save wait..";
                        Task.Delay(500).Wait();
                        TextinSettingBtn = "Save wait...";
                        Task.Delay(500).Wait();
                        TextinSettingBtn = "Apply";
                    });
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

        private int _delayInRequestsToUpdateStatistics = 1000;
        public int DelayInRequestsToUpdateStatistics
        {
            get { return _delayInRequestsToUpdateStatistics; }
            set { _delayInRequestsToUpdateStatistics = value; OnPropertyChanged(); }
        }

        private int _sizePacketInRequestsToUpdateStatistics = 32;
        public int SizePacketInRequestsToUpdateStatistics
        {
            get { return _sizePacketInRequestsToUpdateStatistics; }
            set { _sizePacketInRequestsToUpdateStatistics = value; OnPropertyChanged(); }
        }

        private string _textinToolTipsFromControlBtn = "Start traceroute";
        public string TextinToolTipsFromControlBtn
        {
            get { return _textinToolTipsFromControlBtn; }
            set { _textinToolTipsFromControlBtn = value; OnPropertyChanged(); }
        }

        private string _textinSettingBtn = "Apply";
        public string TextinSettingBtn
        {
            get { return _textinSettingBtn; }
            set { _textinSettingBtn = value; OnPropertyChanged(); }
        }

        /// Graph 1
        private List<Axis> _xAxisGraph1 = null;
        public List<Axis> XAxesGraph1
        {
            get { return _xAxisGraph1; }
            set
            {
                _xAxisGraph1 = value;
                OnPropertyChanged();
            }
        }
        /// Graph 2
        private List<Axis> _xAxisGraph2 = null;
        public List<Axis> XAxesGraph2
        {
            get { return _xAxisGraph2; }
            set
            {
                _xAxisGraph2 = value;
                OnPropertyChanged();
            }
        }

        public Axis[] YAxesGraph1 { get; set; } =
        {
            new Axis
            {
                 MinLimit = 0,
                 MaxLimit = 100,
                 MinStep = 10,
            }
        };


        public Axis[] YAxesGraph2 { get; set; } =
       {
            new Axis
            {
                 MinLimit = 0,
                 MaxLimit = 1,
            }
        };

        private ObservableCollection<ISeries> _valuesLossGraph1;
        public ObservableCollection<ISeries> SeriesGraph1
        {
            get { return _valuesLossGraph1; }
            set { _valuesLossGraph1 = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ISeries> _valuesLossGraph2;
        public ObservableCollection<ISeries> SeriesGraph2
        {
            get { return _valuesLossGraph2; }
            set { _valuesLossGraph2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Stop Stream Traceroute to hostname.
        /// </summary>
        /// <returns></returns>
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

        public bool StatusWorkDataGrid
        {
            get { return _statusWorkDataGrid; }
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
                _tracerService!.StartStreamTracerouteHost(HostnameView!, _HostViewModelEvents, DelayInRequestsToUpdateStatistics);
                _statusWorkDataGrid = true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Start Stream Traceroute {HostnameView} | ID: {PublicId}");
                return false;
            }

        }
        /// <summary>
        /// Command Update Graphs 1 or 2 in select Tabitem.
        /// </summary>
        private DelegateCommand? _updateAllGraph { get; } = null;
        public DelegateCommand UpdateAllGraph
        {
            get
            {
                return _updateAllGraph ?? new DelegateCommand((obj) =>
                {
                    _valuesLossGraph1 = new ObservableCollection<ISeries>();

                    foreach (var item in TracertObject)
                    {
                        _valuesLossGraph1.Add(new LineSeries<int>
                        {
                            DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 22f),
                            Name = item.Hostname,
                            Values = item.ArhiveStateValuePercentLossPacket,
                            Fill = null,
                            LineSmoothness = 0,
                            GeometrySize = 0,
                        });
                    };

                    SeriesGraph1 = _valuesLossGraph1;

                    _xAxisGraph1 = new List<Axis>
                        {
                             new Axis
                            {
                                NameTextSize = 14,
                                Name = "General graph of packet loss",
                                Labels = _tracerService.GetArhiveTimeRequestCollection(),
                                LabelsRotation = 15,
                            }
                        };

                    XAxesGraph1 = _xAxisGraph1;

                    ////////////////
                    _valuesLossGraph2 = new ObservableCollection<ISeries>();

                    foreach (var item in TracertObject)
                    {
                        _valuesLossGraph2.Add(new StepLineSeries<int>
                        {
                            DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 12),
                            Name = item.Hostname,
                            Values = item.ArhiveStatusRequestPacket,
                            Fill = null,
                            GeometrySize = 0,
                        });
                    };

                    SeriesGraph2 = _valuesLossGraph2;

                    _xAxisGraph2 = new List<Axis>
                        {
                             new Axis
                            {
                                NameTextSize = 14,
                                Name = "Graph of % losses for all time",
                                Labels = _tracerService.GetArhiveTimeRequestCollection(),
                                LabelsRotation = 15,
                            }
                        };

                    XAxesGraph2 = _xAxisGraph2;

                    _logger.LogWarning($"Update Graph in hostname {HostnameView}. ID: {PublicId}");
                    OnPropertyChanged();
                });
            }
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


        private List<Axis> DefaultValueXaXies()
        {
            return new List<Axis>
            {
                 new Axis
                {
                    NameTextSize = 14,
                    Name = "Time",
                    Labels = new string[] { "Time Now" },
                    LabelsRotation = 15
                }
            };
        }
    }
}
