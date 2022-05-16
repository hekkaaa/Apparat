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
using System.Threading;
using System.Threading.Tasks;
using WinObserver.Algorithms;
using WinObserver.Model;

namespace WinObserver.Service
{
    public class TracertService : INotifyPropertyChanged
    {
        private ObservableCollection<TracertModel> _innerTracertValue;
        public readonly ReadOnlyObservableCollection<TracertModel> _tracertValue;

        static CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource!.Token;

        public TracertService()
        {
            _innerTracertValue = new ObservableCollection<TracertModel>();
            _tracertValue = new ReadOnlyObservableCollection<TracertModel>(_innerTracertValue);
        }

        public void StartTraceroute(string hostname)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                Traceroute getTracertIp = new Traceroute();
                var objectTracertResult = getTracertIp.GetIpTraceRoute(hostname);

                App.Current.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    _innerTracertValue.Clear();
                    OnPropertyChanged();
                });
                
                int countHostname = 1;

                foreach (string addr in objectTracertResult)
                {
                    App.Current.Dispatcher.BeginInvoke((System.Action)delegate
                    {
                        _innerTracertValue.Add(new TracertModel { NumberHostname = countHostname, Hostname = addr });
                        countHostname++;
                        OnPropertyChanged();
                    });
                }

                while (true)
                {
                    var test = token;
                    Task.Delay(1000).Wait();
                    UpdateStatistic();

                    if (token.IsCancellationRequested)
                    {   
                        _cancellationTokenSource!.Dispose();
                        RestartToken();
                        break;
                    }
                }

            }), token);
        }

        public void StopTraceroute()
        {
            _cancellationTokenSource!.Cancel();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void RestartToken()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            token = _cancellationTokenSource.Token;
        }

        private void UpdateStatistic()
        {
            IcmpRequestSender icmpUtilite = new IcmpRequestSender();

            foreach (TracertModel objectCollection in _innerTracertValue)
            {   
                PingReply tmpResult = icmpUtilite.RequestIcmp(objectCollection.Hostname);
                TracertModel tempValue = objectCollection;

                if (tmpResult.Status == IPStatus.Success)
                {
                    tempValue.LastDelay = (int)tmpResult.RoundtripTime;
                    tempValue.ArhivePingList!.Add((int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.UpdateMinMaxPing(ref tempValue, (int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.MiddlePing(ref tempValue);
                    tempValue.CounterPacket++;
                }
                else
                {
                    tempValue.LastDelay = 0;
                    tempValue.CounterPacket++;
                    tempValue.CounterLossPacket++;
                }

                tempValue.PercentLossPacket = DataGridStatisticAlgorithm.RateLosses(tempValue.CounterPacket, tempValue.CounterLossPacket);
            }
        }
    }
}
