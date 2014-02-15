using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Biller.Data.Orders;
using System.Windows;

namespace Biller.Data.Factories
{
    public class InvoiceFactory : Interfaces.DocumentFactory
    {
        public string DocumentType { get { return "Invoice"; } }

        public Document.Document GetNewDocument()
        {
            return new Invoice();
        }

        public List<UIElement> GetEditContentTabs()
        {
            var list = new List<UIElement>();
            list.Add(new UI.OrderView.Contextual.EditTabs.Settings.EditTab());
            list.Add(new UI.OrderView.Contextual.EditTabs.Receipent.EditTab());
            list.Add(new UI.OrderView.Contextual.EditTabs.Articles.EditTab());
            list.Add(new UI.OrderView.Contextual.EditTabs.Others.EditTab());
            list.Add(new UI.OrderView.Contextual.EditTabs.PrintPreview.EditTab());
            return list;
        }


        public Fluent.Button GetCreationButton()
        {
            return new Buttons.InvoiceButton();
        }


        public string LocalizedDocumentType
        {
            get 
            {
                var invoice = new Invoice();
                return invoice.LocalizedDocumentType;
            }
        }


        public Interfaces.IExport GetNewExportClass()
        {
            return new Data.PDF.OrderPdfExport();
        }
    }
}
