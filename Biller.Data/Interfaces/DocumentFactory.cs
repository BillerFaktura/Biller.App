using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Biller.Data.Interfaces
{
    public interface DocumentFactory
    {
        string DocumentType { get; }

        string LocalizedDocumentType { get; }

        Document.Document GetNewDocument();

        List<UIElement> GetEditContentTabs();

        Fluent.Button GetCreationButton();

        void ReceiveData(object source, Document.Document target);
    }
}
