using Apparat.Commands;
using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
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


            _xAxisGraph1 = DefaultValueXandYaXies();
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

        private DelegateCommand? _updateGraph1 { get; } = null;
        public DelegateCommand UpdateGraph1
        {
            get
            {
                return _updateGraph1 ?? new DelegateCommand((obj) =>
                {
                    _valuesLossGraph1 = new ObservableCollection<ISeries>();

                    foreach (var item in TracertObject)
                    {
                        _valuesLossGraph1.Add(new LineSeries<int>
                        {
                            Name = item.Hostname,
                            Values = item.ArhiveStateValuePercentLossPacket,
                            Fill = null,
                        });
                    };

                    SeriesGraph1 = _valuesLossGraph1;

                    _xAxisGraph1 = new List<Axis>
                        {
                             new Axis
                            {
                                Name = "General graph of packet loss",
                                Labels = _tracerService.GetArhiveTimeRequestCollection(),
                                LabelsRotation = 15
                            }
                        };

                    XAxesGraph1 = _xAxisGraph1;

                    ////////////////
                    _valuesLossGraph2 = new ObservableCollection<ISeries>();

                    foreach (var item in TracertObject)
                    {
                        _valuesLossGraph2.Add(new LineSeries<int>
                        {
                            Name = item.Hostname,
                            Values = item.ArhiveStatusRequestPacket,
                            Fill = null,
                        });
                    };

                    SeriesGraph2 = _valuesLossGraph2;

                    _xAxisGraph2 = new List<Axis>
                        {
                             new Axis
                            {
                                Name = "Graph of % losses for all time",
                                Labels = _tracerService.GetArhiveTimeRequestCollection(),
                                LabelsRotation = 15
                            }
                        };

                    XAxesGraph2 = _xAxisGraph2;


                    OnPropertyChanged();
                });
            }
        }

        //new LineSeries<int>
        //            {
        //                Name = _hostnameView,
        //                Values = TracertObject,
        //                Fill = null
        //            },

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

      
        private List<Axis> DefaultValueXandYaXies()
        {
            return _xAxisGraph1 = new List<Axis>
            {
                 new Axis
                {
                    Name = "Time",
                    Labels = new string[] { "Time Now" },
                    LabelsRotation = 15
                }
            };
        }

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
        public ObservableCollection<ISeries> SeriesGraph1 { 
            get { return _valuesLossGraph1; } 
            set { _valuesLossGraph1 = value; OnPropertyChanged(); } }

        private ObservableCollection<ISeries> _valuesLossGraph2;
        public ObservableCollection<ISeries> SeriesGraph2 { 
            get { return _valuesLossGraph2; } 
            set { _valuesLossGraph2 = value; OnPropertyChanged(); } }

    }
}
