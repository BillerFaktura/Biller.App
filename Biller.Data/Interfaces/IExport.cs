using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biller.Data.Interfaces
{
    /// <summary>
    /// Interface to create implementations of an document exporting and printing class.
    /// </summary>
    public interface IExport
    {
        /// <summary>
        /// Shows a rendered preview of a document. Call <see cref="RenderDocumentPreview"/> to update the preview.
        /// </summary>
        UIElement PreviewControl { get; set; }

        /// <summary>
        /// Renders a given document with the <see cref="PreviewControl"/>.
        /// </summary>
        /// <param name="document"></param>
        void RenderDocumentPreview(Document.Document document);

        /// <summary>
        /// Saves a given document to a file.
        /// </summary>
        /// <param name="document">The document that should be saved.</param>
        /// <param name="FilePath">Path to the file the document shall be saved.</param>
        /// <param name="OpenOnSuccess">Opens the file if the creation of the document file was successfull</param>
        void SaveDocument(Document.Document document, string FilePath, bool OpenOnSuccess = true);

        /// <summary>
        /// Prints a given document directly with the printer.
        /// </summary>
        /// <param name="document">The <see cref="Document"/> that should be printed.</param>
        void PrintDocument(Document.Document document);

        /// <summary>
        /// Lists all kinds of documents that can be rendered with this explicit implementation of IExport.
        /// </summary>
        /// <returns></returns>
        List<string> AvailableDocumentTypes();

        /// <summary>
        /// The name of the export class
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns an GUID that should never change.
        /// </summary>
        string GuID { get; }
    }
}
