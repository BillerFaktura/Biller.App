using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Orders
{
    /// <summary>
    /// Explicit implementation of <see cref="Order"/>.\n
    /// This class implements invoices.
    /// </summary>
    public class Invoice : Order
    {
        public Invoice()
            : base()
        {
        }

        public override string DocumentType
        {
            get { return "Invoice"; }
        }

        public override string LocalizedDocumentType
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "de":
                        return "Rechnung";

                    default:
                        return base.LocalizedDocumentType;
                }
            }
        }

        /// <summary>
        /// Does not parse <see cref="Customer"/> and <see cref="OrderedArticles"/>!! This will be handled by <see cref="InvoiceParser"/>
        /// </summary>
        /// <param name="source"></param>
        public override void ParseFromXElement(XElement source)
        {
            if (source.Name != DocumentType)
                throw new Exception("Can not parse " + source.Name + " inside the " + DocumentType + "-class");
            DocumentID = source.Element("ID").Value;
            Date = DateTime.Parse(source.Element("Date").Value);
            DateOfDelivery = DateTime.Parse(source.Element("DateOfDelivery").Value);
            OrderOpeningText = source.Element("OrderOpeningText").Value;
            OrderClosingText = source.Element("OrderClosingText").Value;
            
        }

        public override XElement GetXElement()
        {
            var element = new XElement(XElementName);
            element.Add(new XElement("ID", DocumentID), new XElement("CustomerID", Customer.CustomerID),
                new XElement("Date", Date), new XElement("DateOfDelivery", DateOfDelivery),
                new XElement("OrderOpeningText", OrderOpeningText), new XElement("OrderClosingText", OrderClosingText),
                new XElement("OrderedArticles",
                    from articles in this.OrderedArticles
                    select articles.GetXElement()),
                new XElement("PreviewValue", OrderCalculation.OrderSummary.GetXElement()), new XElement("PreviewCustomer", Customer.DisplayName), OrderShipment.GetXElement(), PaymentMethode.GetXElement());
            if (DeliveryAddress != null)
                element.Add(DeliveryAddress.GetXElement());

            return element;
        }

        public Utils.EAddress DeliveryAddress
        {
            get { return GetValue(() => DeliveryAddress); }
            set { SetValue(value); }
        }

        public override Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Invoice();
        }

        public override string IDFieldName
        {
            get { return "ID"; }
        }
    }
}
