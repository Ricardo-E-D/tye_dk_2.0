using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

/// <summary>
/// Summary description for ProcessParser
/// </summary>
public class ProcessParser {

	HttpContext context = null;
	public ProcessParser(HttpContext Context) {
		context = Context;
	}
	
	private string RqValue(string Variable) {
		if (context.Request.QueryString[Variable] != null)
			return context.Request.QueryString[Variable];
		else
			return String.Empty;
	}
	
	public List<ProcessStep> Parse() {
		List<ProcessStep> lst = new List<ProcessStep>();
		string[] keys = context.Request.QueryString.AllKeys.OrderBy(n => n).ToArray();
		if (keys.Where(n => n.Contains(":")).Any()) {
			for (int i = 0; i <= 30; i++) {
				if (!keys.Where(n => n.StartsWith(i + ":")).Any())
					continue;

				ProcessStep ps = new ProcessStep();
				Settings set = new Settings();
				populateSettings(i + ":", set);
				ps.Settings = set;
				lst.Add(ps);
			}
		}
		ProcessStep psPlain = new ProcessStep();
		Settings setPlain = new Settings();
		populateSettings("", setPlain);
		psPlain.Settings = setPlain;
		lst.Insert(0, psPlain);

		return lst;
	}

	private Settings populateSettings(string prefix, Settings set) {
		int iTry = 0;

		set.CacheActive = (RqValue("cache") == "1");
		int cacheTtl = 0;
		if (int.TryParse(RqValue("cache-ttl"), out cacheTtl) && cacheTtl > 0)
			set.CacheTtl = cacheTtl;

		set.SaveFormat = (new string[] { "jpg", "png" }.Contains(RqValue("save-format")) ? RqValue("save-format") : "jpg");
		
		if (RqValue("file").Contains("..")) {
			set.File = String.Empty;
			return set;
		}

		set.File = HostingEnvironment.MapPath("~/" + RqValue("file").Replace("\\", "/"));
		
		if (String.IsNullOrEmpty(RqValue(prefix + "width"))) {
			if (int.TryParse(RqValue("width"), out iTry))
				set.Width = iTry;
		} else {
			if (int.TryParse(RqValue(prefix + "width"), out iTry))
				set.Width = iTry;
		}
		if (String.IsNullOrEmpty(RqValue(prefix + "height"))) {
			if (int.TryParse(RqValue("height"), out iTry))
				set.Height = iTry;
		} else {
			if (int.TryParse(RqValue(prefix + "height"), out iTry))
				set.Height = iTry;
		}

		/*
		 concat?
		 */
		if (RqValue("concat") != "") {
			string[] vals = RqValue("concat").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (vals.Length == 5) {
				int iTryConcat = 0;
				set.File = vals[0];
				int.TryParse(vals[1], out iTryConcat);
				set.Width = iTryConcat;
				int.TryParse(vals[2], out iTryConcat);
				set.Height = iTryConcat;
				set.Crop = (vals[3] == "1");
				set.CacheActive = (vals[4] == "1");
			}
		}

		if (int.TryParse(RqValue(prefix + "adjust-brightness"), out iTry))
			set.AdjustBrightness = iTry;
		if (int.TryParse(RqValue(prefix + "adjust-contrast"), out iTry))
			set.AdjustContrast = iTry;
		if (int.TryParse(RqValue(prefix + "adjust-desaturate"), out iTry))
			set.AdjustDesaturate = Math.Max(Math.Min(iTry, 100), 0);
		if (int.TryParse(RqValue(prefix + "adjust-blue"), out iTry))
			set.AdjustBlue = iTry;
		if (int.TryParse(RqValue(prefix + "adjust-green"), out iTry))
			set.AdjustGreen = iTry;
		if (int.TryParse(RqValue(prefix + "adjust-red"), out iTry))
			set.AdjustRed = iTry;

		set.AdjustGrayscale = (RqValue(prefix + "adjust-grayscale") == "1");
		set.AdjustInvert = (RqValue(prefix + "adjust-invert") == "1");
		set.AdjustSepia = (RqValue(prefix + "adjust-sepia") == "1");

		set.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
		set.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
		if (RqValue(prefix + "caption-color").Length == 8) {
			int a = int.Parse(RqValue(prefix + "caption-color"), System.Globalization.NumberStyles.AllowHexSpecifier);
			set.CaptionColor = System.Drawing.Color.FromArgb(a);
		}
		set.CaptionFontFamily = RqValue(prefix + "caption-font-family");
		set.CaptionFontStyle = (new string[] { "bold", "italic" }.Contains(RqValue(prefix + "caption-font-style")) ? RqValue(prefix + "caption-font-style") : "");
		if (!int.TryParse(RqValue(prefix + "caption-font-size"), out iTry) || iTry <= 0)
			set.CaptionFontSize = 12;
		else
			set.CaptionFontSize = iTry;

		if (int.TryParse(RqValue(prefix + "caption-offset-x"), out iTry))
			set.CaptionOffsetX = iTry;
		if (int.TryParse(RqValue(prefix + "caption-offset-y"), out iTry))
			set.CaptionOffsetY = iTry;

		set.CaptionText = HttpUtility.UrlDecode(RqValue(prefix + "caption-text"));
		if (!int.TryParse(RqValue(prefix + "caption-padding-horizontal"), out iTry) || iTry < 0)
			set.CaptionPaddingHorizontal = 10;
		else
			set.CaptionPaddingHorizontal = iTry;

		if (!int.TryParse(RqValue(prefix + "caption-padding-vertical"), out iTry) || iTry < 0)
			set.CaptionPaddingVertical = 10;
		else
			set.CaptionPaddingVertical = iTry;

		set.Crop = (RqValue(prefix + "crop") == "1");
		set.CropAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
		set.CropValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;

		set.FillAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
		set.FillValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;

		if (int.TryParse(RqValue(prefix + "fill-height"), out iTry))
			set.FillHeight = iTry;
		if (int.TryParse(RqValue(prefix + "fill-padding-horizontal"), out iTry))
			set.FillPaddingHorizontal = iTry;
		if (int.TryParse(RqValue(prefix + "fill-padding-vertical"), out iTry))
			set.FillPaddingVertical = iTry;
		if (int.TryParse(RqValue(prefix + "fill-width"), out iTry))
			set.FillWidth = iTry;


		if (RqValue(prefix + "fill-color").Length == 8) {
			int a = int.Parse(RqValue(prefix + "fill-color"), System.Globalization.NumberStyles.AllowHexSpecifier);
			set.FillColor = System.Drawing.Color.FromArgb(a);
			if (RqValue(prefix + "fill-color").StartsWith("00")) // exception for transparent
				set.FillColor = System.Drawing.Color.Transparent;
		}
		switch (RqValue(prefix + "fill-align")) {
			case "center":
				set.FillAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
				break;
			case "left":
				set.FillAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
				break;
			case "right":
				set.FillAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
				break;
		}
		switch (RqValue(prefix + "fill-valign")) {
			case "bottom":
				set.FillValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
				break;
			case "middle":
				set.FillValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
				break;
			case "top":
				set.FillValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Top;
				break;
		}

		switch (RqValue(prefix + "crop-align")) {
			case "center":
				set.CropAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
				break;
			case "left":
				set.CropAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
				break;
			case "right":
				set.CropAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
				break;
		}
		switch (RqValue(prefix + "crop-valign")) {
			case "bottom":
				set.CropValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
				break;
			case "middle":
				set.CropValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
				break;
			case "top":
				set.CropValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Top;
				break;
		}

		switch (RqValue(prefix + "caption-align")) {
			case "center":
				set.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
				break;
			case "left":
				set.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
				break;
			case "right":
				set.CaptionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
				break;
		}

		switch (RqValue(prefix + "caption-valign")) {
			case "bottom":
				set.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
				break;
			case "middle":
				set.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
				break;
			case "top":
				set.CaptionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Top;
				break;
		}

		switch (RqValue(prefix + "overlay-align")) {
			case "center":
				set.OverlayAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
				break;
			case "left":
				set.OverlayAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
				break;
			case "right":
				set.OverlayAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
				break;
		}

		switch (RqValue(prefix + "overlay-valign")) {
			case "bottom":
				set.OverlayValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
				break;
			case "middle":
				set.OverlayValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
				break;
			case "top":
				set.OverlayValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Top;
				break;
		}
		
		set.OverlayFile = RqValue(prefix + "overlay-file");

		if (int.TryParse(RqValue(prefix + "overlay-x"), out iTry))
			set.OverlayX = iTry;
		if (int.TryParse(RqValue(prefix + "overlay-y"), out iTry))
			set.OverlayY = iTry;
		if (int.TryParse(RqValue(prefix + "overlay-offset-x"), out iTry))
			set.OverlayOffsetX = iTry;
		if (int.TryParse(RqValue(prefix + "overlay-offset-y"), out iTry))
			set.OverlayOffsetY = iTry;
		if (int.TryParse(RqValue(prefix + "overlay-opacity"), out iTry)) { 
			set.OverlayOpacity = iTry;
			set.OverlayOpacity = Math.Max(0, set.OverlayOpacity);
			set.OverlayOpacity = Math.Min(100, set.OverlayOpacity);
		}
		if (RqValue(prefix + "overlay-color").Length == 8) {
			int a = int.Parse(RqValue(prefix + "overlay-color"), System.Globalization.NumberStyles.AllowHexSpecifier);
			set.OverlayColor = System.Drawing.Color.FromArgb(a);
			if (RqValue(prefix + "overlay-color").StartsWith("00")) // exception for transparent
				set.OverlayColor = System.Drawing.Color.Transparent;
		}

		if (int.TryParse(RqValue(prefix + "rotate-angle"), out iTry))
			set.RotateAngle = iTry;

		set.RotateExpandingBounding = (RqValue(prefix + "rotate-expand-bounding") == "1");

		return set;
	}
}