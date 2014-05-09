using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Utils
{
    public class KeyValueStore : ObservableCollection<Models.KeyValueModel>
    {
        /// <summary>
        /// Returns an existing <see cref="Models.KeyValueModel"/> from the collection or null if there is no item with the given key.
        /// </summary>
        /// <param name="key">A unique key inside the database</param>
        /// <returns></returns>
        public Models.KeyValueModel GetByKey(string key)
        {
            return this.FirstOrDefault(x => x.Key == key);
        }
    }
}
