using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class KeyValueModel : Utils.PropertyChangedHelper
    {
        public KeyValueModel() { }

        public KeyValueModel(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get { return GetValue(() => Key); } set { SetValue(value); } }

        public object Value { get { return GetValue(() => Value); } set { SetValue(value); } }

        public override bool Equals(object obj)
        {
            if (obj is KeyValueModel)
                if ((obj as KeyValueModel).Key == this.Key)
                    return true;
            return false;
        }
    }
}
