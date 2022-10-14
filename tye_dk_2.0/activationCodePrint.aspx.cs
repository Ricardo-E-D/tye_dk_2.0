using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using monosolutions.Utils;
using System.Web.Hosting;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using System.Drawing.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel.Shapes;
using System.IO;

public partial class activationCodePrint : PageBase {

	int EditID = 0;
	PageSetup ps = new PageSetup();

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Administrator }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	private MemoryStream RotatePagesBy90(MemoryStream stream) {
		// Create the output document
		PdfDocument outputDocument = new PdfDocument();

		// Show single pages
		// (Note: one page contains two pages from the source document)
		outputDocument.PageLayout = PdfPageLayout.SinglePage;

		XFont font = new XFont("Verdana", 8, XFontStyle.Bold);
		XStringFormat format = new XStringFormat();
		format.Alignment = XStringAlignment.Center;
		format.LineAlignment = XLineAlignment.Far;
		XGraphics gfx;
		XRect box;

		// Open the external document as XPdfForm object
		XPdfForm form = XPdfForm.FromStream(stream);

		for (int idx = 0; idx < form.PageCount; idx++) {
			// Add a new page to the output document
			PdfPage page = outputDocument.AddPage();
			page.Orientation = PdfSharp.PageOrientation.Portrait;
			double width = page.Width;
			double height = page.Height;

			int rotate = page.Elements.GetInteger("/Rotate");

			gfx = XGraphics.FromPdfPage(page);

			// Set page number (which is one-based)
			form.PageNumber = idx + 1;

			//Rotate landscape pages
			if (form.Page.Width > form.Page.Height) {
				//Need 595-842 = 247. Otherwise the page contents are moved outside the page after the rotation
				box = new XRect(-247, 0, form.Page.Width, form.Page.Height);
				form.Page.Rotate = 90;

				// Draw the page identified by the page number like an image
				gfx.DrawImage(form, box);

				//gfx.RotateTransform(90); // Still does nothing :(
			}
				//Just draw portrait pages
			else {
				box = new XRect(0, 0, form.Page.Width, form.Page.Height);
				gfx.DrawImage(form, box);
			}
			//If you have embeded fonts - embed them again
			//new InvoiceEmailController().EmbedFonts(page, gfx);
		}

		MemoryStream ms = new MemoryStream();
		outputDocument.Save(ms);
		return ms;
	}

	private Document pdfPrepare() {
		Document pdf = new Document();
		pdf.DefaultPageSetup.PageWidth = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(85);
		pdf.DefaultPageSetup.PageHeight = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(54);
		pdf.DefaultPageSetup.TopMargin = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(0);
		pdf.DefaultPageSetup.RightMargin = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(0);
		pdf.DefaultPageSetup.BottomMargin = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(0);
		pdf.DefaultPageSetup.LeftMargin = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(0);
		pdf.DefaultPageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;

		using (var ipa = statics.GetApi()) {
			var codes = ipa.ActivationCodeGetCollection(EditID);

			var optician = ipa.UserGetSingle(EditID);

			foreach (var code in codes) {
				CheckBox chk = (CheckBox)pnlChecks.FindControl("chk" + code.Code);
				if (chk == null)
					continue;
				if (chk.Checked) {
					pdfAddPage(code, optician, ref pdf);
				}
			}
		}

		PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
		renderer.Document = pdf;
		renderer.RenderDocument();

		using (MemoryStream ms = new MemoryStream()) {
			renderer.PdfDocument.Save(ms, false);
			//ms = RotatePagesBy90(ms);

			//renderer.Save(@"C:\temp\pdf.pdf");
			//System.Diagnostics.Process.Start(@"C:\temp\pdf.pdf");
			//using (MemoryStream ms2 = new MemoryStream()) {

			//   using (PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(ms, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import)) {

			//      foreach (PdfPage page in inputDocument.Pages) {
			//         page.Rotate = 0;
			//         page.Orientation = PdfSharp.PageOrientation.Portrait;
			//      }

			//   }

				Response.Buffer = true;
				Response.Clear();
				Response.ClearContent();
				Response.ClearHeaders();
				Response.ContentType = "application/octet-stream";
				string contentValue = string.Format("attachment; filename=codes.pdf;");
				Response.AddHeader("Content-Disposition", contentValue);

				//renderer.PdfDocument.Save(ms);
				Response.OutputStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
			//   ms2.Close();
			//}
			ms.Close();
		}
		Response.End();

		return pdf;

	}

	private void pdfAddPage(tye.Data.ActivationCode code, tye.Data.User optician, ref Document pdf) {

		string strOpticianName = optician.FullName;
		string strOptician = "\n"
				+ optician.Address + "\n"
				+ optician.PostalCode + " "
				+ optician.City + "\n"
				+ optician.Phone + "\n"
				+ optician.Email + " \u8325";


		Section section = pdf.AddSection();
		var bg = section.AddImage(Server.MapPath("~/img/code-card-template.jpg"));

		bg.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Page;
		bg.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Page;
		bg.WrapFormat.Style = MigraDoc.DocumentObjectModel.Shapes.WrapStyle.Through;

		var frameCode = section.AddTextFrame();
		var paraFrameCode = frameCode.AddParagraph(code.Code);
		paraFrameCode.Format.Alignment = ParagraphAlignment.Center;
		paraFrameCode.Format.Font.Size = MigraDoc.DocumentObjectModel.Unit.FromPoint(24);
		if (code.Code.Length > 6) {
			paraFrameCode.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(20);
		}
		if (code.Code.Length > 7) {
			paraFrameCode.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(55);
		}
		frameCode.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Page;
		frameCode.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Page;
		frameCode.Orientation = MigraDoc.DocumentObjectModel.Shapes.TextOrientation.Horizontal;
		frameCode.Width = pdf.DefaultPageSetup.PageWidth;
		frameCode.Top = MigraDoc.DocumentObjectModel.Unit.FromPoint(58); //40
		

		var frameOpticianName = section.AddTextFrame();
		var paraFrameOpticianName = frameOpticianName.AddParagraph(strOpticianName);
		paraFrameOpticianName.Format.Alignment = ParagraphAlignment.Left; // Center
		paraFrameOpticianName.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(10);
		paraFrameOpticianName.Format.Font.Size = MigraDoc.DocumentObjectModel.Unit.FromPoint(8);
		frameOpticianName.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Page;
		frameOpticianName.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Page;
		frameOpticianName.Orientation = MigraDoc.DocumentObjectModel.Shapes.TextOrientation.Horizontal;
		//frameOptician.Left = ShapePosition.Center;
		frameOpticianName.Width = pdf.DefaultPageSetup.PageWidth;
		frameOpticianName.Top = MigraDoc.DocumentObjectModel.Unit.FromPoint(98); // 86

		var frameOptician = section.AddTextFrame();
		var paraFrameOptician = frameOptician.AddParagraph(strOptician);
		paraFrameOptician.Format.Alignment = ParagraphAlignment.Left; // Center
		paraFrameOptician.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(10);
		paraFrameOptician.Format.Font.Size = MigraDoc.DocumentObjectModel.Unit.FromPoint(6);
		frameOptician.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Page;
		frameOptician.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Page;
		frameOptician.Orientation = MigraDoc.DocumentObjectModel.Shapes.TextOrientation.Horizontal;
		//frameOptician.Left = ShapePosition.Center;
		//frameOptician.Left = MigraDoc.DocumentObjectModel.Unit.FromPoint(100);
		frameOptician.Width = pdf.DefaultPageSetup.PageWidth;
		frameOptician.Top = MigraDoc.DocumentObjectModel.Unit.FromPoint(108); // 86

		var frameValid = section.AddTextFrame();
		var paraFrameValid = frameValid.AddParagraph(Dictionary.GetValue("activationCodeValid", optician.Language));
		paraFrameValid.Format.Alignment = ParagraphAlignment.Right;
		paraFrameValid.Format.RightIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(10);
		paraFrameValid.Format.Font.Color = Colors.White;
		paraFrameValid.Format.Font.Size = MigraDoc.DocumentObjectModel.Unit.FromPoint(5);
		paraFrameValid.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromPoint(150);
		frameValid.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Page;
		frameValid.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Page;
		frameValid.Orientation = MigraDoc.DocumentObjectModel.Shapes.TextOrientation.Horizontal;
		frameValid.Width = pdf.DefaultPageSetup.PageWidth;
		frameValid.Top = MigraDoc.DocumentObjectModel.Unit.FromPoint(115);


	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		if (!int.TryParse(VC.RqValue("OpticianID"), out EditID) || EditID == 0) {
			AddJavascript("$(function() { window.close(); });");
			return;
		}

		using (var ipa = statics.GetApi()) {

			var codes = ipa.ActivationCodeGetCollection(EditID); //.Where(n => !n.ClientUserID.HasValue);
			if (VC.RqValue("showall") == "true") {

			} else {
				lnkShowAll.Visible = true;
				lnkShowAll.NavigateUrl = "activationCodePrint.aspx?OpticianID=" + EditID + "&showall=true";
				codes = codes.Where(n => !n.ClientUserID.HasValue).ToList();
			}

			var optician = ipa.UserGetSingle(EditID);

			if (optician == null) {
				AddJavascript("$(function() { window.close(); });");
				return;
			}

			//string imgPath = HostingEnvironment.MapPath("~/img/keycardPrint.png");
			string imgPath = HostingEnvironment.MapPath("~/img/code-card-template.jpg");
			string strOptician = optician.FullName + "\n"
				+ optician.Address + "\n"
				+ optician.PostalCode + " "
				+ optician.City + "\n"
				+ optician.Phone + " - "
				+ optician.Email;

			string tempPath = HostingEnvironment.MapPath("~/temp/");

			//toPdf(codes.ToList());

			foreach (var code in codes) {

				monosolutions.Utils.Media.Image.ImageProcessing ip = new monosolutions.Utils.Media.Image.ImageProcessing(imgPath);
				ip.SaveFormat = monosolutions.Utils.Media.Image.ImageProcessing.Format.Jpeg;
				ip.SaveQuality = 100;
				ip.TargetResolution = 300;

				string filename = System.IO.Path.Combine(tempPath + code.Code + ".jpg");

				ip.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Top;
				
				ip.CaptionOffsetY = 105; //45
				ip.DrawCaption(code.Code, System.Drawing.Color.Black, "Courier", 20);

				ip.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
				ip.CaptionOffsetX = 10;
				ip.CaptionOffsetY = 165; //120
				ip.DrawCaption(strOptician, System.Drawing.Color.Black, "Tahoma", 7);

				ip.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
				ip.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
				ip.CaptionOffsetY = -20;
				ip.DrawCaption(Dictionary.GetValue("activationCodeValid", optician.Language), System.Drawing.Color.White, "Tahoma", 6);


				ip.Save(filename);
				ip.Dispose();
				bool activated = code.ActivationDate == null && code.ClientUserID == null;
				bool printed = code.Printed;

				list.Controls.Add(new LiteralControl("<img" + (printed ? " style=\"display:none;\"" : "") + " id=\"" + code.Code + "\"src=\"/temp/" + code.Code + ".jpg\" />"));

				CheckBox chk = new CheckBox() { CssClass = "checker", ID = "chk" + code.Code, ClientIDMode = System.Web.UI.ClientIDMode.Static };
				chk.Checked = !printed;
				chk.Text = code.Code + "(";
				if (!printed)
					chk.Text += "not printed";
				if (!activated)
					chk.Text += " not activated";

				if (code.ClientUserID.HasValue)
					chk.Text += " ER BRUGT!";

				chk.Text += ")";

				pnlChecks.Controls.Add(chk);
				pnlChecks.Controls.Add(new LiteralControl("<br />"));

			}
		}


	}

	protected void eBtnCreatePdf_Click(object sender, EventArgs e) {
		PrivateFontCollection pfc = new PrivateFontCollection();

		string tempPath = HostingEnvironment.MapPath("~/temp/");

		tye.Data.User optician = null;
		using (var ipa = statics.GetApi())
			optician = ipa.UserGetSingle(EditID);

		//string imgPath = HostingEnvironment.MapPath("~/img/code-card-template.jpg");
		string strOptician = optician.FullName + "\n"
			+ optician.Address + "\n"
			+ optician.PostalCode + " "
			+ optician.City + "\n"
			+ optician.Phone + " - "
			+ optician.Email;

		eBtnMarkAsPrinted_Click(btnMarkAsPrinted, new EventArgs());
		var doc = pdfPrepare();
	}

	protected void eBtnMarkAsPrinted_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var codes = ipa.ActivationCodeGetCollection(EditID);
			var optician = ipa.UserGetSingle(EditID);

			foreach (var code in codes) {
				CheckBox chk = (CheckBox)pnlChecks.FindControl("chk" + code.Code);
				if (chk == null)
					continue;
				if (chk.Checked) {
					code.Printed = true;
					ipa.ActivationCodeSave(code);
				}
			}
		}

		AddJavascript("$(function() { window.close(); });");
		return;

	}

	private void redir() {

	}
}