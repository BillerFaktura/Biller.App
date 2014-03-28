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
            list.Add(new UI.DocumentView.Contextual.EditTabs.Settings.EditTab());
            list.Add(new UI.DocumentView.Contextual.EditTabs.Receipent.EditTab());
            list.Add(new UI.DocumentView.Contextual.EditTabs.Articles.EditTab());
            list.Add(new UI.DocumentView.Contextual.EditTabs.Others.EditTab());
            list.Add(new UI.DocumentView.Contextual.EditTabs.PrintPreview.EditTab());
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

        public void ReceiveData(object source, Document.Document target)
        {
            if (target is Order)
            {
                if (source is Data.Customers.Customer)
                {
                    (target as Data.Orders.Order).Customer = (source as Data.Customers.Customer);
                    (target as Data.Orders.Order).PaymentMethode = (source as Data.Customers.Customer).DefaultPaymentMethode;
                }

                if (source is Data.Articles.Article)
                {
                    if (target is Data.Orders.Order)
                    {
                        if (!String.IsNullOrEmpty((target as Data.Orders.Order).Customer.MainAddress.OneLineString))
                        {
                            //Check pricegroup
                            var customer = (target as Data.Orders.Order).Customer;
                            var orderedArticle = new Data.Articles.OrderedArticle(source as Data.Articles.Article);
                            orderedArticle.OrderedAmount = 1;
                            orderedArticle.OrderPosition = (target as Data.Orders.Order).OrderedArticles.Count + 1;

                            switch (customer.Pricegroup)
                            {
                                case 0:
                                    orderedArticle.OrderPrice = orderedArticle.Price1;
                                    break;
                                case 1:
                                    orderedArticle.OrderPrice = orderedArticle.Price2;
                                    break;
                                case 2:
                                    orderedArticle.OrderPrice = orderedArticle.Price3;
                                    break;
                            }
                            (target as Data.Orders.Order).OrderedArticles.Add(orderedArticle);

                        }
                        else
                        {
                            var orderedArticle = new Data.Articles.OrderedArticle(source as Data.Articles.Article);
                            orderedArticle.OrderedAmount = 1;
                            orderedArticle.OrderPrice = orderedArticle.Price1;
                            orderedArticle.OrderPosition = (target as Data.Orders.Order).OrderedArticles.Count + 1;
                            (target as Data.Orders.Order).OrderedArticles.Add(orderedArticle);
                        }
                    }
                }
            }
        }
    }
}