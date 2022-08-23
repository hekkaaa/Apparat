using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinObserver.Model
{
    public class TracertModel : INotifyPropertyChanged
    {
        private int _numberHostname;
        private string _hostname = string.Empty;
        private int? _lastDelay;

        private int _minPing = 0;
        private int _middlePing = 0;
        private int _maxPing = 0;

        private int _percentLossPacket = 0;
        private int _counterPacket = 0;
        private int _counterLossPacket = 0;

        private string _colorLossView = "Black";

        private List<int>? _arhivePingList = new List<int>();
        private List<int> _arhiveStatusRequestPacket = new List<int>(); // 0 or 1

        public int NumberHostname
        {
            get { return _numberHostname; }
            set
            {
                _numberHostname = value;
                OnPropertyChanged("NumberHostname");
            }
        }

        public string ColorLossView
        {
            get { return _colorLossView; }
            set
            {
                _colorLossView = value;
                OnPropertyChanged("ColorLossView");
            }
        }

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

        public int PercentLossPacket
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
        }

        public List<int>? ArhiveStatusRequestPacket
        {
            get { return _arhiveStatusRequestPacket; }
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

