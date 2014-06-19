using Biller.Data.Utils;
using NLog;
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
    /// Converts a double to specific decimal format with a unit
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class kgConverter : IValueConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Unit kgUnit { get; private set; }

        public kgConverter()
        {
            if (kgConverter.kgUnit == null)
                kgConverter.kgUnit = new Unit() { DecimalDigits = 3, DecimalSeperator = ",", Name = "Kilogramm", ShortName = "kg", ThousandSeperator = "" };
        }

        #region "IValueConverter Members"
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var val = (double)value;
                return kgConverter.kgUnit.ValueToString(val);
            }
            catch (Exception e)
            {
                logger.ErrorException("Error converting " + value.ToString(), e);
            }
            return value;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return kgConverter.kgUnit.StringToValue(value.ToString());
            }
            catch (Exception e)
            {
                logger.ErrorException("Error converting back from " + value.ToString(), e);
            }
            return 0;
        }
        #endregion
    }
}
