﻿using ProjetImmo.Core.Models;
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
    public class EstateSaleStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(Estate)) return null;

            return EstateSaleStatusToBool((Estate)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null; // UNIMPLEMENTED
            if (value.GetType() != typeof(bool)) return null; // UNIMPLEMENTED

            //return BoolToEstateSaleStatus((bool)value);
            return null; // UNIMPLEMENTED
        }

        public static bool EstateSaleStatusToBool(Estate estate)
        {
            if (estate.Transactions == null || estate.Transactions.Count == 0) { return false; }
            if (estate.Transactions.Last().GetType().Equals(typeof(SaleTransaction)) && estate.Transactions.Last().TransactionDate == null) { return true; }
            else { return false; }
        }

        public static Estate BoolToEstateSaleStatus(bool SaleStatus)
        {

            return null; // UNIMPLEMENTED

        }
    }
}

