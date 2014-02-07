using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Biller.Data.Converters
{
    /// <summary>
    /// Converts a double to an string containing the percentage symbol ('%'). Also supports <see cref="ConvertBack"/>.
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentageConverter : IValueConverter
    {
        #region "IValueConverter Members"
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                var temp = double.Parse(value.ToString());
                return temp.ToString("0.00 %");
            }
            return "0,00 %";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double temp;
            if (double.TryParse((value.ToString()).Replace("%", "").Replace(",", ".").Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                return temp / 100;
            return 0;
        }
        #endregion
    }
}
