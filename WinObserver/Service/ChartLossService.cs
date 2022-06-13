using Apparat.Helpers;
using Apparat.Service.Interface;
using Data.Entities;
using Data.Repositories;
using Data.Repositories.Connect;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinObserver.Model;

namespace Apparat.Service
{
    public class ChartLossService : IChartLossService
    {   
        private bool IsEndWhile = true;
        private ObservableCollection<ISeries> _innerLoss;
        public readonly ReadOnlyObservableCollection<ISeries> _lossList;

        private readonly RequestTimeRepository _requestTimeRepository;
        private readonly ChartLossRepository _chartLossRepository;
        private readonly ApplicationContext _applicationContext;
        private readonly LockWay _lockWay;

        static CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource!.Token;

        private List<Axis> _innerObjectXAxes;
        public readonly List<Axis> _ObjectXAxes;
        private List<Axis> _innerObjectYAxes;
        public readonly List<Axis> _ObjectYAxes;

        public ChartLossService(ApplicationContext context, LockWay lockWay)
        {
            //_applicationContext = new ApplicationContext();
            _applicationContext = context;
            _lockWay = lockWay;
            _chartLossRepository = new ChartLossRepository(_applicationContext);
            _requestTimeRepository = new RequestTimeRepository(_applicationContext);
            DefaultValuesForViewChart();

            _innerLoss = new ObservableCollection<ISeries>();
            _lossList = new ReadOnlyObservableCollection<ISeries>(_innerLoss);

            _ObjectXAxes = new List<Axis>(_innerObjectXAxes);
            _ObjectYAxes = new List<Axis>(_innerObjectYAxes);
        }

        public void StartUpdateChart()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (IsEndWhile)
                    {
                        Task.Delay(500).Wait();
                        if (_lockWay.IsFullingCollectionHost)
                        {
                            AddHostNameChart();
                            IsEndWhile = false;
                        }
                    }

                    while (true)
                    {   
                        GetAllTimeXAxes();
                        UpdateValueCollectionLoss();
                        Task.Delay(5000).Wait();
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
                }
            });
        }

        public void StopUpdateChart()
        {
            _cancellationTokenSource!.Cancel();
        }

        private void RestartToken()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            token = _cancellationTokenSource.Token;
        }

        private void GetAllTimeXAxes()
        {
            var res = _requestTimeRepository.GetAllTime();
            _innerObjectXAxes[0].Labels = res;
        }

        private void AddHostNameChart()
        {
            List<Loss> hostList = _chartLossRepository.GetAllHostInfo();
            foreach(Loss loss in hostList)
            {
                var newHost = new LineSeries<double>
                {
                    GeometryStroke = null,
                    GeometryFill = null,
                    Values = new List<double>() { 0 },
                    Name = loss.Hostname
                };

                _innerLoss.Add(newHost);
            }
        }

        private void UpdateValueCollectionLoss()
        {
            List<Loss> res = _chartLossRepository.GetAllHostInfo();
            
            foreach (Loss loss in res)
            {   
                if(loss.ListLoss is null)
                {
                    continue;
                }
                else
                {
                    string[] tokens = loss.ListLoss.Split(',');
                    tokens[0] = "0"; // Correct bug null.
                    double[] myItems = Array.ConvertAll<string, double>(tokens, double.Parse);

                    _innerLoss[loss.Id - 1].Values = myItems;
                }
            }
        }

        private void DefaultValuesForViewChart()
        {
            _innerObjectXAxes = new List<Axis>
                {
                    new Axis
                    {
                        LabelsRotation = 15,
                        Labels = new List<string>() { "00:00:00" },
                    }
                };

            _innerObjectYAxes = new List<Axis>
            {
                 new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 100,
                    MinStep = 10,
                }
            };
        }
    }
}
