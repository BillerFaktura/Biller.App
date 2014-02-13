using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Articles
{
    /// <summary>
    /// OrderedArticle inherits from <see cref="Article"/> and is used for articles inside the <see cref="Order"/> object.
    /// </summary>
    public class OrderedArticle : Article
    {
        /// <summary>
        /// This class inherits from <see cref="Article"/> and is used with orders. It stores additional information like ordered amount, order text.
        /// </summary>
        /// <param name="article">You need to load the corresponding article first so the inherited class can load all additional information.</param>
        public OrderedArticle(Article article)
            : base()
        {
            this.ArticleDescription = article.ArticleDescription;
            this.ArticleID = article.ArticleID;
            this.ArticleText = article.ArticleText;
            this.ArticleUnit = article.ArticleUnit;
            this.TaxClass = article.TaxClass;
            this.Price1 = article.Price1;
            this.Price2 = article.Price2;
            this.Price3 = article.Price3;
            this.ArticleWeight = article.ArticleWeight;
            this.ArticleText = article.ArticleText;
            this.OrderText = article.ArticleText;

            // insert empty values to avoid null exceptions
            OrderArticleID = Guid.NewGuid().ToString(); 
            OrderPrice = new Models.PriceModel(this);

            OrderPrice.Price1.PropertyChanged += Price1_PropertyChanged;
        }

        /// <summary>
        /// Use this to create objects out of the box (e.g. from the saved invoice)
        /// </summary>
        public OrderedArticle()
            : base()
        {
            OrderArticleID = Guid.NewGuid().ToString();
            OrderPrice = new Models.PriceModel(this);
        }

        /// <summary>
        /// Unique ID inside an order.\n
        /// This allows to have many articles of the type in one order.
        /// </summary>
        public string OrderArticleID
        {
            get { return GetValue(() => OrderArticleID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// The quantity of articles the customer ordered.
        /// </summary>
        public double OrderedAmount
        {
            get { return GetValue(() => OrderedAmount); }
            set { SetValue(value); RaiseRecalculateEvents(); }
        }

        public string OrderedAmountString
        {
            get { return ArticleUnit.ValueToString(OrderedAmount); }
            set
            {
                var amount = ArticleUnit.StringToValue(value);
                OrderedAmount = amount;
            }
        }

        /// <summary>
        /// Percentage rebate to <see cref="OrderPrice"/>. Use values between 0 and 1.
        /// </summary>
        public double OrderRebate
        {
            get { return GetValue(() => OrderRebate); }
            set { SetValue(value); RaiseRecalculateEvents(); }
        }

        /// <summary>
        /// The price the customer has to pay.
        /// </summary>
        public Models.PriceModel OrderPrice
        {
            get { return GetValue(() => OrderPrice); }
            set
            {
                if (OrderPrice != null)
                    OrderPrice.Price1.PropertyChanged -= Price1_PropertyChanged;
                SetValue(value);
                OrderPrice.Price1.PropertyChanged += Price1_PropertyChanged;
                RaiseRecalculateEvents(); 
            }
        }

        /// <summary>
        /// Specifies the position of this article inside the articlelist.
        /// </summary>
        public int OrderPosition
        {
            get { return GetValue(() => OrderPosition); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Additional information to the article. They are normally shown below the article name
        /// </summary>
        public string OrderText
        {
            get { return GetValue(() => OrderText); }
            set { SetValue(value); }
        }

        /// <summary>
        /// The order value including taxes.
        /// </summary>
        public Utils.Money RoundedGrossOrderValue
        {
            get
            {
                if (OrderPrice.Price1.IsGross)
                    return new Utils.Money(OrderPrice.Price1.Amount * OrderedAmount * (1 - OrderRebate));
                
                //Net
                return RoundedNetOrderValue + ExactVAT;
            }
        }

        /// <summary>
        /// The net value rounded to 2 decimals calculated from the <see cref="GrossOrderValue"/> and the <see cref="TaxClass"/> of the article.
        /// </summary>
        public Utils.Money RoundedNetOrderValue
        {
            get
            {
                if (!OrderPrice.Price1.IsGross)
                    return new Utils.Money(OrderPrice.Price1.Amount * OrderedAmount * (1 - OrderRebate));

                //Gross
                return RoundedGrossOrderValue - ExactVAT * OrderedAmount;
            }
        }

        /// <summary>
        /// The exact net value calculated from  <see cref="RoundedGrossOrderValue"/> or <see cref="RoundedNetOrderValue"/> depending on the value of <see cref="Utils.EMoney.IsGross"/>.
        /// </summary>
        public double ExactNetOrderValue
        {
            get
            {
                if (!OrderPrice.Price1.IsGross)
                    return RoundedNetOrderValue.Amount;

                //Gross
                return RoundedGrossOrderValue.Amount - ExactVAT * OrderedAmount * (1 - OrderRebate);
            }
        }

        /// <summary>
        /// The exact net value calculated from  <see cref="RoundedGrossOrderValue"/> or <see cref="RoundedNetOrderValue"/> depending on the value of <see cref="Utils.EMoney.IsGross"/>.
        /// </summary>
        public double ExactGrossOrderValue
        {
            get
            {
                if (OrderPrice.Price1.IsGross)
                    return RoundedGrossOrderValue.Amount;

                //Net
                return RoundedNetOrderValue.Amount + ExactVAT * OrderedAmount * (1 - OrderRebate);
            }
        }

        /// <summary>
        /// Returns the exact VAT amount as double.
        /// </summary>
        public double ExactVAT
        {
            get
            {
                if (OrderPrice.Price1.IsGross)
                    return RoundedGrossOrderValue.Amount * (TaxClass.TaxRate.Amount / (1 + TaxClass.TaxRate.Amount));
                
                //Net
                return RoundedNetOrderValue.Amount * (TaxClass.TaxRate.Amount);
            }
        }

        /// <summary>
        /// Trigger to recalculate the ordered value after price has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Price1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount") 
            {
                RaiseRecalculateEvents();
            }
        }

        internal void RaiseRecalculateEvents()
        {
            RaiseUpdateManually("ExactVAT");
            RaiseUpdateManually("ExactGrossOrderValue");
            RaiseUpdateManually("ExactNetOrderValue");
            RaiseUpdateManually("RoundedNetOrderValue");
            RaiseUpdateManually("RoundedGrossOrderValue");
            RaiseUpdateManually("ArticleRecalculation");
        }

        /// <summary>
        /// Parses properties of a XElement to the current object.\n
        /// <b>ATTENTION: Methode does not parse <see cref="TaxClass"/> and <see cref="ArticleUnit"/>. These objects are saved within the source element with their corresponding identifiers.</b>
        /// </summary>
        /// <param name="source"></param>
        public new void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Name of XElement was " + source.Name + " but expected " + XElementName);

            ArticleID = source.Element("ArticleID").Value;
            ArticleDescription = source.Element("ArticleDescription").Value;
            ArticleWeight = double.Parse(source.Element("ArticleWeight").Value, CultureInfo.InvariantCulture);
            OrderText = source.Element("OrderText").Value;
            OrderedAmount = double.Parse(source.Element("OrderedAmount").Value, CultureInfo.InvariantCulture);
            OrderPosition = int.Parse(source.Element("ArticlePosition").Value);
            OrderPrice.ParseFromXElement(source.Element("OrderPrice").Element("PriceGroup"));
            OrderRebate = double.Parse(source.Element("OrderRebate").Value, CultureInfo.InvariantCulture);       
        }

        public new XElement GetXElement()
        {
            return new XElement(XElementName, new XElement("ArticleID", ArticleID), new XElement("ArticleDescription", ArticleDescription),
                         new XElement("OrderText", OrderText), new XElement("OrderedAmount", OrderedAmount),
                         new XElement("ArticlePosition", OrderPosition), new XElement("OrderPrice", OrderPrice.GetXElement()),
                         new XElement("OrderRebate", OrderRebate), new XElement("ArticleWeight", ArticleWeight),
                         new XElement("ArticleUnit", ArticleUnit.Name), new XElement("TaxClass", TaxClass.Name));
        }

        public override bool Equals(object obj)
        {
            if (obj is OrderedArticle)
                if ((obj as OrderedArticle).OrderArticleID == this.OrderArticleID)
                    return true;
            return false;
        }
    }
}
