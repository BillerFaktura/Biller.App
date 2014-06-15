using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Interfaces
{
    public interface IStorageable
    {
        string ID { get; }

        /// <summary>
        /// Returns a new instance of the object
        /// </summary>
        /// <returns></returns>
        IXMLStorageable GetNewInstance();
    }
}
