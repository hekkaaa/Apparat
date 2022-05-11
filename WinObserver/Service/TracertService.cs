using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinObserver.Model;

namespace WinObserver.Service
{
    public class TracertService : INotifyPropertyChanged
    {
        private ObservableCollection<TracertModel> _innerTracertValue;
        public readonly ReadOnlyObservableCollection<TracertModel> _tracertValue;
        
        //private string _ip;

        //public string Ip
        //{
        //    get { return _ip; }
        //    set
        //    {
        //        _ip = value;
        //        OnPropertyChanged("Ip");

        //    }
        //}

        public TracertService()
        {
            _innerTracertValue = new ObservableCollection<TracertModel>();
            _tracertValue = new ReadOnlyObservableCollection<TracertModel>(_innerTracertValue);
        }

        public void StartTraceroute()
        {
            NetObserver.PingUtility.IcmpRequestSender utility = new NetObserver.PingUtility.IcmpRequestSender();
            
            var item1 = utility.RequestIcmp("ya.ru");
            var item2 = utility.RequestIcmp("google.com");

            int ping1 = Convert.ToInt32(item1.RoundtripTime.ToString());
            int ping2 = Convert.ToInt32(item2.RoundtripTime.ToString());

            _innerTracertValue.Add(new TracertModel() { Ip = item1.Address.ToString(), Delay = ping1, Status = item1.Status.ToString() });
            _innerTracertValue.Add(new TracertModel() { Ip = item2.Address.ToString(), Delay = ping2, Status = item2.Status.ToString() });


            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(2000).Wait();
                    item1 = utility.RequestIcmp("ya.ru");
                    item2 = utility.RequestIcmp("google.com");

                    ping1 = Convert.ToInt32(item1.RoundtripTime.ToString());
                    ping2 = Convert.ToInt32(item2.RoundtripTime.ToString());

                    _innerTracertValue[0].Delay = ping1;
                    _innerTracertValue[1].Delay = ping2;
                }
            });

            //while (true)
            //{
            //    item1 = utility.RequestIcmp("ya.ru");
            //    item2 = utility.RequestIcmp("google.com");

            //    ping1 = Convert.ToInt32(item1.RoundtripTime.ToString());
            //    ping2 = Convert.ToInt32(item2.RoundtripTime.ToString());

            //    _innerTracertValue[0].Delay = ping1;
            //    _innerTracertValue[1].Delay = ping2;

            //};

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
