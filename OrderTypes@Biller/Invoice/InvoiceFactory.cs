using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;

namespace OrderTypes_Biller.Invoice
{
    public class InvoiceFactory : Biller.Data.Interfaces.DocumentFactory
    {
        public string DocumentType { get { return "Invoice"; } }

        public Biller.Data.Document.Document GetNewDocument()
        {
            return new Invoice();
        }

        public List<UIElement> GetEditContentTabs()
        {
            var list = new List<UIElement>();
            list.Add(new Biller.UI.DocumentView.Contextual.EditTabs.Settings.EditTab());
            list.Add(new Biller.UI.DocumentView.Contextual.EditTabs.Receipent.EditTab());
            list.Add(new Biller.UI.DocumentView.Contextual.EditTabs.Articles.EditTab());
            list.Add(new Biller.UI.DocumentView.Contextual.EditTabs.Others.EditTab());
            list.Add(new Biller.UI.DocumentView.Contextual.EditTabs.PrintPreview.EditTab());
            return list;
        }

        public Fluent.Button GetCreationButton()
        {
            return new Button();
        }


        public string LocalizedDocumentType
        {
            get 
            {
                var invoice = new Invoice();
                return invoice.LocalizedDocumentType;
            }
        }

        /// <summary>
        /// Works with the <see cref="source"/> when <see cref="target"/> is type of <see cref="Order"/>.
        /// </summary>
        /// <param name="source">When source is type of <see cref="Biller.Data.Customers.Customer"/> or <see cref="Biller.Data.Articles.Article"/> the data will be passed to the target <see cref="Order.Order"/>.</param>
        /// <param name="target"><see cref="source"/> will be passed to <see cref="target"/> if it's type of <see cref="Order.Order"/>.</param>
        public void ReceiveData(object source, Biller.Data.Document.Document target)
        {
            if (target is Order.Order)
            {
                if (source is Biller.Data.Customers.Customer)
                {
                    (target as Order.Order).Customer = (source as Biller.Data.Customers.Customer);
                    (target as Order.Order).PaymentMethode = (source as Biller.Data.Customers.Customer).DefaultPaymentMethode;
                }

                if (source is Biller.Data.Articles.Article)
                {
                    if (target is Order.Order)
                    {
                        if (!String.IsNullOrEmpty((target as Order.Order).Customer.MainAddress.OneLineString))
                        {
                            //Check pricegroup
                            var customer = (target as Order.Order).Customer;
                            var orderedArticle = new Biller.Data.Articles.OrderedArticle(source as Biller.Data.Articles.Article);
                            orderedArticle.OrderedAmount = 1;
                            orderedArticle.OrderPosition = (target as Order.Order).OrderedArticles.Count + 1;

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
                            (target as Order.Order).OrderedArticles.Add(orderedArticle);

                        }
                        else
                        {
                            var orderedArticle = new Biller.Data.Articles.OrderedArticle(source as Biller.Data.Articles.Article);
                            orderedArticle.OrderedAmount = 1;
                            orderedArticle.OrderPrice = orderedArticle.Price1;
                            orderedArticle.OrderPosition = (target as Order.Order).OrderedArticles.Count + 1;
                            (target as Order.Order).OrderedArticles.Add(orderedArticle);
                        }
                    }
                }
            }
        }


        public Biller.Data.Document.PreviewDocument GetPreviewDocument(Biller.Data.Document.Document source)
        {
            return Order.Order.PreviewFromOrder(source as Order.Order);
        }
    }
}