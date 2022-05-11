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
        private ItemPing selectIp;
        private int _click;

        private readonly TracertService _tracerService;
        public ReadOnlyObservableCollection<TracertModel> TracertObject { get; set; }

        public int Click
        {
            get { return _click; }
            set
            {
                _click = value;
                OnPropertyChanged("Click");
            }
        }

       
        private DelegateCommand startTracert;
        public DelegateCommand ClickAdd
        {
            get
            {
                return startTracert ?? new DelegateCommand((obj) =>
                {
                    Click = Click + 10;
                });
            }
        }

        public DelegateCommand StartTracert
        {
            get
            {
                return startTracert ?? new DelegateCommand((obj) =>
                {   
                    _tracerService.StartTraceroute();
                    OnPropertyChanged();
                });
            }
        }

        public ApplicationViewModel()
        {
            _tracerService = new TracertService();
            TracertObject = _tracerService._tracertValue;


            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        Task.Delay(2000).Wait();
            //        Click++;
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
