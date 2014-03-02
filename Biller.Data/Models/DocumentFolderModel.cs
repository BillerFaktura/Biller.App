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
            return new System.Xml.Linq.XElement(XElementName, new System.Xml.Linq.XElement(IDFieldName,ID),
                new System.Xml.Linq.XElement("Documents",
                    from docs in this.Documents
                    select docs));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            ID = source.Element("ID").Value;
            var docs = source.Element("Documents").Elements();
            Documents.Clear();
            //foreach (var doc in docs)
            //    Documents.Add(doc.Value);
        }

        public string XElementName
        {
            get { return "DocumentFolder"; }
        }

        public string ID { get { return GetValue(() => ID); } private set { SetValue(value); } }

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
