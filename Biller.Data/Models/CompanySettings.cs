using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public System.Xml.Linq.XElement GetXElement()
        {
            throw new NotImplementedException();
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            throw new NotImplementedException();
        }

        public string XElementName
        {
            get { throw new NotImplementedException(); }
        }

        public string ID
        {
            get { throw new NotImplementedException(); }
        }

        public string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new CompanySettings();
        }
    }
}
