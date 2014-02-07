using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Utils
{
    public class Address : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Address()
        {
            //Insert empty values to avoid null exceptions
            Salutation = ""; Title = ""; Forname = ""; Surname = "";
            CompanyName = ""; Street = ""; HouseNumber = ""; Zip = "";
            City = ""; Country = ""; Addition = "";
        }

        public string Salutation
        {
            get { return GetValue(() => Salutation); }
            set { SetValue(value); }
        }

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(value); }
        }

        public string Forname
        {
            get { return GetValue(() => Forname); }
            set { SetValue(value); }
        }

        public string Surname
        {
            get { return GetValue(() => Surname); }
            set { SetValue(value); }
        }

        public string CompanyName
        {
            get { return GetValue(() => CompanyName); }
            set { SetValue(value); }
        }

        public string Street
        {
            get { return GetValue(() => Street); }
            set { SetValue(value); }
        }

        public string HouseNumber
        {
            get { return GetValue(() => HouseNumber); }
            set { SetValue(value); }
        }

        public string Zip
        {
            get { return GetValue(() => Zip); }
            set { SetValue(value); }
        }

        public string City
        {
            get { return GetValue(() => City); }
            set { SetValue(value); }
        }

        public string Country
        {
            get { return GetValue(()=> Country); }
            set { SetValue(value); }
        }

        public string Addition
        {
            get { return GetValue(() => Addition); }
            set { SetValue(value); }
        }

        public virtual System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = new XElement(XElementName);
            objectnode.Add(new XElement("Salutation", Salutation), new XElement("Title", Title), new XElement("Forname", Forname),
                new XElement("Surname", Surname), new XElement("CompanyName", CompanyName), new XElement("Street", Street),
                new XElement("HouseNumber", HouseNumber), new XElement("Zip", Zip), new XElement("City", City),
                new XElement("Country", Country), new XElement("Addition", Addition));
            return objectnode;
        }

        public virtual void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("The given element is not named Address and will not be parsed.");

            Salutation = source.Element("Salutation").Value;
            Title = source.Element("Title").Value;
            Forname = source.Element("Forname").Value;
            Surname = source.Element("Surname").Value;
            CompanyName = source.Element("CompanyName").Value;
            Street = source.Element("Street").Value;
            HouseNumber = source.Element("HouseNumber").Value;
            Zip = source.Element("Zip").Value;
            City = source.Element("City").Value;
            Country = source.Element("Country").Value;
            Addition = source.Element("Addition").Value;
        }

        public virtual string XElementName
        {
            get { return "Address"; }
        }

        public string OneLineString
        {
            get
            {
                var names = (((Salutation + " " + Title + " ").Trim() + " " + (Forname + " ").Trim()).Trim() + " " + Surname).Trim();
                if (!String.IsNullOrEmpty(names))
                    names += ", ";
                
                var street = (Street + " " + HouseNumber).Trim();
                if (!String.IsNullOrEmpty(street))
                    street += ", ";

                var city = (Zip + " " + City).Trim();
                if (!String.IsNullOrEmpty(city))
                    city += ", ";

                var output = "";
                if (!String.IsNullOrEmpty(CompanyName))
                    output = CompanyName + ", ";
                output += names + street;
                if (!String.IsNullOrEmpty(Addition))
                    output += Addition + ", ";
                output += city;
                if (!String.IsNullOrEmpty(Country))
                    output += Country;

                if (output.EndsWith(", "))
                    output=output.Remove(output.LastIndexOf(", "));
                return output;
            }
        }

        public virtual string ID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public string IDFieldName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Address();
        }
    }
}
