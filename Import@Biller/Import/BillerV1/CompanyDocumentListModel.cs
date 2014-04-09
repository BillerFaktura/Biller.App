using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Import.BillerV1
{
    public class CompanyDocumentListModel
    {
        /// <summary>
        /// Model to store documents in combination with a specific <see cref="Company"/>.
        /// </summary>
        public CompanyDocumentListModel()
        {
            Documents = new List<Document.Document>();
        }

        /// <summary>
        /// The current company
        /// </summary>
        public Models.CompanyInformation Company { get; set; }

        /// <summary>
        /// The documents associated to the <see cref="Company"/>.
        /// </summary>
        public List<Document.Document> Documents { get; set; }
    }
}
