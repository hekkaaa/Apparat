using Apparat.Helpers;
using Apparat.Services.Interfaces;
using Apparat.ViewModel.Interfaces;
using NetObserver.PingUtility;
using NetObserver.TracerouteUtility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WinObserver.Algorithms;
using WinObserver.Model;

namespace WinObserver.Service
{
    public class TracertService : INotifyPropertyChanged, ITracertService
    {
        private ObservableCollection<TracertModel> _innerTracertValue;
        public readonly ReadOnlyObservableCollection<TracertModel> _tracertValue;

        public readonly DataGridModel _gridTracert;
        private readonly Traceroute _tracerouteHelper;

        CancellationTokenSource? _cancellationTokenSource;
        CancellationToken _token;

        public TracertService()
        {
            _innerTracertValue = new ObservableCollection<TracertModel>();
            _tracertValue = new ReadOnlyObservableCollection<TracertModel>(_innerTracertValue);
            _gridTracert = new DataGridModel();
            _tracerouteHelper = new Traceroute();

            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource!.Token;
        }

        public void StartTraceroute(string hostname, IHostViewModel appViewModel)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                try
                {
                    appViewModel.ManagementEnableGeneralControlBtn(false);
                    IEnumerable<string> objectTracertResult = _tracerouteHelper.GetIpTraceRoute(hostname);
                    
                    ClearOldTable();
                    FillingNewtable(objectTracertResult);
                    appViewModel.ManagementEnableGeneralControlBtn(true);

                    while (true)
                    {
                        Task.Delay(1000).Wait();
                        UpdateStatistic();
                        if (_token.IsCancellationRequested)
                        {
                            appViewModel.ManagementEnableGeneralControlBtn(false);
                            _cancellationTokenSource!.Dispose();
                            RestartToken();
                            appViewModel.ManagementEnableGeneralControlBtn(true);
                            break;
                        }
                    }
                }
                catch (PingException)
                {
                    _cancellationTokenSource!.Cancel();
                    _cancellationTokenSource.Dispose();
                    appViewModel.ErrorNameHostname();
                }

            }), _token);
        }

        public void StopTraceroute()
        {
            _cancellationTokenSource!.Cancel();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void RestartToken()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource.Token;
        }

        private void ClearOldTable()
        {
            App.Current.Dispatcher.BeginInvoke((System.Action)delegate
            {
                _innerTracertValue.Clear();
                OnPropertyChanged();
            });
        }

        private void UpdateStatistic()
        {
            IcmpRequestSender icmpUtilite = new IcmpRequestSender();
            int countHop = 0;

            foreach (TracertModel objectCollection in _innerTracertValue)
            {
                PingReply tmpResult = icmpUtilite.RequestIcmp(objectCollection.Hostname, 1500);
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
                countHop++;
            }
        }

        private void FillingNewtable(IEnumerable<string> collectionIpAddres)
        {
            int countHostname = 1;

            foreach (string addres in collectionIpAddres)
            {
                App.Current.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    _innerTracertValue.Add(new TracertModel { NumberHostname = countHostname, Hostname = addres });
                    countHostname++;
                    OnPropertyChanged();
                });
            }
        }
    }
}
