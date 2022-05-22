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

        private GeneralPanelModel? _generalPanelModel;
        private readonly TracertService? _tracerService;
        private readonly ChartRepository _chartRepository;
        private ReadOnlyObservableCollection<Axis> _timeInfoXAxes;
        private ReadOnlyObservableCollection<Axis> _valueInfoYAxes;


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

        public ReadOnlyObservableCollection<Axis> XAxes
        {
            get { return _timeInfoXAxes; }
            //set { _timeInfoXAxes = value; OnPropertyChanged(); }
        }

        public ReadOnlyObservableCollection<Axis> YAxes
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
                        ControlBtnName = ViewStatusStringBtn.Stop.ToString();
                        RestartInfoInDataGrid();
                        NameTableDataGrid = _hostname.ToString();
                        _tracerService!.StartTraceroute(_hostname);
                        RemoveInfoinTextBoxPanel();
                        _statusWorkDataGrid = true;
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
            _chartRepository = new ChartRepository();
            _tracerService = new TracertService(_chartRepository);
            _generalPanelModel = new GeneralPanelModel();
            TracertObject = _tracerService._tracertValue;
            _timeInfoXAxes = _chartRepository._ObjectXAxes;
            _valueInfoYAxes = _chartRepository._ObjectYAxes;

            //TextBoxHostname = "vk.com"; // Потом убрать!
         
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

    }
}
