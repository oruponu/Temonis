using System;
using System.Globalization;
using System.Windows.Data;

namespace Temonis.Converters
{
    internal class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.ToString() == parameter.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Enum.Parse(targetType, parameter.ToString());
            return Binding.DoNothing;
        }
    }
}
