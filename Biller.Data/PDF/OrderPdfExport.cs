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

namespace Biller.Data.PDF
{
    public class OrderPdfExport
    {
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

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public MigraDoc.DocumentObjectModel.Document CreateDocument(Orders.Order order)
        {
            // Create a new MigraDoc document
            this.document = new MigraDoc.DocumentObjectModel.Document();
            this.document.Info.Title = "A sample invoice";
            this.document.Info.Subject = "Demonstrates how to create an invoice.";
            this.document.Info.Author = "Stefan Lange";

            DefineStyles();

            CreatePage(order);

            FillContent(order);

            return this.document;
        }

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
            style.Font.Name = "Arial";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Arial";
            style.Font.Name = "Arial";
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
        void CreatePage(Orders.Order order)
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            // Put a logo in the header
            Image image = section.Headers.Primary.AddImage("../../PowerBooks.png");
            image.Height = "2.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Absender");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "5.0cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            paragraph = this.addressFrame.AddParagraph("Absender");
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText(order.LocalizedDocumentType + " Nr. " + order.DocumentID, TextFormat.Bold);

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

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2cm");
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
            row.Cells[4].AddParagraph("Ust.");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].AddParagraph("Gesamtpreis");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            row.Format.SpaceBefore = "0,1cm";
            row.Format.SpaceAfter = "0,25cm";
            //this.table.SetEdge(0, 0, 6, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }

        /// <summary>
        /// Creates the dynamic parts of the invoice.
        /// </summary>
        void FillContent(Orders.Order order)
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
                paragraph.AddFormattedText(article.ArticleDescription, TextFormat.Bold);
                paragraph.AddLineBreak();
                paragraph.AddText(article.ArticleText);
                row1.Cells[4].AddParagraph(new Utils.Percentage() { Amount = article.TaxClass.TaxRate }.PercentageString);
                row1.Cells[5].AddParagraph(article.RoundedGrossOrderValue.AmountString);
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
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(order.OrderCalculation.NetArticleSummary.AmountString);
            row.Format.SpaceBefore = "0,1cm";
            row.Cells[0].Borders.Bottom = NoBorder.Clone();
            row.Cells[5].Borders.Bottom = NoBorder.Clone();
            row.Cells[0].Borders.Top = NoBorder.Clone();
            row.Cells[5].Borders.Top = NoBorder.Clone();

            if (order.OrderCalculation.OrderRebate.Amount > 0)
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Abzgl. " + order.OrderRebate.PercentageString + " Gesamtrabatt" );
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 4;
                row.Cells[5].AddParagraph(order.OrderCalculation.NetOrderRebate.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[5].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[5].Borders.Top = NoBorder.Clone();
            }

            if (!String.IsNullOrEmpty(order.OrderShipment.Name))
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zzgl. " + order.OrderShipment.Name);
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 4;
                row.Cells[5].AddParagraph(order.OrderCalculation.NetShipment.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = BlackBorder.Clone();
                row.Cells[5].Borders.Bottom = BlackBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[5].Borders.Top = NoBorder.Clone();
            }

            if (order.OrderCalculation.OrderRebate.Amount > 0 || !String.IsNullOrEmpty(order.OrderShipment.Name))
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zwischensumme Netto");
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 4;
                row.Cells[5].AddParagraph(order.OrderCalculation.NetOrderSummary.AmountString);
                row.Format.SpaceBefore = "0,1cm";
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[5].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = BlackBorder.Clone();
                row.Cells[5].Borders.Top = BlackBorder.Clone();
            }

            // Add the VAT row
            foreach (var tax in order.OrderCalculation.TaxValues)
            {
                row = this.table.AddRow();
                row.Cells[0].AddParagraph("Zzgl. " + tax.TaxClass.Name + " "+ tax.TaxClassAddition);
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[0].MergeRight = 4;
                row.Cells[5].AddParagraph(tax.Value.AmountString);
                row.Cells[0].Borders.Bottom = NoBorder.Clone();
                row.Cells[5].Borders.Bottom = NoBorder.Clone();
                row.Cells[0].Borders.Top = NoBorder.Clone();
                row.Cells[5].Borders.Top = NoBorder.Clone();
            }

            row = this.table.AddRow();
            row.Cells[0].Borders.Bottom = BlackThickBorder.Clone();
            row.Cells[0].AddParagraph("Gesamtbetrag");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(order.OrderCalculation.OrderSummary.AmountString);
            row.Cells[5].Borders.Bottom = BlackThickBorder.Clone();
            row.Format.SpaceBefore = "0,25cm";
            row.Format.SpaceAfter = "0,05cm";

            //// Add the additional fee row
            //row = this.table.AddRow();
            //row.Cells[0].Borders.Visible = false;
            //row.Cells[0].AddParagraph("Shipping and Handling");
            //row.Cells[5].AddParagraph(0.ToString("0.00") + " €");
            //row.Cells[0].Format.Font.Bold = true;
            //row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //row.Cells[0].MergeRight = 4;

            //// Add the total due row
            //row = this.table.AddRow();
            //row.Cells[0].AddParagraph("Total Due");
            //row.Cells[0].Borders.Visible = false;
            //row.Cells[0].Format.Font.Bold = true;
            //row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //row.Cells[0].MergeRight = 4;
            //totalExtendedPrice += 0.19 * totalExtendedPrice;
            //row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");


            // Add the notes paragraph
            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText(order.OrderClosingText);


            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;

            renderer.RenderDocument();

            //renderer.Save("test.pdf");

            //Process.Start("test.pdf");
        }
        // Some pre-defined colors
#if true
        // RGB colors
        readonly static Color TableBorder = new Color(0, 0, 0);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);
#else
    // CMYK colors
    readonly static Color tableBorder = Color.FromCmyk(100, 50, 0, 30);
    readonly static Color tableBlue = Color.FromCmyk(0, 80, 50, 30);
    readonly static Color tableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif
    }
}
