using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Interfaces
{
    /// <summary>
    /// Interface for saving any kind of objects into the database.
    /// </summary>
    public interface IXMLStorageable
    {
        /// <summary>
        /// Returns a XElement we can save to a databasefile
        /// </summary>
        /// <returns></returns>
        XElement GetXElement();

        /// <summary>
        /// Parses a XElement (from <see cref="GetXElement"/>) and writes the values into the current instance.
        /// </summary>
        /// <param name="source"></param>
        void ParseFromXElement(XElement source);

        /// <summary>
        /// Returns the <bold>unique</bold> XElement name inside the database. The database connector will need this field to find your data.
        /// </summary>
        string XElementName { get; }

        string ID { get; }

        /// <summary>
        /// A child inside the object <see cref="XElement"/>. The child needs to be <see cref="XElement"/> as well.
        /// </summary>
        string IDFieldName { get; }

        IXMLStorageable GetNewInstance();
    }
}
