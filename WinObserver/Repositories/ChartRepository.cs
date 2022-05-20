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
        public ReadOnlyObservableCollection<ISeries> _loss;

        public ChartRepository()
        {
            _innerLoss = new ObservableCollection<ISeries>();
            _loss = new ReadOnlyObservableCollection<ISeries>(_innerLoss);
        }

        public void AddHops(string hostname)
        {
            var newHost = new LineSeries<double>
            {
                GeometryStroke = null,
                GeometryFill = null,
                Values = new List<double>(),
                Name = hostname
            };

            _innerLoss.Add(newHost);
        }

        public void AddValueLossCollection(int numberHop, double newValue)
        {
            List<double> tmp = (List<double>)_innerLoss[numberHop].Values;
            tmp.Add(newValue);
            _innerLoss[numberHop].Values = tmp;
        }

    }
}
