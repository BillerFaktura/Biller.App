using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Utils
{
    public class TaxClass : PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public TaxClass()
        {
            Name = "";   
        }

        public virtual string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(value); }
        }

        /// <summary>
        /// TaxRate has to be between 0 and 1
        /// </summary>
        public virtual double TaxRate
        {
            get { return GetValue(() => TaxRate); }
            set { SetValue(value); }
        }

        public override bool Equals(object obj)
        {
            if (obj is TaxClass)
            {
                if ((obj as TaxClass).Name == Name && (obj as TaxClass).TaxRate == TaxRate)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return ("{TaxClass: Name="  +Name + "; TaxRate=" + TaxRate.ToString() + "}");
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + TaxRate.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public System.Xml.Linq.XElement GetXElement()
        {
            var objectnode = new XElement("TaxClass");
            objectnode.Add(new XElement("Name", Name));
            objectnode.Add(new XElement("TaxRate", TaxRate));
            return objectnode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != "TaxClass")
                throw new Exception("The given source element is not named TaxClass and will not be parsed!");

            Name = source.Element("Name").Value;
            double temp;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            bool parse = double.TryParse(source.Element("TaxRate").Value, NumberStyles.Number, provider, out temp);
            if (parse)
            {
                TaxRate = temp;
            }
            else
            {
                throw new Exception("Could not parse a valid TaxRate for the TaxClass named:" + Name);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string XElementName
        {
            get { return "TaxClass"; }
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
            return new TaxClass();
        }
    }
}
