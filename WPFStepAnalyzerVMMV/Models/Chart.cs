using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WPFStepAnalyzerVMMV.Models
{
    internal class Chart : INotifyPropertyChanged
    {
        public SeriesCollection getSeries(List<User> list)
        {
            var chartValues = new ChartValues<ObservablePoint>();
            int maxDayId = 0;
            int minDayId = 0;
            int maxDaySteps = list.First().steps;
            int minDaySteps = list.First().steps;
            for (int i = 0; i < list.Count; i++)
            {
                if (maxDaySteps < list[i].steps)
                {
                    maxDaySteps = list[i].steps;
                    maxDayId = i;
                }
                if (minDaySteps > list[i].steps)
                {
                    minDaySteps = list[i].steps;
                    minDayId = i;
                }
                var point = new ObservablePoint(x: i + 1, y: list[i].steps);
                chartValues.Add(point);
            }
            var series = new SeriesCollection()
            {
                new LineSeries()
                {
                    Configuration = new CartesianMapper<ObservablePoint>()
                            .X(point => point.X)
                            .Y(point => point.Y)
                            .Stroke(point => point.Y == maxDaySteps? Brushes.LightGreen : point.Y == minDaySteps?Brushes.DarkRed:Brushes.Blue)
                            .Fill(point => point.Y == maxDaySteps? Brushes.LightGreen : point.Y == minDaySteps?Brushes.DarkRed:Brushes.White),
                            Values = chartValues
                }
            };
            return series;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
