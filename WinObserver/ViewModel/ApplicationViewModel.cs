using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinObserver.Model;
using WinObserver.Service;

namespace WinObserver.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private int _click;
        private string _hostname;
        private string _TableNameDataGrid = "New";
        private string _btnGeneralName;
        private bool _statusGeneralBtn = true;


        private readonly TracertService _tracerService;
        public ReadOnlyObservableCollection<TracertModel> TracertObject { get; set; }


        public string BtnName
        {
            get { return _btnGeneralName; }
            set
            {
                _btnGeneralName = value;
                OnPropertyChanged();
            }
        }

        public bool StatusGeneralBtn
        {
            get { return _statusGeneralBtn; }
            set
            {
                _statusGeneralBtn = value;
                OnPropertyChanged();
            }
        }

        public string TableNameDataGrid
        {
            get { return _TableNameDataGrid; }
            set
            {
                _TableNameDataGrid = value;
                OnPropertyChanged();
            }
        }

        public string TexboxHostname
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

        private DelegateCommand startTracert { get; }

        public DelegateCommand StartTracert
        {
            get
            {   
                return startTracert ?? new DelegateCommand((obj) =>
                {
                    BtnName = "Stop";
                    StatusGeneralBtn = false;
                    TableNameDataGrid = _hostname.ToString();
                    Task.Factory.StartNew(() =>
                    {
                        _tracerService.StartTraceroute(_hostname);
                        TexboxHostname = null;
                        OnPropertyChanged();
                    });
                });
            }
        }

        public ApplicationViewModel()
        {
            _tracerService = new TracertService();
            TracertObject = _tracerService._tracertValue;
            TexboxHostname = "vk.com";
            BtnName = "Start";

            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        Task.Delay(2000).Wait();
            //        //TracertObject = _tracerService._tracertValue;
            //        if (TracertObject.Count != 0)
            //        {
            //            //TracertObject[0].Delay += 20;
            //            Click = (int)TracertObject[0].Delay;
            //        }

            //    }
            //});

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
