using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class TaxClassMoneyModel : Utils.PropertyChangedHelper
    {
        public Utils.Money Value { get { return GetValue(() => Value); } set { SetValue(value); } }

        public Utils.TaxClass TaxClass { get { return GetValue(() => TaxClass); } set { SetValue(value); } }

        public string TaxClassAddition { get { return GetValue(() => TaxClassAddition); } set { SetValue(value); } }
    }
}
