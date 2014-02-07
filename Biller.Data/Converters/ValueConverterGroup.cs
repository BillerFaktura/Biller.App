using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Biller.Data.Converters
{
    /// <summary>
    /// This class allows the combination of multiple converters in one resource. This allows you to create complex converters for XAML.
    /// <example>This example shows how to define a ValueConverterGroup in XAML
    /// <code>
    /// <pre>
    /// <convert:ValueConverterGroup x:Key="ConverterGroup">
    ///     <convert:EmptyStringConverter/>
    ///     <convert:InverseBooleanConverter/>
    /// </convert:ValueConverterGroup>
    /// </pre>
    /// </code>
    /// </example>
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region "IValueConverter Members"

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
