using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Document
{
    public abstract class Document : Data.Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        /// <summary>
        /// Default implementation for storing the <see cref="DocumentID"/>.
        /// </summary>
        public virtual string DocumentID
        {
            get { return GetValue(() => DocumentID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Readonly property for every class inheriting from <see cref="Order"/>.\n
        /// Sample implementation:
        /// <code>
        /// <pre>
        /// public override string OrderType
        /// {
        ///     get { return "Invoice"; }
        /// }
        /// </pre>
        /// </code>
        /// </summary>
        public abstract string DocumentType
        {
            get;
        }

        public virtual string LocalizedDocumentType
        {
            get { return DocumentType; }
        }

        public virtual DateTime Date
        {
            get { return GetValue(() => Date); }
            set { SetValue(value); }
        }

        /// <summary>
        /// XElement structure should be:
        /// <code>
        /// <pre>
        /// <DocumentType ID="DocumentID">
        ///     <YourContent />
        /// </DocumentType>
        /// </pre>
        /// </code>
        /// </summary>
        /// The database can only recognise items with this structure (important for getting an document or updating it).
        /// <returns></returns>
        public abstract System.Xml.Linq.XElement GetXElement();

        public abstract void ParseFromXElement(System.Xml.Linq.XElement source);

        public string XElementName { get { return DocumentType; } }


        public string ID
        {
            get { return DocumentID; }
        }

        public abstract string IDFieldName
        {
            get;
        }

        public abstract Interfaces.IXMLStorageable GetNewInstance();
    }
}
