using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Models
{
    public class CompanySettings : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public CompanySettings()
        {
            MainAddress = new Utils.Address();
            Contact = new Utils.Contact();
            TaxID = "";
            SalesTaxID = "";
            ID = Guid.NewGuid().ToString();
        }

        public Utils.Address MainAddress
        {
            get { return GetValue(() => MainAddress); }
            set { SetValue(value); }
        }

        public Utils.Contact Contact
        {
            get { return GetValue(() => Contact); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Also known as "Steuernummer"
        /// </summary>
        public string TaxID
        {
            get { return GetValue(() => TaxID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Also known as "Umsatzsteueridentifikationsnummer"
        /// </summary>
        public string SalesTaxID
        {
            get { return GetValue(() => SalesTaxID); }
            set { SetValue(value); }
        }

        public XElement GetXElement()
        {
            var output = new XElement(XElementName, new XElement("TaxID", TaxID), new XElement("SalesTaxID", SalesTaxID), MainAddress.GetXElement(), Contact.GetXElement(), new XElement(IDFieldName,ID));
            return output;
        }

        public void ParseFromXElement(XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);
            TaxID = source.Element("TaxID").Value;
            SalesTaxID = source.Element("SalesTaxID").Value;
            ID = source.Element(IDFieldName).Value;
            MainAddress.ParseFromXElement(source.Element(MainAddress.XElementName));
            MainAddress.ParseFromXElement(source.Element(MainAddress.XElementName));
        }

        public string XElementName
        {
            get { return "CompanySettings"; }
        }

        public string ID
        {
            get { return GetValue(() => ID); }
            private set { SetValue(value); }
        }

        public string IDFieldName
        {
            get { return "ID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new CompanySettings();
        }
        //public virtual int GetStorageHash()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + MainAddress.GetHashCode();
        //        hash = hash * 23 + Contact.GetHashCode();
        //        hash = hash * 23 + TaxID.GetHashCode();
        //        hash = hash * 23 + SalesTaxID.GetHashCode();
        //        return hash;
        //    }
        //}
    }
}
