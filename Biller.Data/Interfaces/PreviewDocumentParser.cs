using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Interfaces
{
    public interface DocumentParser
    {
        string DocumentType { get; }

        string LocalizedDocumentType { get; }

        bool ParseAdditionalPreviewData(ref dynamic document, XElement data);

        /// <summary>
        /// Used for GetDocument
        /// </summary>
        /// <param name="document"></param>
        /// <param name="data"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        bool ParseAdditionalData(ref Document.Document document, XElement data, Interfaces.IDatabase database);
    }
}
