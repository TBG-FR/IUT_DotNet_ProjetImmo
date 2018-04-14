using ProjetImmo.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ProjetImmo.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolToInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(bool)) return null;

            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(bool)) return null;

            return !((bool)value);
            //throw new NotSupportedException();
        }
        
    }
}

