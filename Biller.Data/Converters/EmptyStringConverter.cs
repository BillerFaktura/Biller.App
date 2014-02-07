using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Data;

namespace Biller.Data.Converters
{
    /// <summary>
    /// EmptyStringConverter checks if a given String is null or empty and returns "true" if so.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class EmptyStringConverter : IValueConverter
    {

        #region "IValueConverter Members"
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
                return String.IsNullOrEmpty(value as string);
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
