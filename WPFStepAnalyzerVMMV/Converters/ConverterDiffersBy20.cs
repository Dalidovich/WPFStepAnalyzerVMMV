using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFStepAnalyzerVMMV.Converters
{
    public class ConverterDiffersBy20 : IMultiValueConverter
	{
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var avgSteps = (int)values[0];
            var bestResult=(int)values[1];
            var worseResult = (int)values[2];
            return (bestResult / (avgSteps / 100) > 120) || (worseResult / (avgSteps / 100) < 80) ? Brushes.LightGoldenrodYellow : Brushes.White;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
