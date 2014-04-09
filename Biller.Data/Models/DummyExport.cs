using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class DummyExport : Interfaces.IExport
    {
        public System.Windows.UIElement PreviewControl
        {
            get
            {
                throw new NotImplementedException("DummyExport");
            }
            set
            {
                throw new NotImplementedException("DummyExport");
            }
        }

        public void RenderDocumentPreview(Document.Document document)
        {
            throw new NotImplementedException("DummyExport");
        }

        public void SaveDocument(Document.Document document, string FilePath, bool OpenOnSuccess = true)
        {
            throw new NotImplementedException("DummyExport");
        }

        public void PrintDocument(Document.Document document)
        {
            throw new NotImplementedException("DummyExport");
        }

        public List<string> AvailableDocumentTypes()
        {
            throw new NotImplementedException("DummyExport");
        }

        public string Name { get; set; }

        public string GuID { get; set; }
    }
}
