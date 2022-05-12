using NetObserver.PingUtility;
using NetObserver.TracerouteUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
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

        public TracertService()
        {
            _innerTracertValue = new ObservableCollection<TracertModel>();
            _tracertValue = new ReadOnlyObservableCollection<TracertModel>(_innerTracertValue);
        }

        public void StartTraceroute()
        {   

            Traceroute getTracertIp = new Traceroute();
            var objectTracertResult = getTracertIp.GetIpTraceRoute("ya.ru");

            foreach (string addr in objectTracertResult)
            {
                _innerTracertValue.Add(new TracertModel { Ip = addr });
            }

            IcmpRequestSender testRequest = new IcmpRequestSender();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(2000).Wait();

                    foreach(TracertModel itemModel in _innerTracertValue)
                    {   

                        PingReply tmpResult = testRequest.RequestIcmp(itemModel.Ip);
                        itemModel.Delay = (int)tmpResult.RoundtripTime;
                    }

                    OnPropertyChanged();
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
