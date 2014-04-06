using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Interfaces
{
    public interface DocumentExportModel : Interfaces.IXMLStorageable
    {
        Document.PreviewDocument Document { get; }

        Interfaces.IExport GetExport();
    }
}
