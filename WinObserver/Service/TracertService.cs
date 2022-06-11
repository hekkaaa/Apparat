using Apparat.Service;
using Apparat.Service.Interface;
using Data.Repositories.Connect;
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
using WinObserver.Repositories.Interface;
using WinObserver.ViewModel;

namespace WinObserver.Service
{
    public class TracertService : INotifyPropertyChanged
    {
        private ObservableCollection<TracertModel> _innerTracertValue;
        public readonly ReadOnlyObservableCollection<TracertModel> _tracertValue;

        public readonly DataGridModel _gridTracert;
        private readonly Traceroute _tracerouteHelper;
        private readonly IChartRepository _chartRepository;
        private readonly IChartLossService _chartLossService;
        private readonly ApplicationContext _applicationContext;

        static CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource!.Token;

        public TracertService(IChartRepository chartService, ApplicationContext context)
        {
            _applicationContext = context;
            _innerTracertValue = new ObservableCollection<TracertModel>();
            _tracertValue = new ReadOnlyObservableCollection<TracertModel>(_innerTracertValue);
            _gridTracert = new DataGridModel();
            _tracerouteHelper = new Traceroute();
            _chartLossService = new ChartLossService(_applicationContext);
            _chartRepository = chartService;
        }

        public void StartTraceroute(string hostname, ApplicationViewModel applicationViewModel)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                try
                {
                    IEnumerable<string> objectTracertResult = _tracerouteHelper.GetIpTraceRoute(hostname);

                    ClearOldTable();
                    FillingNewtable(objectTracertResult);
                    _chartRepository.ClearChart();

                    while (true)
                    {
                        Task.Delay(1000).Wait();
                        UpdateStatistic();
                        if (token.IsCancellationRequested)
                        {
                            _cancellationTokenSource!.Dispose();
                            RestartToken();
                            break;
                        }
                    }
                }
                catch (PingException)
                {
                    _cancellationTokenSource!.Cancel();
                    applicationViewModel.ErrorValidationTextAndAnimation();
                }

            }), token);
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
            token = _cancellationTokenSource.Token;
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
            _chartRepository.UpdateTimeXAxes();

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
                AddLossChart(countHop, tempValue.PercentLossPacket);
                _chartLossService.UpdateLoss(tempValue);
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
                    FillingNameChart(addres);
                    _chartLossService.AddHostname(addres); // here!
                    _innerTracertValue.Add(new TracertModel { NumberHostname = countHostname, Hostname = addres });
                    countHostname++;
                    OnPropertyChanged();
                });
            }
        }


        private void FillingNameChart(string name)
        {
            _chartRepository.AddHops(name);
        }

        private void AddLossChart(int count, double loss)
        {
            _chartRepository.AddValueLossCollection(count, loss);
        }
    }
}
