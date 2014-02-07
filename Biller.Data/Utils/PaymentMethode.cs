using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Utils
{
    public class PaymentMethode : PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public PaymentMethode()
        {
            //insert empty values to avoid null exceptions
            Name = ""; Text = "";
        }
        
        /// <summary>
        /// The name of the payment methode
        /// </summary>
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Additional information of the payment methode. Can include placeholders:
        /// <list type="bullet">
        /// <item>
        /// <description>{1} = </description>
        /// </item>
        /// <item>
        /// <description>{2} = </description>
        /// </item>
        /// <item>
        /// <description>{3} = </description>
        /// </item>
        /// </list>
        /// </summary>
        public string Text
        {
            get { return GetValue(() => Text); }
            set { SetValue(value); }
        }

        /// <summary>
        /// You can set discount and allowances for the payment methode.\n
        /// The value should be between 0 and 1
        /// </summary>
        public double Discount
        {
            get { return GetValue(() => Discount); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the discount in percent including the symbol '%'.
        /// </summary>
        public string DiscountString
        {
            get { return (Discount * 100).ToString("0.00") + " %"; }
            set
            {
                double temp;
                if (Double.TryParse(value.Replace("%", "").Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    Discount = temp;
            }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new System.Xml.Linq.XElement(XElementName, new XElement("Name", Name), new XElement("Discount", Discount), new XElement("Text", Text));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Can not parse " + source.Name + " with " + XElementName);

            Name = source.Element("Name").Value;
            Discount = double.Parse(source.Element("Discount").Value);
            Text = source.Element("Text").Value;
        }

        public string XElementName
        {
            get { return "PaymentMethode"; }
        }

        public override bool Equals(object obj)
        {
            if (obj is PaymentMethode)
                return ((obj as PaymentMethode).Name == Name) ? true : false;
            return false;
        }

        public string ID
        {
            get { return Name; }
        }

        public string IDFieldName
        {
            get { return "Name"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new PaymentMethode();
        }
    }
}
