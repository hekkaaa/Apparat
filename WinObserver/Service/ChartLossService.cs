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
        private ObservableCollection<ISeries> _innerLoss;
        public readonly ReadOnlyObservableCollection<ISeries> _lossList;

        private readonly RequestTimeRepository _requestTimeRepository;
        private readonly ChartLossRepository _chartLossRepository;
        private readonly ApplicationContext _applicationContext;

        private List<Axis> _innerObjectXAxes;
        public readonly List<Axis> _ObjectXAxes;
        private List<string> _collectionTimeXAxes;
        private List<Axis> _innerObjectYAxes;
        public readonly List<Axis> _ObjectYAxes;

        public ChartLossService(ApplicationContext context)
        {   
            _applicationContext = context;
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
                while (true)
                {
                    Task.Delay(5000).Wait();
                    GetAllTimeXAxes();
                }
                
            });
        }

        public void GetAllTimeXAxes()
        {
            var res = _requestTimeRepository.GetAllTime();
            _innerObjectXAxes[0].Labels = res;
        }

        public void GetAllLoss()
        {
            List<Loss> res = _chartLossRepository.GetAllHostInfo();

            foreach (Loss loss in res)
            {
                string[] tokens = loss.ListLoss.Split(',');

                int[] myItems = Array.ConvertAll<string, int>(tokens, int.TryParse);

                var s = "sada";
                //var newHost = new LineSeries<double>
                //{
                //    GeometryStroke = null,
                //    GeometryFill = null,
                //    Values = new List<double>() { 0 },
                //    Name = loss.Hostname
                //};

                //_innerLoss.Add(newHost);
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
