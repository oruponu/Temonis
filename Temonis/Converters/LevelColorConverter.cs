using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Temonis.Converters;

internal class LevelColorConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => MainWindow.Instance.FindResource(((Level)value).ToString());

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
