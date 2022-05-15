﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinObserver.Model
{
    public class TracertModel : INotifyPropertyChanged
    {
        //private string? _generalHostname;
        private string? _hostname;
        private int? _lastDelay;

        private int _minPing = 0;
        private int _middlePing = 0;
        private int _maxPing = 0;

        private double _percentLossPacket = 0;
        private int _counterPacket = 0;
        private int _counterLossPacket = 0;

        private List<int>? _arhivePingList = new List<int>();

        //public string? GeneralHostname
        //{
        //    get { return _generalHostname; }
        //    set
        //    {
        //        _generalHostname = value;
        //        OnPropertyChanged("GeneralHostname");
        //    }
        //}

        public string? Hostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                OnPropertyChanged("Hostname");
            }
        }
        public int? LastDelay
        {
            get { return _lastDelay; }
            set
            {
                _lastDelay = value;
                OnPropertyChanged("LastDelay");
            }
        }

        public int MinPing
        {
            get { return _minPing; }
            set
            {
                _minPing = value;
                OnPropertyChanged("MinPing");
            }
        }

        public int MiddlePing
        {
            get { return _middlePing; }
            set
            {
                _middlePing = value;
                OnPropertyChanged("MiddlePing");
            }
        }

        public int MaxPing
        {
            get { return _maxPing; }
            set
            {
                _maxPing = value;
                OnPropertyChanged("MaxPing");
            }
        }

        public double PercentLossPacket
        {
            get { return _percentLossPacket; }
            set
            {
                _percentLossPacket = value;
                OnPropertyChanged("PercentLossPacket");
            }
        }

        public int CounterPacket
        {
            get { return _counterPacket; }
            set
            {
                _counterPacket = value;
                OnPropertyChanged("CounterPacket");
            }
        }

        public int CounterLossPacket
        {
            get { return _counterLossPacket; }
            set
            {
                _counterLossPacket = value;
                OnPropertyChanged("CounterLossPacket");
            }
        }

        public List<int>? ArhivePingList
        {
            get { return _arhivePingList; }
            //set 
            //{ 
            //    _arhivePingList = value;
            //    OnPropertyChanged("ArhivePingList");
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

