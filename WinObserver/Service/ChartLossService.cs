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
using System.Text;
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

        private List<Axis> _innerObjectXAxes;
        public readonly List<Axis> _ObjectXAxes;
        private List<string> _collectionTimeXAxes;
        private List<Axis> _innerObjectYAxes;
        public readonly List<Axis> _ObjectYAxes;

        public ChartLossService(ApplicationContext context, LockWay lockWay)
        {
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
                while (IsEndWhile)
                {
                    Task.Delay(500).Wait();
                    if (_lockWay.IsFullingCollectionHost)
                    {
                        AddHostnameChart();
                        IsEndWhile = false;
                    }
                }

                while (true)
                {
                    GetAllTimeXAxes();
                    UpdateValueCollectionLoss();
                    Task.Delay(5000).Wait();
                }
            });
        }

        public void GetAllTimeXAxes()
        {
            var res = _requestTimeRepository.GetAllTime();
            _innerObjectXAxes[0].Labels = res;
        }

        public void AddHostnameChart()
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

        public void UpdateValueCollectionLoss()
        {
            List<Loss> res = _chartLossRepository.GetAllHostInfo();
            //_innerLoss.Clear();
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
            _collectionTimeXAxes = new List<string>() { "00:00:00" };
            _innerObjectXAxes = new List<Axis>
                {
                    new Axis
                    {
                        LabelsRotation = 15,
                        Labels = _collectionTimeXAxes,
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
