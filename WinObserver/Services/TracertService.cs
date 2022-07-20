using Apparat.Configuration.Events;
using Apparat.Helpers;
using Apparat.Helpers.Interfaces;
using Apparat.Services.Interfaces;
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

        CancellationTokenSource? _cancellationTokenSource;
        CancellationToken _token;

        public TracertService()
        {
            _innerCollectionTracerouteValue = new ObservableCollection<TracertModel>();
            _collectionTracerouteValue = new ReadOnlyObservableCollection<TracertModel>(_innerCollectionTracerouteValue);
            _hostRouteHelper = new HostRouteHelper();
            _updateInfoStatistic = new UpdateStatisticOfTracerouteElementsHelper();
            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource!.Token;
        }

        public void StartStreamTracerouteHost(string hostname, IHostViewModelEvents hostViewEvent)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                try
                {
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(true);
                    hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(false);

                    ClearOldTable();
                    IEnumerable<string> createNewRouteList = _hostRouteHelper.CreateNewRouteCollection(hostname);
                    _hostRouteHelper.FillingNewRoute(ref _innerCollectionTracerouteValue, createNewRouteList);

                    hostViewEvent.ManagementEnableGeneralControlBtnEventAndPreloaderVisible(true);

                    while (true)
                    {
                        Task.Delay(1000).Wait();
                        _innerCollectionTracerouteValue = _updateInfoStatistic.Update(_innerCollectionTracerouteValue);
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
                catch (PingException)
                {
                    _cancellationTokenSource!.Cancel();
                    _cancellationTokenSource.Dispose();
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(false);
                    hostViewEvent.ErrorNameHostnameEvent();
                }
                catch (Exception)
                {
                    _cancellationTokenSource!.Cancel();
                    _cancellationTokenSource.Dispose();
                    hostViewEvent.WorkingProggresbarInListBoxHostnameEvent(false);
                    hostViewEvent.ErrorNameHostnameEvent();
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
    }
}
