using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class MoneyDescriptionModel : Utils.PropertyChangedHelper
    {
        public Utils.Money Value { get { return GetValue(() => Value); } set { SetValue(value); RaiseUpdateManually("ArticleRecalculation"); } }

        public string Description { get { return GetValue(() => Description); } set { SetValue(value); } }

        public Utils.MathematicalOperation MathematicalOperation { get { return GetValue(() => MathematicalOperation); } set { SetValue(value); RaiseUpdateManually("ArticleRecalculation"); } }
    }
}
