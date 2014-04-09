using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Biller.Data.Models
{
    public class DocumentExportModel : Interfaces.IXMLStorageable
    {
        public DocumentExportModel()
        {
            ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Type needs to be <see cref="PreviewDocument"/> or <see cref="Document"/>.
        /// </summary>
        public dynamic Document { get; set; }

        public Interfaces.IExport Export { get; set; }

        public XElement GetXElement()
        {
            return new XElement(XElementName, new XElement(IDFieldName, ID), new XElement("Document", new XAttribute("DocumentType", Document.DocumentType), new XAttribute("LocalizedDocumentType", Document.LocalizedDocumentType)), new XElement("Export", new XAttribute("Name", Export.Name), new XAttribute("ID", Export.GuID)));
        }

        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Expected " + XElementName + " but got " + source.Name);

            ID = source.Element(IDFieldName).Value;

            dynamic doc = new Document.PreviewDocument(source.Element("Document").Attribute("DocumentType").Value);
            doc.LocalizedDocumentType = source.Element("Document").Attribute("LocalizedDocumentType").Value;
            Document = doc;

            Export = new DummyExport() { Name = source.Element("Export").Attribute("Name").Value, GuID = source.Element("Export").Attribute("ID").Value };
        }

        public string XElementName
        {
            get { return "DocumentExportPreference"; }
        }

        public string ID { get; private set; }

        public string IDFieldName
        {
            get { return "ID"; }
        }

        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new DocumentExportModel();
        }
    }
}
