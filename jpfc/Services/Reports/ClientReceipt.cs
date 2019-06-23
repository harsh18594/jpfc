using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Classes;
using jpfc.Models.ClientViewModels;
using jpfc.Models.ReportViewModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace jpfc.Services.Reports
{
    public class ClientReceipt
    {
        /// <summary>
        /// The MigraDoc document that represents the report.
        /// </summary>
        private Document _document;

        /// <summary>
        /// The header table
        /// </summary>
        private Table _headerTable;

        /// <summary>
        /// The main table
        /// </summary>
        private Table _table;

        private readonly Color _tableBorder = new Color(81, 125, 192);
        private readonly Color _tableBlue = new Color(235, 240, 249);
        private readonly Color _tableGray = new Color(242, 242, 242);


        // report data
        private ReceiptViewModel _model;
        private readonly string _rootPath;

        public ClientReceipt(ReceiptViewModel model, string rootPath)
        {
            _model = model;
            _rootPath = rootPath;
        }

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public Document CreateDocument()
        {
            // Create a new MigraDoc document.
            _document = new Document
            {
                Info =
                {
                    Title = $"{_model.ClientName} - {_model.ReceiptNumber} - Receipt",
                    Author = "J P Finance Chase Ltd"
                }
            };

            DefineStyles();
            CreatePage();
            FillContent();

            return _document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            var style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arial";
            style.Font.Size = 10;

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Right);

            style = _document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal.
            style = _document.Styles.AddStyle("Table", "Normal");

            // Create a new style called Title based on style Normal.
            style = _document.Styles.AddStyle("Title", "Normal");
            style.Font.Bold = true;
            style.Font.Size = 14;

            // Create a new style called ReportTitle based on style Normal.
            style = _document.Styles.AddStyle("ReportTitle", "Normal");
            style.Font.Bold = true;
            style.Font.Size = 28;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.Font.Color = Color.FromRgb(44, 91, 104);

            // Create a new style called SectionTitle based on style Normal.
            style = _document.Styles.AddStyle("SectionTitle", "Normal");
            style.Font.Bold = true;
            style.Font.Size = 10;
            style.ParagraphFormat.SpaceAfter = 10;

            // Create a new style called SectionTitle based on style Normal.
            style = _document.Styles.AddStyle("Conditions", "Normal");
            style.Font.Bold = false;
            style.Font.Size = 6;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.3);

            // Create a new style called TitleUnderlined based on style Normal.
            style = _document.Styles.AddStyle("TitleUnderlined", "Normal");
            style.Font.Bold = true;
            style.Font.Size = 12;
            style.Font.Underline = Underline.Single;

            // Create a new style called Caption based on style Normal.
            style = _document.Styles.AddStyle("Caption", "Normal");
            style.Font.Size = 10;

            // Create a new style called Reference based on style Normal.
            style = _document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        private void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            var section = _document.AddSection();

            // Define the page setup. We use an image in the header, therefore the
            // default top margin is too small for our invoice.
            section.PageSetup = _document.DefaultPageSetup.Clone();

            // We increase the TopMargin to prevent the document body from overlapping the page header.
            // We have an image of 3.5 cm height in the header.
            // The default position for the header is 1.25 cm.
            // We add 0.5 cm spacing between header image and body and get 5.25 cm.
            // Default value is 2.5 cm.
            section.PageSetup.PageFormat = PageFormat.Letter;
            section.PageSetup.Orientation = Orientation.Portrait;
            section.PageSetup.TopMargin = Unit.FromMillimeter(80);
            section.PageSetup.LeftMargin = Unit.FromInch(0.5);
            section.PageSetup.RightMargin = Unit.FromInch(0.5);
            section.PageSetup.BottomMargin = Unit.FromInch(0.75);

            // Create the header table.
            _headerTable = section.Headers.Primary.AddTable();
            _headerTable.Style = "Table";
            _headerTable.Rows.LeftIndent = 0;

            float sectionWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;
            float leftColumnWidth = 112;
            float rightColumnWidth = sectionWidth - leftColumnWidth;

            // Before you can add a row, you must define the columns.
            var headerColumn = _headerTable.AddColumn(leftColumnWidth);
            headerColumn.Format.Alignment = ParagraphAlignment.Left;

            headerColumn = _headerTable.AddColumn(rightColumnWidth);
            headerColumn.Format.Alignment = ParagraphAlignment.Left;
            headerColumn.Style = "Caption";

            // Add header image and title
            Row titleRow = _headerTable.AddRow();
            var imagePath = $"{_rootPath}\\images\\logo120X90.png";
            var image = titleRow.Cells[0].AddImage(imagePath);
            image.LockAspectRatio = true;
            image.Width = Unit.FromPoint(70);
            titleRow.Cells[0].MergeDown = 1;
            titleRow.Cells[1].Style = "ReportTitle";
            titleRow.Cells[1].AddParagraph("J P Finance Chase Ltd");
            titleRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // business details
            titleRow = _headerTable.AddRow();
            titleRow.Cells[1].Style = "SectionTitle";
            titleRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            titleRow.Cells[1].AddParagraph("Bayfield Mall, 320 Bayfield St, Suite 106A, Barrie, ON L4M 3C1 \n jpfinancechase@gmail.com | (416) 705-3885 \n BN Number #70648-6511");

            // Add horizontal line
            var paragraph = section.Headers.Primary.AddParagraph();
            paragraph.Format.Borders.Bottom.Width = 0.25;

            // Add the client details to header
            _headerTable = section.Headers.Primary.AddTable();
            _headerTable.Style = "Table";
            _headerTable.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns.
            headerColumn = _headerTable.AddColumn(Unit.FromCentimeter(4));
            headerColumn.Format.Alignment = ParagraphAlignment.Left;
            headerColumn = _headerTable.AddColumn(Unit.FromCentimeter(5));
            headerColumn.Format.Alignment = ParagraphAlignment.Left;
            headerColumn.Style = "Caption";
            headerColumn = _headerTable.AddColumn(Unit.FromCentimeter(4));
            headerColumn.Format.Alignment = ParagraphAlignment.Left;
            headerColumn = _headerTable.AddColumn(Unit.FromCentimeter(5));
            headerColumn.Format.Alignment = ParagraphAlignment.Left;
            headerColumn.Style = "Caption";

            // Add Date to top
            Row headerRow = _headerTable.AddRow();
            headerRow.Height = Unit.FromCentimeter(0.6);
            headerRow.TopPadding = Unit.FromCentimeter(0.10);       // add some space between logo and course details
            headerRow.Cells[0].AddParagraph("Date:");
            headerRow.Cells[0].Format.Font.Bold = true;
            var billDateStr = _model.BillDate.ToString("MMM dd, yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            headerRow.Cells[1].AddParagraph(billDateStr);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add Client Name to top            
            headerRow.Cells[2].AddParagraph("Client Name:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_model.ClientName);
            headerRow.Cells[3].Format.Font.Bold = false;

            // Add Client Number to top
            headerRow = _headerTable.AddRow();
            headerRow.Height = Unit.FromCentimeter(0.5);
            headerRow.Cells[0].AddParagraph("Client Number:");
            headerRow.Cells[0].Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph(_model.ClientNumber);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add contact number
            headerRow.Cells[2].AddParagraph("Contact Number:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_model.ContactNumber ?? "");
            headerRow.Cells[3].Format.Font.Bold = false;

            headerRow = _headerTable.AddRow();
            // Add receipt number
            headerRow.Cells[0].AddParagraph("Receipt Number:");
            headerRow.Cells[0].Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph(_model.ReceiptNumber);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add Address By to top
            headerRow.Cells[2].AddParagraph("Address:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_model.Address ?? "");
            headerRow.Cells[3].Format.Font.Bold = false;


            // Add horizontal line
            paragraph = section.Headers.Primary.AddParagraph();
            paragraph.Format.Borders.Bottom.Width = 0.25;

            // Create the item table
            _table = section.AddTable();
            _table.Style = "Table";
            //_table.Borders.Color = _tableBorder;
            //_table.Borders.Width = 0.25;
            //_table.Borders.Left.Width = 0.5;
            //_table.Borders.Right.Width = 0.5;
            _table.Borders.Visible = false;
            _table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            // item
            Column column = _table.AddColumn("4.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            // purity
            column = _table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // weight
            column = _table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // item price
            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // replacement value
            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // final price
            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            // background color to header
            row.Shading.Color = Color.FromRgb(204, 204, 204);

            var cellIdx = 0;
            row.Cells[cellIdx].AddParagraph("Item");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            cellIdx++;
            row.Cells[cellIdx].AddParagraph("Purity/Brand");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            cellIdx++;
            row.Cells[cellIdx].AddParagraph("Weight \n (in gm)");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            cellIdx++;
            row.Cells[cellIdx].AddParagraph("Item Price \n (per gm)");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            cellIdx++;
            row.Cells[cellIdx].AddParagraph("Replacement \n Value");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            cellIdx++;
            row.Cells[cellIdx].AddParagraph("Final Price");
            row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;

            // Add footer
            Paragraph footerSignature = new Paragraph();
            footerSignature.AddText("Initials: ________________________");

            footerSignature.Format.Alignment = ParagraphAlignment.Right;
            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(footerSignature);
            // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
            // not belong to more than one other object. If you forget cloning an exception is thrown.
            section.Footers.EvenPage.Add(footerSignature.Clone());
        }

        void FillContent()
        {
            //var count = 0;
            foreach (var item in _model.Belongings)
            {
                //count++;
                Row row = _table.AddRow();

                row.TopPadding = 1;
                row.BottomPadding = 1;
                row.Height = Unit.FromCentimeter(0.75);
                // define debit/credit identifier
                var crDr = "";
                if (item.BusinessGetsMoney)
                {
                    // client pays money
                    crDr = "[CR]";
                }
                else if (item.BusinessPaysMoney)
                {
                    // client gets money
                    crDr = "[DR]";
                }

                // define transaction
                var transactionAction = "";
                if (item.TransactionAction == Constants.TransactionAction.Purchase)
                {
                    // business purchases
                    transactionAction = "[S]";
                }
                else if (item.TransactionAction == Constants.TransactionAction.Sell)
                {
                    // business sells 
                    transactionAction = "[P]";
                }
                else if (item.TransactionAction == Constants.TransactionAction.Loan)
                {
                    // business gives loan
                    transactionAction = "[L]";
                }

                // define item description
                var itemDesc = "";
                if (!string.IsNullOrEmpty(item.ItemDescription))
                {
                    itemDesc = $"\n({item.ItemDescription})";
                }

                var cellIdx = 0;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[cellIdx].AddParagraph($"{transactionAction} {item.Metal} {itemDesc}");

                cellIdx++;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[cellIdx].AddParagraph(item.Karat ?? "");

                cellIdx++;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[cellIdx].AddParagraph(item.WeightStr ?? "");

                cellIdx++;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[cellIdx].AddParagraph(item.ItemPriceStr ?? "");

                cellIdx++;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[cellIdx].AddParagraph(item.ReplacementValueStr ?? "");

                cellIdx++;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[cellIdx].AddParagraph($"{item.FinalPriceStr ?? ""} {crDr}");
            }

            // Add an invisible row as a space line to the table
            Row _row = _table.AddRow();
            _row.Borders.Top.Visible = true;
            _row.Borders.Top.Width = 0.25;

            // add total purchase row
            _row = _table.AddRow();
            _row.Height = Unit.FromCentimeter(0.50);
            _row.Format.Alignment = ParagraphAlignment.Right;
            _row.VerticalAlignment = VerticalAlignment.Center;
            _row.Format.Font.Bold = true;
            _row.Cells[0].AddParagraph("Total Purchase");
            _row.Cells[0].MergeRight = 4;
            _row.Cells[5].AddParagraph(_model.PurchaseTotal.ToString("C"));

            // add total sell row
            _row = _table.AddRow();
            _row.Height = Unit.FromCentimeter(0.50);
            _row.Format.Alignment = ParagraphAlignment.Right;
            _row.VerticalAlignment = VerticalAlignment.Center;
            _row.Format.Font.Bold = true;
            _row.Cells[0].AddParagraph("Total Sell");
            _row.Cells[0].MergeRight = 4;
            _row.Cells[5].AddParagraph("-" + _model.SellTotal.ToString("C"));

            // add prinicipal loan amount row
            _row = _table.AddRow();
            _row.Height = Unit.FromCentimeter(0.50);
            _row.Format.Alignment = ParagraphAlignment.Right;
            _row.VerticalAlignment = VerticalAlignment.Center;
            _row.Format.Font.Bold = true;
            _row.Cells[0].AddParagraph("Principal Loan Amount");
            _row.Cells[0].MergeRight = 4;
            _row.Cells[5].AddParagraph("-" + _model.PrincipalLoanAmount.ToString("C"));

            // add final price row
            _row = _table.AddRow();
            _row.Height = Unit.FromCentimeter(0.75);
            _row.Shading.Color = Color.FromRgb(204, 204, 204);
            _row.Cells[0].Borders.Visible = false;
            _row.Cells[0].AddParagraph("Final Amount");
            _row.Cells[0].Format.Font.Bold = true;
            _row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            _row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            _row.Cells[0].MergeRight = 4;
            _row.Cells[5].Borders.Visible = false;
            _row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            _row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            _row.Cells[5].Format.Font.Bold = true;
            var cellText = _model.BillAmount.ToString("C");
            if (_model.ClientPaysFinal)
            {
                cellText = $"{_model.BillAmount.ToString("C")} [CR]";
            }
            else
            {
                cellText = $"{_model.BillAmount.ToString("C")} [DR]";
            }
            _row.Cells[5].AddParagraph(cellText);

            // add legend row
            _row = _table.AddRow();
            _row.Cells[0].Borders.Visible = false;
            _row.Cells[0].AddParagraph("[P] = Purchase \n [S] = Sell \n [L] = Loan");
            _row.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            _row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            _row.Cells[1].Borders.Visible = false;
            _row.Cells[1].AddParagraph("[DR] = Business pays to the client \n [CR] = Client pays to the business");
            _row.Cells[1].VerticalAlignment = VerticalAlignment.Top;
            _row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            _row.Cells[1].MergeRight = 3;

            // Add the replacement value agreement
            var paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText($"I, {_model.ClientName}, here by acknowledge that the replacement values mentioned in this receipt are correct to my knowledge. \n\n _____________________________");

            // add terms and conditions
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.Format.Font.Size = 8;
            paragraph.AddText("Terms and Conditions");
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "0.2cm";
            paragraph.Style = "Conditions";
            paragraph.AddText(Constants.Business.TermsConditions);

            // add date and sign area
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText($"Sign: _____________________________ \n Date: {DateTime.Now:MMM dd, yyyy}");
        }
    }
}