using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Models
{
    public class PriceModel : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        private Articles.Article _parentArticle;

        public PriceModel(Articles.Article parentArticle)
        {
            Price1 = new Utils.EMoney(0);
            Price2 = new Utils.EMoney(0, false);
            _parentArticle = parentArticle;
            Price1.PropertyChanged += Price1_PropertyChanged;
            Price2.PropertyChanged += Price2_PropertyChanged;
        }

        void Price2_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount" || e.PropertyName == "IsGross" && _parentArticle.TaxClass.Name != "")
            {
                if (Price2.IsGross == true)
                {
                    Price1.PropertyChanged -= Price1_PropertyChanged;
                    Price1.IsGross = false;
                    Price1.Amount = Price2.Amount / (1 + _parentArticle.TaxClass.TaxRate.Amount);
                    Price1.PropertyChanged += Price1_PropertyChanged;
                }
                else
                {
                    Price1.PropertyChanged -= Price1_PropertyChanged;
                    Price1.IsGross = false;
                    Price1.Amount = Price2.Amount * (1 + _parentArticle.TaxClass.TaxRate.Amount);
                    Price1.PropertyChanged += Price1_PropertyChanged;
                }
            }
        }

        void Price1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount" || e.PropertyName == "IsGross")
            {
                if (Price1.IsGross == true)
                {
                    Price2.PropertyChanged -= Price2_PropertyChanged;
                    Price2.IsGross = false;
                    Price2.Amount = Price1.Amount / (1 + _parentArticle.TaxClass.TaxRate.Amount);
                    Price2.PropertyChanged += Price2_PropertyChanged;
                }
                else
                {
                    Price2.PropertyChanged -= Price2_PropertyChanged;
                    Price2.IsGross = false;
                    Price2.Amount = Price1.Amount * (1 + _parentArticle.TaxClass.TaxRate.Amount);
                    Price2.PropertyChanged += Price2_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// Represents the price of.
        /// </summary>
        public Utils.EMoney Price1
        {
            get { return GetValue(() => Price1); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the price of.
        /// </summary>
        public Utils.EMoney Price2
        {
            get { return GetValue(() => Price2); }
            set { SetValue(value); }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new XElement(XElementName, Price1.GetXElement(), Price2.GetXElement());
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            Price1.PropertyChanged -= Price1_PropertyChanged;
            Price2.PropertyChanged -= Price2_PropertyChanged;
            Price1.ParseFromXElement(source.Elements(Price1.XElementName).ElementAt(0));
            Price2.ParseFromXElement(source.Elements(Price2.XElementName).ElementAt(1));
            Price1.PropertyChanged += Price1_PropertyChanged;
            Price2.PropertyChanged += Price2_PropertyChanged;
        }

        public string XElementName
        {
            get { return "PriceGroup"; }
        }


        public string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            throw new NotImplementedException();
        }
    }
}
