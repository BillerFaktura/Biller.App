using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Models
{
    public class DocumentFolderModel : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public DocumentFolderModel()
        {
            Documents = new ObservableCollection<Document.PreviewDocument>();
            ID = Guid.NewGuid().ToString();
        }

        public void GenerateID()
        {
            ID = Guid.NewGuid().ToString();
        }

        public ObservableCollection<Document.PreviewDocument> Documents { get { return GetValue(() => Documents); } set { SetValue(value); } }

        public XElement GetXElement()
        {
            var output = new System.Xml.Linq.XElement(XElementName, new XElement(IDFieldName,ID));
            foreach(var doc in Documents)
            {
                output.Add(new XElement("Entry", new XAttribute("Type", doc.DocumentType), new XAttribute("ID", doc.DocumentID)));
            }
            return output;
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            ID = source.Element("ID").Value;
            var docs = source.Element("Documents").Elements();
            Documents.Clear();
            foreach (var doc in docs)
                Documents.Add(new Document.PreviewDocument(doc.Attribute("Type").Value) { DocumentID = doc.Attribute("ID").Value });
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
