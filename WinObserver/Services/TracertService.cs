using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Helpers.Interfaces;
using Apparat.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using WinObserver.Model;

namespace WinObserver.Service
{
    public class TracertService : ITracertService
    {
        private ObservableCollection<TracertModel> _innerCollectionTracerouteValue;
        public readonly ReadOnlyObservableCollection<TracertModel> _collectionTracerouteValue;
        private readonly IHostRouteHelper _hostRouteHelper;
        private readonly IUpdateStatisticOfTracerouteElementsHelper _updateInfoStatistic;
        private int _delayValue = 1000;
        private int _sizePacket = 32;
        ILogger _logger;

        CancellationTokenSource? _cancellationTokenSource;
        CancellationToken _token;

        public List<string> ArhiveTimeRequest { get; set; }

        public TracertService(ILogger logger)
        {
            _logger = logger;
            _innerCollectionTracerouteValue = new ObservableCollection<TracertModel>();
            _collectionTracerouteValue = new ReadOnlyObservableCollection<TracertModel>(_innerCollectionTracerouteValue);
            _hostRouteHelper = new HostRouteHelper();
            _updateInfoStatistic = new UpdateStatisticOfTracerouteElementsHelper();
            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource!.Token;

        }

        public void StartStreamTracerouteHost(string hostname, IHostViewModelEvents hostViewEvent, int delay)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                try
                {
                    _delayValue = delay;
                    ArhiveTimeRequest = new List<string>(); // Create Time list.
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(true);
                    hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(false);

                    ClearOldTable();
                    IEnumerable<string> createNewRouteList = _hostRouteHelper.CreateNewRouteCollection(hostname);
                    _hostRouteHelper.FillingNewRoute(ref _innerCollectionTracerouteValue, createNewRouteList);

                    hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(true);

                    while (true)
                    {
                        Task.Delay(_delayValue).Wait();
                        _innerCollectionTracerouteValue = _updateInfoStatistic.Update(_innerCollectionTracerouteValue, _sizePacket);
                        ArhiveTimeRequest.Add(DataTimeUpdateStatistic());
                        if (_token.IsCancellationRequested)
                        {
                            hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(false);
                            hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(false);
                            _cancellationTokenSource!.Dispose();
                            RestartToken();
                            hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(true);
                            break;
                        }
                    }
                }
                catch (PingException ex)
                {
                    _cancellationTokenSource!.Cancel();
                    _cancellationTokenSource.Dispose();
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(false);
                    hostViewEvent.ErrorNameHostnameEvent();
                    _logger.LogError($"Error PingException in Hostname: {hostname} | ERROR: {ex.Message}" + $" \n | {ex.InnerException}");
                }
                catch (Exception ex)
                {
                    _cancellationTokenSource!.Cancel();
                    _cancellationTokenSource.Dispose();
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(false);
                    hostViewEvent.ErrorNameHostnameEvent();
                    _logger.LogError($"Error Exception in Hostname: {hostname} | ERROR: {ex.Message}" + $" \n | {ex.InnerException}");
                }

            }), _token);
        }

        public void StopStreamTracerouteHost()
        {
            _cancellationTokenSource!.Cancel();
        }

        public ReadOnlyObservableCollection<TracertModel> GetActualCollectionTracertValue()
        {
            return _collectionTracerouteValue;
        }

        public List<string> GetArhiveTimeRequestCollection()
        {
            return ArhiveTimeRequest;
        }

        /// <summary>
        /// Returns the time in milliseconds used between requests to update statistics.
        /// </summary>
        /// <returns></returns>
        public int GetDelayValue()
        {
            return _delayValue;
        }

        public void UpdateDelayValue(int newDelay)
        {
            _delayValue = newDelay;
        }

        public int GetSizePacketValue()
        {
            return _sizePacket;
        }

        public void UpdateSizePacketValue(int newSize)
        {
            _sizePacket = newSize;
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
                _innerCollectionTracerouteValue.Clear();
            });
        }

        private string DataTimeUpdateStatistic()
        {
            DateTime date1 = DateTime.Now;
            return date1.ToString("T");
        }
    }
}
