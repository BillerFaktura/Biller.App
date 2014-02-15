using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biller.Data.Interfaces
{
    public interface IExport
    {
        UIElement PreviewControl { get; set; }

        void RenderDocumentPreview(Document.Document document);

        void SaveDocument(Document.Document document, string filename, bool OpenOnSuccess = true);

        void PrintDocument(Document.Document document);
    }
}
