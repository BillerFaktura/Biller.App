using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class OrderFolderModel : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public OrderFolderModel()
        {
        }

        public void GenerateID()
        {
            ID = Guid.NewGuid().ToString();
        }

        public ObservableCollection<Document.PreviewDocument> Documents { get { return GetValue(() => Documents); } set { SetValue(value); } }

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

        public string ID { get; private set; }

        public string IDFieldName
        {
            get { return "ID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new OrderFolderModel();
        }
    }
}
