using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converter.Converters
{
    class BoolToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return Visibility.Hidden;
            if (values.Length == 0) return Visibility.Hidden;
            if (values.Length == 1)
            {
                BoolToVisibilityConverter c = new BoolToVisibilityConverter();
                return c.Convert(values[0], targetType, parameter, culture);
            }

            bool[] b = new bool[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                    b[i] = false;
                else if (values[i].GetType() != typeof(bool))
                    b[i] = false;
                else
                    b[i] = (bool)values[i];
            }

            bool result = true;
            for (int i = 0; i < b.Length; i++)
            {
                result &= b[i];
            }

            if (result) return Visibility.Visible;

            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
