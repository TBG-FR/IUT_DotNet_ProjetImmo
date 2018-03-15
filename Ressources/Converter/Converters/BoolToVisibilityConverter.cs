using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converter.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Hidden;
            if (value.GetType() != typeof(bool)) return Visibility.Hidden;
            if (!(bool)value) return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(Visibility)) return false;
            switch((Visibility)value)
            {
                case Visibility.Collapsed:
                case Visibility.Hidden:
                    return false;
                case Visibility.Visible:
                    return true;
            }
            return false;
        }
    }
}
