using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Temonis.Converters
{
    internal class SliderConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ticks = -(int)(double)value;
            var hour = ticks / 3600;
            var minute = ticks % 3600 / 60;
            var second = ticks % 60;

            var text = "";
            if (hour > 0)
                text += $"{hour.ToString()}時間";
            if (minute > 0)
                text += $"{minute.ToString()}分";
            if (second > 0)
                text += $"{second.ToString()}秒";
            if (text.Length != 0)
                text += "前";

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
