using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Biller.Data.Converters
{
    /// <summary>
    /// EmptyListConverter checks if a given List is empty returns "true" if so.
    /// </summary>
    [ValueConversion(typeof(IEnumerable), typeof(bool))]
    public class EmptyListConverter : IValueConverter
    {
        #region "IValueConverter Members"
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable)
            {
                IList collection = (IList)value;
                return collection.Count == 0 ? true : false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
