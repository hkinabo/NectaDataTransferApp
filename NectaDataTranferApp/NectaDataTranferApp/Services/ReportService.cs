

namespace NectaDataTransfer.Services
{
	public class ReportService
	{
		//Export weather data to PDF document.

		//private static Table _table;
		//private static List<ErrorModel> errorModels = new();
		//public static MemoryStream CreatePdf(ErrorModel[] errorlist)
		//{
		//	//Create a new PDF document.
		//	using Syncfusion.Pdf.PdfDocument pdfDocument = new();
		//	int paragraphAfterSpacing = 8;
		//	int cellMargin = 8;
		//	//Add page to the PDF document.
		//	Syncfusion.Pdf.PdfPage page = pdfDocument.Pages.Add();
		//	//Create a new font.
		//	PdfStandardFont font = new(PdfFontFamily.TimesRoman, 16);

		//	//Create a text element to draw a text in PDF page.
		//	PdfTextElement title = new("Candidate List with missing Candidate Number ", font, PdfBrushes.Black);
		//	PdfLayoutResult result = title.Draw(page, new Syncfusion.Drawing.PointF(0, 0));
		//	PdfStandardFont contentFont = new(PdfFontFamily.TimesRoman, 12);
		//	PdfTextElement content = new("", contentFont, PdfBrushes.Black);
		//	PdfLayoutFormat format = new()
		//	{
		//		Layout = PdfLayoutType.Paginate
		//	};
		//	//Draw a text to the PDF document.
		//	result = content.Draw(page, new Syncfusion.Drawing.RectangleF(0, result.Bounds.Bottom + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

		//	//Create a PdfGrid.
		//	PdfGrid pdfGrid = new();
		//	pdfGrid.Style.CellPadding.Left = cellMargin;
		//	pdfGrid.Style.CellPadding.Right = cellMargin;
		//	//Applying built-in style to the PDF grid.
		//	pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

		//	//Assign data source.
		//	pdfGrid.DataSource = errorlist ?? throw new ArgumentNullException("Forecast cannot be null");

		//	pdfGrid.Style.Font = contentFont;
		//	//Draw PDF grid into the PDF page.
		//	_ = pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

		//	using MemoryStream stream = new();
		//	//Saving the PDF document into the stream.
		//	pdfDocument.Save(stream);
		//	//Closing the PDF document.
		//	pdfDocument.Close(true);
		//	return stream;

		//}
		//public static byte[] Report(List<ErrorModel> emodel)
		//{
		//	errorModels = emodel;

		//	ReportHeader();
		//	ReportBody();
		//	using MemoryStream stream = new();
		//	PdfWriter writer = new(stream);
		//	iText.Kernel.Pdf.PdfDocument pdf = new(writer);
		//	Document _document = new(pdf, PageSize.A4, false);
		//	_document.SetMargins(20, 20, 20, 20);
		//	_ = _document.Add(new Paragraph("List without Candidate Number")
		//   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
		//   .SetFontSize(16)
		//   .SetMarginBottom(2));

		//	_ = _document.Add(_table);

		//	// Page numbers
		//	iText.Kernel.Pdf.PdfPage page = pdf.GetPage(1);
		//	iText.Kernel.Geom.Rectangle pageSize = page.GetPageSize();
		//	float x = pageSize.GetWidth() / 2;
		//	float y = pageSize.GetBottom();
		//	int n = pdf.GetNumberOfPages();
		//	for (int i = 1; i <= n; i++)
		//	{
		//		_ = _document.ShowTextAligned(new Paragraph(string
		//		   .Format("page" + i + " of " + n)),
		//			x, y + 4, i, TextAlignment.RIGHT,
		//			VerticalAlignment.BOTTOM, 0);
		//	}

		//	_document.Close();
		//	//stream.Position = 0;

		//	return stream.ToArray();

		//}
		//private static void ReportHeader()
		//{

		//}

		//private static void ReportBody()
		//{

		//	_table = new Table(new float[] { 20, 80, 80, 100, 100, 100 });
		//	_ = _table.SetMinWidth(100)
		//	   .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
		//	   .SetFontSize(10);
		//	_ = _table.AddHeaderCell("Sn");
		//	_ = _table.AddHeaderCell("Prem");
		//	_ = _table.AddHeaderCell("CandidateNo");
		//	_ = _table.AddHeaderCell("First name");
		//	_ = _table.AddHeaderCell("Other name");
		//	_ = _table.AddHeaderCell("Last name");
		//	//  _table.Flush();
		//	int sn = 1;
		//	foreach (ErrorModel item in errorModels)
		//	{
		//		_ = _table.AddCell(sn++.ToString());
		//		_ = _table.AddCell(item.PremNumber);
		//		_ = _table.AddCell(item.CandNumber);
		//		_ = _table.AddCell(item.Fname);
		//		_ = _table.AddCell(item.Oname);
		//		_ = _table.AddCell(item.Sname);

		//	}

		//}

	}
}
