using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Temonis.Converters;

internal class RadioButtonConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.ToString() == parameter.ToString();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Enum.Parse(targetType, parameter.ToString()) : Binding.DoNothing;

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
