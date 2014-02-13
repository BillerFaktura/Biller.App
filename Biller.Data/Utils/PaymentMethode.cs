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
            Name = ""; Text = ""; Discount = new Percentage();
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
        public Percentage Discount
        {
            get { return GetValue(() => Discount); }
            set { SetValue(value); }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new System.Xml.Linq.XElement(XElementName, new XElement("Name", Name), Discount.GetXElement(), new XElement("Text", Text));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Can not parse " + source.Name + " with " + XElementName);

            Name = source.Element("Name").Value;
            Discount.ParseFromXElement(source.Element(Discount.XElementName));
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
