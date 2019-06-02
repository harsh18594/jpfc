using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Classes;
using jpfc.Models.ClientViewModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace jpfc.Services.Reports
{
    public class ClientReceiptReport
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


        /// <summary>
        /// The data source for report
        /// </summary>
        private readonly List<ClientBelongingListViewModel> _belonging;

        private readonly DateTime _billDate;
        private readonly string _clientNumber;
        private readonly string _receiptNumber;
        private readonly string _clientName;
        private readonly string _clientAddress;
        private readonly string _phoneNumber;
        private readonly string _emailAddress;
        private readonly decimal _billAmount;
        private readonly bool _clientPaysFinal;
        private readonly string _rootPath;

        public ClientReceiptReport(DateTime billDate, string clientNumber, string receiptNumber, string clientName, string clientAddress,
            string phoneNumber, string emailAddress, decimal billAmount, bool clientPaysFinal, string rootPath, List<ClientBelongingListViewModel> belonging)
        {
            _billDate = billDate;
            _clientNumber = clientNumber;
            _receiptNumber = receiptNumber;
            _clientName = clientName;
            _clientAddress = clientAddress;
            _phoneNumber = phoneNumber;
            _emailAddress = emailAddress;
            _billAmount = billAmount;
            _clientPaysFinal = clientPaysFinal;
            _rootPath = rootPath;
            _belonging = belonging;
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
                    Title = $"{_clientName} - {_receiptNumber} - Receipt",
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
            style.Font.Size = 7;
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
            titleRow.Cells[1].AddParagraph("123 Main Street, Barrie, ON L4ML4M | test@email.com | (647) 123-1234 \n www.website.com");

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
            var billDateStr = _billDate.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            headerRow.Cells[1].AddParagraph(billDateStr);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add Client Name to top            
            headerRow.Cells[2].AddParagraph("Client Name:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_clientName);
            headerRow.Cells[3].Format.Font.Bold = false;

            // Add Client Number to top
            headerRow = _headerTable.AddRow();
            headerRow.Height = Unit.FromCentimeter(0.5);
            headerRow.Cells[0].AddParagraph("Client Number:");
            headerRow.Cells[0].Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph(_clientNumber);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add contact number
            headerRow.Cells[2].AddParagraph("Contact Number:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_phoneNumber ?? "");
            headerRow.Cells[3].Format.Font.Bold = false;

            headerRow = _headerTable.AddRow();
            // Add receipt number
            headerRow.Cells[0].AddParagraph("Receipt Number:");
            headerRow.Cells[0].Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph(_receiptNumber);
            headerRow.Cells[1].Format.Font.Bold = false;
            // Add Address By to top
            headerRow.Cells[2].AddParagraph("Address:");
            headerRow.Cells[2].Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph(_clientAddress ?? "");
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
            foreach (var item in _belonging)
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

                var cellIdx = 0;
                row.Cells[cellIdx].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[cellIdx].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[cellIdx].AddParagraph($"{transactionAction} {item.Metal}");

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
            _row.Borders.Visible = false;

            // add final price row
            _row = _table.AddRow();
            _row.Height = Unit.FromCentimeter(0.75);
            _row.Shading.Color = Color.FromRgb(204, 204, 204);
            _row.Cells[0].Borders.Visible = false;
            _row.Cells[0].AddParagraph("Total Price");
            _row.Cells[0].Format.Font.Bold = true;
            _row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            _row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            _row.Cells[0].MergeRight = 4;
            _row.Cells[5].Borders.Visible = false;
            _row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            _row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            _row.Cells[5].Format.Font.Bold = true;
            var cellText = _billAmount.ToString("C");
            if (_clientPaysFinal)
            {
                cellText = $"{_billAmount.ToString("C")} [CR]";
            }
            else
            {
                cellText = $"{_billAmount.ToString("C")} [DR]";
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
            paragraph.AddText($"I, {_clientName}, here by acknowledge that the replacement values mentioned in this receipt are correct to my knowledge. \n\n _____________________________");

            // add terms and conditions
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.Format.Font.Size = 8;
            paragraph.AddText("Terms and Conditions");
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "0.2cm";
            paragraph.Style = "Conditions";            
            paragraph.AddText("1) The client hereby acknowledges receipt of Loan/Sell amount, copy of this contract and payment schedule. " +
                "2) We agree to return the described property(ies) to the client only upon presentation of this contract and payment of Principal Loan amount plus Applicable Interest and Service charges. " +
                "3) Client hereby certifies that he or she is legal owner of the property(ies) as described above, empowered to sell or dispose of the above property(ies) and is/are free and clear of all liens and encumbrances. " +
                "4) Client will be responsible for any legal fees incurred by J.P. Finance Chase Ltd. resulting from this transaction. " +
                "5) All Interest charges, Service fee, Storage Fee are calculated per month and due 30 days from the date of this Loan Contract. " +
                "6) No credit shall be allowed for redemption in less than 30 days. " +
                "7) This loan is due in 12 months from the date of this contract and if we do not receive Principal Loan amount plus Interest, Service Fee, Storage Fee as mentioned in this contract, you will forfeit your ownership of the above property(ies). If the above contract is not redeemed within 12 months, the above property(ies) may be sold at public or private sale by J.P. Finance Chase Ltd. " +
                "8) Once Property(ies) is/are sold to J.P. Finance Chase Ltd. at agreed upon price, you will forfeit your rights of ownership immediately. " +
                "9) As per Pawn Broker Law, you allow us to share your information with Legal Authorities if requested by Court order, Police Department, or any other official Government authorities.");

            // add date and sign area
            paragraph = _document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText($"Sign: _____________________________ \n Date: {DateTime.Now:MMM dd, yyyy}");
        }
    }
}