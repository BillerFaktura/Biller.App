using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System.Diagnostics;
using MigraDoc.Rendering.Printing;
using MigraDoc.DocumentObjectModel.IO;

namespace OrderTypes_Biller.Export
{
    public class OrderPdfExport : Biller.Data.Interfaces.IExport
    {
        Biller.UI.ViewModel.MainWindowViewModel ParentViewModel;
        public OrderPdfExport(Biller.UI.ViewModel.MainWindowViewModel ParentViewModel)
        {
            this.ParentViewModel = ParentViewModel;
            PreviewElement = new MigraDoc.Rendering.Windows.DocumentPreview();
            PrintDialog = new System.Windows.Forms.PrintDialog();
        }

        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        MigraDoc.DocumentObjectModel.Document document;

        /// <summary>
        /// The text frame of the MigraDoc document that contains the address.
        /// </summary>
        TextFrame addressFrame;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        Table table;

        MigraDoc.Rendering.Windows.DocumentPreview PreviewElement;

        System.Windows.Forms.PrintDialog PrintDialog;

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Calibri";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        async void CreatePage(Order.Order order)
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            // Create footer
            var mycryptominerfooter = section.Footers.Primary.AddTable();
            mycryptominerfooter.Style = "Table";
            mycryptominerfooter.Borders.Color = Color.Empty;
            mycryptominerfooter.Borders.Visible = false;
            mycryptominerfooter.Rows.LeftIndent = 0;
            mycryptominerfooter.LeftPadding = "-1cm";

            Column footercolumn = mycryptominerfooter.AddColumn("4.75cm");
            //footercolumn.Borders.Visible = false;
            //footercolumn.Format.Alignment = ParagraphAlignment.Left;
            //footercolumn = mycryptominerfooter.AddColumn("4.5cm");
            //footercolumn.Borders.Visible = false;
            //footercolumn.Format.Alignment = ParagraphAlignment.Left;
            //footercolumn = mycryptominerfooter.AddColumn("4.0cm");
            //footercolumn.Borders.Visible = false;
            //footercolumn.Format.Alignment = ParagraphAlignment.Left;
            //footercolumn = mycryptominerfooter.AddColumn("5.75cm");
            //footercolumn.Borders.Visible = false;
            //footercolumn.Format.Alignment = ParagraphAlignment.Left;

            Row footerrow = mycryptominerfooter.AddRow();
            //footerrow.Cells[0].AddParagraph("");
            //footerrow.Cells[1].AddParagraph("");
            //footerrow.Cells[2].AddParagraph("");
            //footerrow.Cells[3].AddParagraph("");

            // Create the text frame for the address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "5.0cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            var address = (await ParentViewModel.Database.AllStorageableItems(new Biller.Data.Models.CompanySettings())).FirstOrDefault() as Biller.Data.Models.CompanySettings;
            
            Paragraph paragraph = this.addressFrame.AddParagraph(address.MainAddress.OneLineString);
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 8;
            paragraph.Format.SpaceAfter = 3;

            // Datum
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "4cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.Format.RightIndent = "-0.5cm";
            paragraph.AddText("Rechnungsdatum:");
            paragraph.AddTab();
            paragraph.AddText(order.Date.ToString("dd.MM.yyyy"));
            paragraph.AddLineBreak();
            paragraph.AddText("Leistungsdatum:");
            paragraph.AddTab();
            paragraph.AddTab();
            paragraph.AddText(order.Date.ToString("dd.MM.yyyy"));


            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText(order.LocalizedDocumentType + " Nr. " + order.DocumentID, TextFormat.Bold);

            if (!String.IsNullOrEmpty(order.OrderOpeningText))
            {
                paragraph = section.AddParagraph(order.OrderOpeningText);
                paragraph.Format.SpaceAfter = "0.75cm";
            }

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("1.75cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("6.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Cells[0].AddParagraph("Pos");
            row.Cells[0].Format.Font.Bold = false;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Menge");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[2].AddParagraph("Art.-Nr.");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[3].AddParagraph("Text");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[4].AddParagraph("Einzelpreis");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].AddParagraph("Ust.");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[6].AddParagraph("Gesamtpreis");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Right;
            row.Format.SpaceBefore = "0,1cm";
            row.Format.SpaceAfter = "0,25cm";
            //this.table.SetEdge(0, 0, 6, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }

        /// <summary>
        /// Creates the dynamic parts of the invoice.
        /// </summary>
        void FillContent(Order.Order order)
        {
            Paragraph paragraph = this.addressFrame.AddParagraph();
            foreach (var line in order.Customer.MainAddress.AddressStrings)
            {
                paragraph.AddText(line);
                paragraph.AddLineBreak();
            }

            foreach (var article in order.OrderedArticles)
            {
                // Each item fills two rows
                Row row1 = this.table.AddRow();

                row1.Cells[0].AddParagraph(article.OrderPosition.ToString());
                row1.Cells[1].AddParagraph(article.OrderedAmountString);
                row1.Cells[2].AddParagraph(article.ArticleID);
                paragraph = row1.Cells[3].AddParagraph();
                paragraph.AddText(article.ArticleDescription);
                paragraph.AddLineBreak();
                paragraph.AddText(article.ArticleText);
                row1.Cells[4].AddParagraph(article.OrderPrice.Price1.ToString());
                row1.Cells[5].AddParagraph(new Biller.Data.Utils.Percentage() { Amount = article.TaxClass.TaxRate.Amount }.PercentageString);
                row1.Cells[6].AddParagraph(article.RoundedGrossOrderValue.AmountString);
                row1.Format.SpaceBefore = "0,1cm";
                row1.Format.SpaceAfter = "0,4cm";
                //this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
            }

            Border BlackBorder = new Border();
            BlackBorder.Visible = true;
            BlackBorder.Color = Colors.Black;
            BlackBorder.Width = 0.75;

            Border BlackThickBorder = new Border();
            BlackThickBorder.Visible = true;
            BlackThickBorder.Color = Colors.Black;
            BlackThickBorder.Width = 1.5;

            Border NoBorder = new Border();
            NoBorder.Visible = false;

            // Add the total price row
            Row row = this.table.AddRow();
            row.Cells[0].AddParagraph("Zwischensumme Netto");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 5;
            row.Cells[6].AddParagraph(order.OrderCalculation.NetArticleSummary.AmountString);
            row.Format.SpaceBefore = "0,1cm";
            row.Cells[0].Borders.Bottom = NoBorder.Clone();
            row.Cells[6].Borders.Bottom = NoBorder.Clone();
            row.Cells[0].Borders.Top = NoBorder.Clone();
            row.Cells[6].Borders.Top = NoBorder.Clone();

            if (order.OrderCalculation.OrderRebate.Amount > 0)
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Abzgl. " + order.OrderRebate.PercentageString + " Gesamtrabatt" );
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 5;
                row.Cells[6].AddParagraph(order.OrderCalculation.NetOrderRebate.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[6].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[6].Borders.Top = NoBorder.Clone();
            }

            if (!String.IsNullOrEmpty(order.OrderShipment.Name))
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zzgl. " + order.OrderShipment.Name);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 5;
                row.Cells[6].AddParagraph(order.OrderCalculation.NetShipment.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = BlackBorder.Clone();
                row.Cells[6].Borders.Bottom = BlackBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[6].Borders.Top = NoBorder.Clone();
            }

            if (order.OrderCalculation.OrderRebate.Amount > 0 || !String.IsNullOrEmpty(order.OrderShipment.Name))
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zwischensumme Netto");
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 5;
                row.Cells[6].AddParagraph(order.OrderCalculation.NetOrderSummary.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[6].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = BlackBorder.Clone();
                row.Cells[6].Borders.Top = BlackBorder.Clone();
            }

            // Add the VAT row
            foreach (var tax in order.OrderCalculation.TaxValues)
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zzgl. " + tax.TaxClass.Name + " "+ tax.TaxClassAddition);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 5;
                row.Cells[6].AddParagraph(tax.Value.AmountString);
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[6].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[6].Borders.Top = NoBorder.Clone();
            }

            row = this.table.AddRow();
            row.Cells[0].Borders.Bottom = BlackThickBorder.Clone();
            row.Cells[0].AddParagraph("Gesamtbetrag");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 5;
            row.Cells[6].AddParagraph(order.OrderCalculation.OrderSummary.AmountString);
            row.Cells[6].Borders.Bottom = BlackThickBorder.Clone();
            row.Format.SpaceBefore = "0,25cm";
            row.Format.SpaceAfter = "0,05cm";

            // Add the notes paragraph
            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText(order.OrderClosingText);
        }

        readonly static Color TableBorder = new Color(0, 0, 0);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);

        public System.Windows.UIElement PreviewControl
        {
            get { return PreviewElement; }
            set { }
        }

        public async void RenderDocumentPreview(Biller.Data.Document.Document document)
        {
            if (document is Order.Order)
            {
                PreviewElement.Ddl = DdlWriter.WriteToString(await GetDocument(document as Order.Order));
            }
        }

        private async Task<MigraDoc.DocumentObjectModel.Document> GetDocument(Order.Order order)
        {
            // Create a new MigraDoc document
            this.document = new MigraDoc.DocumentObjectModel.Document();
            this.document.Info.Author = "Biller V2";

            DefineStyles();

            await Task.Run(() => CreatePage(order));

            FillContent(order);

            return this.document;
        }

        public async void SaveDocument(Biller.Data.Document.Document document, string filename, bool OpenOnSuccess = true)
        {
            if (document is Order.Order)
            {
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                renderer.Document = await GetDocument(document as Order.Order);

                renderer.RenderDocument();
                renderer.Save(filename);

                if (OpenOnSuccess)
                    Process.Start(filename);
            }
        }

        public void PrintDocument(Biller.Data.Document.Document document)
        {
            /*
            // Reuse the renderer from the preview
            RenderDocumentPreview(document);
            DocumentRenderer renderer = this.PreviewElement.Renderer;
            if (renderer != null)
            {
                int pageCount = renderer.FormattedDocument.PageCount;
                // Creates a PrintDocument that simplyfies printing of MigraDoc documents
                MigraDocPrintDocument printDocument = new MigraDocPrintDocument();

                if (PrintDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Attach the current printer settings
                    printDocument.PrinterSettings = this.PrintDialog.PrinterSettings;

                    if (this.PrintDialog.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.Selection)
                        printDocument.SelectedPage = this.PreviewElement.Page;

                    renderer.PrepareDocument();

                    // Attach the current document renderer
                    printDocument.Renderer = renderer;
                    
                    // Print the document
                    printDocument.Print();
                }
            }
            */

            SaveDocument(document, document.DocumentType + document.DocumentID + ".pdf");
        }

        public List<string> AvailableDocumentTypes()
        {
            var output = new List<string>();
            output.Add("Invoice");
            output.Add("Docket");
            return output;
        }


        public string Name
        {
            get
            {
                return "Standardlayout für Rechnungen und Lieferscheine";
            }
        }

        public string GuID
        {
            get { return "bfeeab8f-c7fc-4560-8278-85de2a413d40"; }
        }

    }
}
