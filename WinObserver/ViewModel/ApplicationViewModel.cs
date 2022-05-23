using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WinObserver.Model;
using WinObserver.Repositories;
using WinObserver.Service;

namespace WinObserver.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private int _click;
        private string _hostname;
        private bool _statusWorkDataGrid = false;
        private string _textBlockGeneralError;
        private string _borderTextBox = "#FFABADB3";

        private GeneralPanelModel? _generalPanelModel;
        private readonly TracertService? _tracerService;
        private readonly ChartRepository _chartRepository;
        private List<Axis> _timeInfoXAxes;
        private List<Axis> _valueInfoYAxes;


        public ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }

        public string ControlBtnName
        {
            get { return _generalPanelModel!.NameControlBtn; }
            set
            {
                _generalPanelModel!.NameControlBtn = value;
                OnPropertyChanged();
            }
        }

        public List<Axis> XAxes
        {
            get { return _timeInfoXAxes; }
            //set { _timeInfoXAxes = value; OnPropertyChanged(); }
        }

        public List<Axis> YAxes
        { 
            get { return _valueInfoYAxes; } 
        }

        public ReadOnlyObservableCollection<ISeries> Losses
        {
            get
            {
                return _chartRepository._lossList;
            }

        }

        public string NameTableDataGrid
        {
            get { return _tracerService!._gridTracert.HeaderNameTable; }
            set
            {
                _tracerService!._gridTracert.HeaderNameTable = value;
                OnPropertyChanged();
            }
        }

        public string TextBoxHostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                OnPropertyChanged();
            }
        }

        public int Click
        {
            get { return _click; }
            set
            {
                _click = value;
                OnPropertyChanged("Click");
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

        public string VersionProgramm { get; set; }

        private DelegateCommand? controlTracert { get; }
        public DelegateCommand ControlTracert
        {
            get
            {
                return controlTracert ?? new DelegateCommand((obj) =>
                {
                    if (_statusWorkDataGrid)
                    {
                        _tracerService!.StopTraceroute();
                        _statusWorkDataGrid = false;
                        ControlBtnName = ViewStatusStringBtn.Start.ToString();
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(_hostname))
                        {
                            ErrorValidationTextAndAnimation();
                        }
                        else
                        {
                            NameTableDataGrid = _hostname;
                            ControlBtnName = ViewStatusStringBtn.Stop.ToString();
                            RestartInfoInDataGrid();
                            _tracerService!.StartTraceroute(_hostname, this);
                            RemoveInfoinTextBoxPanel();
                            _statusWorkDataGrid = true;
                        }
                    }
                    OnPropertyChanged();
                });
            }
        }


        private DelegateCommand? stopCommand;
        public DelegateCommand StopCommand
        {
            get
            {
                return stopCommand ?? (stopCommand = new DelegateCommand(obj =>
                {
                    _tracerService!.StopTraceroute();
                }));
            }
        }

        public ApplicationViewModel()
        {
            VersionProgramm = "Version: 0.0.15 - alpha";
            _chartRepository = new ChartRepository();
            _tracerService = new TracertService(_chartRepository);
            _generalPanelModel = new GeneralPanelModel();
            TracertObject = _tracerService._tracertValue;
            _timeInfoXAxes = _chartRepository._ObjectXAxes;
            _valueInfoYAxes = _chartRepository._ObjectYAxes;
         
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void RestartInfoInDataGrid()
        {
            TracertObject = null;
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
                NameTableDataGrid = "New";

                _statusWorkDataGrid = false;
                ControlBtnName = ViewStatusStringBtn.Start.ToString();
                RemoveInfoinTextBoxPanel();
               
                Task.Delay(5000).Wait();

                TextBlockGeneralError = string.Empty;
                BorderTextBox = "#FFABADB3";
            });
        }

    }
}
