using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace Biller.Data.Utils
{
    public class Shipment : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Shipment()
        {
            Name = "";
            DefaultPrice = new EMoney(0);
        }

        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(value); }
        }

        public EMoney DefaultPrice
        {
            get { return GetValue(() => DefaultPrice); }
            set { SetValue(value); }
        }

        public System.Xml.Linq.XElement GetXElement()
        {
            return new XElement(XElementName, new XElement("Name", Name), DefaultPrice.GetXElement());
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Got " + source.Name + " but expected " + XElementName);

            Name = source.Element("Name").Value.ToString();
            DefaultPrice.ParseFromXElement(source.Element(DefaultPrice.XElementName));
        }

        public string XElementName
        {
            get { return "Shipment"; }
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
            return new Shipment();
        }

        public override bool Equals(object obj)
        {
            if (obj is Shipment)
                if ((obj as Shipment).ID == ID)
                    return true;
            return false;
        }
    }
}
