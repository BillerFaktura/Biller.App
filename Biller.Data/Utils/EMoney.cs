using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Utils
{
    public class EMoney : Money, Interfaces.IXMLStorageable
    {
        public EMoney(double amount, bool Gross = true, Currency currency = Currency.EUR)
            : base(amount, currency)
        {
            IsGross = Gross;
        }

        public bool IsGross { get { return GetValue(() => IsGross); } set { SetValue(value); } }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new XElement(XElementName, new XAttribute("Amount", Amount), new XAttribute("IsGross", IsGross), new XAttribute("Currency", Currency));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            double temp;
            if (double.TryParse(source.Attribute("Amount").Value, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                Amount = temp;
            IsGross = bool.Parse(source.Attribute("IsGross").Value);
            Currency = (Currency)Enum.Parse(typeof(Currency),source.Attribute("Currency").Value);
        }

        public string XElementName
        {
            get { return "Money"; }
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
            return new EMoney(0);
        }
    }
}
