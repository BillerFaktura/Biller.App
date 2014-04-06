using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biller.Data;
using OrderTypes_Biller.Calculations;
using Biller.Data.Document;

namespace OrderTypes_Biller.Order
{
    /// <summary>
    /// Abstract class to implement various types of orders like <see cref="Invoice"/>, Offer, Delivery Note, ...
    /// 
    /// </summary>
    public abstract class Order : Biller.Data.Document.Document
    {
        public Order()
        {
            // insert empty values to avoid null exceptions.
            OrderedArticles = new ObservableCollection<Biller.Data.Articles.OrderedArticle>();
            DocumentID = ""; OrderOpeningText = ""; OrderClosingText = ""; Customer = new Biller.Data.Customers.Customer(); Date = DateTime.Now;
            OrderRebate = new Biller.Data.Utils.Percentage();
            OrderShipment = new Biller.Data.Utils.Shipment();
            PaymentMethode = new Biller.Data.Utils.PaymentMethode();
            OrderCalculation = new DefaultOrderCalculation(this);
        }

        /// <summary>
        /// Default implementation for storing the <see cref="OrderedArticles"/>.
        /// </summary>
        public virtual ObservableCollection<Biller.Data.Articles.OrderedArticle> OrderedArticles
        {
            get { return GetValue(() => OrderedArticles); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Default implementation for storing the <see cref="OrderOpeningText"/>.
        /// </summary>
        public virtual string OrderOpeningText
        {
            get { return GetValue(() => OrderOpeningText); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Default implementation for storing the <see cref="OrderClosingText"/>.
        /// </summary>
        public virtual string OrderClosingText
        {
            get { return GetValue(() => OrderClosingText); }
            set { SetValue(value); }
        }

        public virtual Biller.Data.Customers.Customer Customer
        {
            get { return GetValue(() => Customer); }
            set { SetValue(value); }
        }

        public virtual DefaultOrderCalculation OrderCalculation
        {
            get { return GetValue(() => OrderCalculation); }
            set { SetValue(value); }
        }

        public virtual Biller.Data.Utils.PaymentMethode PaymentMethode
        {
            get { return GetValue(() => PaymentMethode); }
            set { SetValue(value); }
        }

        public virtual Biller.Data.Utils.Percentage OrderRebate
        {
            get { return GetValue(() => OrderRebate); }
            set { SetValue(value); }
        }

        public virtual Biller.Data.Utils.Shipment OrderShipment
        {
            get { return GetValue(() => OrderShipment); }
            set { SetValue(value); }
        }

        public virtual DateTime DateOfDelivery
        {
            get { return GetValue(() => DateOfDelivery); }
            set { SetValue(value); }
        }

        public override abstract string DocumentType { get; }

        public override abstract System.Xml.Linq.XElement GetXElement();

        public override abstract void ParseFromXElement(System.Xml.Linq.XElement source);

        public static Biller.Data.Document.PreviewDocument PreviewFromOrder(Order source)
        {
            dynamic preview = new Biller.Data.Document.PreviewDocument(source.DocumentType);
            preview.DocumentID = source.DocumentID;
            preview.Date = source.Date;
            preview.Customer = source.Customer.DisplayName;
            preview.Value = source.OrderCalculation.OrderSummary;
            preview.LocalizedDocumentType = source.LocalizedDocumentType;

            return preview;
        }
    }
}
