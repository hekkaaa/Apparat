using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinObserver.Repositories.Interface;

namespace WinObserver.Repositories
{
    public class ChartRepository : IChartRepository
    {
        private ObservableCollection<ISeries> _innerLoss;
        public readonly ReadOnlyObservableCollection<ISeries> _lossList;

        private List<Axis> _innerObjectXAxes;
        public readonly List<Axis> _ObjectXAxes;
        private List<string> _collectionTimeXAxes;

        public ChartRepository()
        {
            _innerLoss = new ObservableCollection<ISeries>();
            _lossList = new ReadOnlyObservableCollection<ISeries>(_innerLoss);

            _collectionTimeXAxes = new List<string>();
            _innerObjectXAxes = new List<Axis>
                {
                    new Axis
                    {
                        LabelsRotation = 15,
                        Labels = _collectionTimeXAxes,
                    }
                };
            
            _ObjectXAxes = new List<Axis>(_innerObjectXAxes);
           
        }

        public void AddHops(string hostname)
        {
            var newHost = new LineSeries<double>
            {
                GeometryStroke = null,
                GeometryFill = null,
                Values = new List<double>() { 0 },
                Name = hostname
            };

            _innerLoss.Add(newHost);
        }

        public void AddValueLossCollection(int numberHop, double newValueLoss)
        {
            List<double>? tmpCollectionLoss = (List<double>)_innerLoss[numberHop].Values;

            if (tmpCollectionLoss == null) 
            {
                tmpCollectionLoss.Add(100);
                _innerLoss[numberHop].Values = tmpCollectionLoss;
            }
            else
            {
                tmpCollectionLoss.Add(newValueLoss);
                _innerLoss[numberHop].Values = tmpCollectionLoss;
            }
        }

        public void CreateNewDatetimeValueXAxes()
        {
            _innerObjectXAxes = new List<Axis>
                {
                    new Axis
                    {
                        LabelsRotation = 15,
                        Labels = _collectionTimeXAxes,
                    }
                };
        }
        public void AddTimeXAxes()
        {
            DateTime date1 = DateTime.Now;
            _collectionTimeXAxes.Add(date1.ToString("T"));
            _innerObjectXAxes[0].Labels = _collectionTimeXAxes;
        }

    }
}
