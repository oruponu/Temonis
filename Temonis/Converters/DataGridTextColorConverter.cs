using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Temonis.Converters
{
    internal class DataGridTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return (SolidColorBrush)MainWindow.Instance.FindResource("White");

            return (SolidColorBrush)MainWindow.Instance.FindResource("Black");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
