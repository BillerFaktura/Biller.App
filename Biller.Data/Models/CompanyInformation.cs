using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Models
{
    public class CompanyInformation : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        /// <summary>
        /// Default constructor when building from a database
        /// </summary>
        public CompanyInformation()
        {
            CompanyName = "";
            CompanyID = "";
        }

        /// <summary>
        /// Use this constructor if you want to generate a new ID for a new company
        /// </summary>
        /// <param name="NewCompanyName">The name of the company you want to create.</param>
        public CompanyInformation(string NewCompanyName)
        {
            CompanyName = NewCompanyName;
            CompanyID = Guid.NewGuid().ToString();
        }

        public string CompanyName
        {
            get { return GetValue(() => CompanyName); }
            set { SetValue(value); }
        }

        public string CompanyID
        {
            get { return GetValue(() => CompanyID); }
            set { SetValue(value); }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new XElement(XElementName, new XElement("CompanyName", CompanyName), new XElement("CompanyID", CompanyID));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Can not parse " + source.Name + " from " + XElementName);
            CompanyName = source.Element("CompanyName").Value;
            CompanyID = source.Element("CompanyID").Value;
        }

        public string XElementName
        {
            get { return "CompanyInformation"; }
        }


        public string ID
        {
            get { return CompanyID; }
        }

        public string IDFieldName
        {
            get { return "CompanyID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new CompanyInformation();
        }

        public void GenerateNewID()
        {
            CompanyID = Guid.NewGuid().ToString();
        }
    }
}
