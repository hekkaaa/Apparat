﻿using Apparat.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinObserver.Model;
using WinObserver.Service;

namespace Apparat.ViewModel
{
    public class HostViewModel : INotifyPropertyChanged, IHostViewModel
    {
        private readonly TracertService? _tracerService;
        private bool _statusWorkDataGrid = false;
        public ReadOnlyObservableCollection<TracertModel>? TracertObject { get; set; }
        private string? _hostnameView;
        private GeneralPanelModel? _generalPanelModel;

        public string? HostnameView
        {
            get { return _hostnameView; }
            set { _hostnameView = value; OnPropertyChanged(); }
        }

        public HostViewModel()
        {
            _tracerService = new TracertService();
            TracertObject = _tracerService._tracertValue;
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
                        _tracerService!.StopTraceroute();

                        _statusWorkDataGrid = false;
                        //ControlBtnName = ViewStatusStringBtn.Start.ToString();
                    }
                    else
                    {
                        //if (String.IsNullOrWhiteSpace(_hostname))
                        //{
                        //    ErrorValidationTextAndAnimation();
                        //}
                        //else
                        //{
                        //NameTableDataGrid = _hostname;
                        //ControlBtnName = ViewStatusStringBtn.Stop.ToString();
                        //RestartInfoInDataGrid();
                        _tracerService.StartTraceroute(HostnameView, this);
                        //RemoveInfoinTextBoxPanel();
                        //_statusWorkDataGrid = true;
                        //}
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

        public string ControlBtnName
        {
            get { return _generalPanelModel!.NameControlBtn; }
            set
            {
                _generalPanelModel!.NameControlBtn = value;
                OnPropertyChanged();
            }
        }

        // Можно потом придумать глобальный стиль для всех.
        public string _ColorTest = "";
        public string ColorTest
        {
            get
            {
                return _ColorTest;
            }
            set
            {
                ColorTest = value;
                OnPropertyChanged();
            }
        }

        public void ErrorNameHostname()
        {
            Task.Factory.StartNew(() =>
            {
                //TextBlockGeneralError = "Hostname not valid";
                //BorderTextBox = "Red";

                //RemoveInfoinTextBoxPanel();

                //Task.Delay(5000).Wait();

                //TextBlockGeneralError = string.Empty;
                //BorderTextBox = "#FFABADB3";
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}