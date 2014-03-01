using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Models
{
    public class DocumentFolderModel : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public DocumentFolderModel()
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
            get { return "DocumentFolder"; }
        }

        public string ID { get; private set; }

        public string IDFieldName
        {
            get { return "ID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new DocumentFolderModel();
        }

        public override bool Equals(object obj)
        {
            if (obj is DocumentFolderModel)
                if ((obj as DocumentFolderModel).ID == this.ID)
                    return true;
            return false;
        }
    }
}
