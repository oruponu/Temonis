﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Temonis.Converters
{
    internal class LevelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => MainWindow.Instance.FindResource(((Level)value).ToString());

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
