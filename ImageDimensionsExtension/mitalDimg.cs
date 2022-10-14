
using System;
using System.Web;
using System.Drawing;
using System.Linq;
using System.Web.Hosting;
using System.Collections.Generic;

public class mitalDimg : IHttpHandler {
	private string RqValue(string Variable) {
		if (HttpContext.Current.Request.QueryString[Variable] != null)
			return HttpContext.Current.Request.QueryString[Variable];
		else
			return String.Empty;
	}

	private monosolutions.Utils.Media.Image.ImageProcessing ip = new monosolutions.Utils.Media.Image.ImageProcessing();

	public void ProcessRequest(HttpContext context) {
		ProcessParser pp = new ProcessParser(context);
		List<ProcessStep> steps = pp.Parse();
		Bitmap bmpb = null;

		context.Response.Buffer = true;
		context.Response.Clear();

		ip.SaveFormat = monosolutions.Utils.Media.Image.ImageProcessing.Format.Jpeg;
		context.Response.ContentType = "image/jpg";
		if (steps.Count > 0 && steps.First().Settings.SaveFormat == "png") {
			context.Response.ContentType = "image/png";
			ip.SaveFormat = monosolutions.Utils.Media.Image.ImageProcessing.Format.Png;
		}

		// <eval caching>
		if(steps.Count > 1) {
			var firstStep = steps.First();
			if(firstStep.Settings.CacheActive && String.IsNullOrEmpty(firstStep.Settings.CacheFilename)) {
				steps.Select(n => n.Settings.CacheActive = false).ToList();
			}
		}
		// </eval caching>

		foreach (ProcessStep step in steps) {
			if (step.Settings.FillColor == System.Drawing.Color.Transparent) {
				string s = "";
			}
			
			bmpb = processImage(step, context, (steps.Count > 1));
			if (step.Settings.FillColor == System.Drawing.Color.Transparent) {
				string s = "";
			}

		}
		
		if (steps.First().Settings.CacheActive) {
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(steps.First().Settings.CacheTtl));
			//context.Response.Cache.SetETag(strCacheFilepath);
		}
		
		if (bmpb != null) { 
			if (steps.First().Settings.SaveFormat == "png")
				bmpb.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
			else
				bmpb.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
		}
		ip.Dispose();
		context.Response.End();
	}

	private Bitmap processImage(ProcessStep step, HttpContext context, bool forceProcess = false) {

		Settings set = step.Settings;

		bool blnProcess = true;
		string strCacheFilepath = String.Empty;

		if (!System.IO.File.Exists(set.File))
			return null; // todo: fallback

		// <test processing required>
		if (set.CacheActive) {
			if (!System.IO.Directory.Exists(HostingEnvironment.MapPath("~/mital.cache")))
				System.IO.Directory.CreateDirectory(HostingEnvironment.MapPath("~/mital.cache"));

			if (!String.IsNullOrEmpty(set.CacheFilename))
				strCacheFilepath = System.IO.Path.Combine(HostingEnvironment.MapPath("~/mital.cache"), set.CacheFilename);
			else { 
				strCacheFilepath = HostingEnvironment.MapPath("~/mital.cache");
				string strCacheFilename = set.File.Replace(HostingEnvironment.MapPath("~/"), "").Replace("\\", "_").Replace("/", "_");
				string strExt = System.IO.Path.GetExtension(strCacheFilename);
				strCacheFilename = strCacheFilename.Remove(strCacheFilename.LastIndexOf("."));
				strCacheFilename += "_" 
									+ (set.FillWidth > 0 ? set.FillWidth : set.Width) 
									+ "x" 
									+ (set.FillHeight > 0 ? set.FillHeight : set.Height) 
									+ (set.Crop ? "_c" : "") 
									+ strExt;
				strCacheFilepath = System.IO.Path.Combine(strCacheFilepath, strCacheFilename);
			}
			if (System.IO.File.Exists(strCacheFilepath)) {
				System.IO.FileInfo fiOrg = new System.IO.FileInfo(set.File);
				System.IO.FileInfo fiCache = new System.IO.FileInfo(strCacheFilepath);
				if (fiOrg.LastWriteTimeUtc == fiCache.LastWriteTimeUtc) {
					if (set.CacheTtl > 0) {
						if (fiCache.LastWriteTimeUtc.AddMinutes(set.CacheTtl) < DateTime.UtcNow)
							blnProcess = false;
					} else
						blnProcess = false;
				} else {
					if (set.CacheTtl > 0) {
						if (fiOrg.LastWriteTimeUtc.AddMinutes(set.CacheTtl) > fiCache.LastWriteTimeUtc)
							blnProcess = false;
					}
				}
			}
		}
		// </test processing required>

		Bitmap bmpb = null;

		// <processing>
		if (blnProcess || forceProcess) {
			ip.Filename = set.File;
			
			ip.CropAlign = set.CropAlign;
			ip.CropValign = set.CropValign;

			// fallback-file
			// * width (integer)
			// * height (integer)
			// * file (string)
			// * crop (boolean -> 0 or 1)
			// * save-format (jpg || png) - default jpg
			// * fill-width
			// * fill-height
			// * fill-padding-horizontal
			// * fill-padding-vertical
			// * fill-color (transparent || hex-value)
			// * fill-align (left || center || right) - default center
			// * fill-valign (top || middle || bottom) - default middle
			// * crop-align (left || center || right) - default center 
			// * crop-valign (top || middle || bottom) - default middle
			// crop-offset-x 
			// crop-offset-y
			// * caption-text
			// * caption-font-family
			// * caption-font-size
			// * caption-font-style (italic || bold) - default none
			// * caption-align (left || center || right) - default left 
			// * caption-valign (top || middle || bottom) - default bottom
			// [caption-bgcolor]
			// * caption-color
			// * caption-padding-vertical
			// * caption-offset-x (integer)
			// * caption-offset-y (integer)
			// * caption-padding-horizontal
			// * adjust-brightness (-100 - 100) default 0
			// * adjust-contrast (-100 - 100) default 0
			// * adjust-grayscale
			// * adjust-sepia
			// * rotate-angle
			// * rotate-expand-bounding

			// allow resize when only one value is present
			if (set.Width > 0 && set.Height == 0) {
				Size size = ip.GetImageSize(ip.Filename);
				double ratio = (double)((double)size.Width / (double)size.Height);
				set.Height = (int)((double)set.Width * ratio);
			}
			if (set.Width == 0 && set.Height > 0) {
				Size size = ip.GetImageSize(ip.Filename);
				double ratio = (double)((double)size.Width / (double)size.Height);
				set.Width = (int)((double)set.Height * ratio);
			}

			//if (RqValue("fake") == "mital") {
				if (set.Crop)
					ip.Crop(set.Width, set.Height);
				else {
					ip.Resize(set.Width, set.Height);
				}
			//} else {
			//   string s = "";
			//   return null;
			//}

			//if (set.AdjustDesaturate > 0)
			//   ip.Desaturate2(set.AdjustDesaturate);
			if (set.AdjustGrayscale)
				ip.Grayscale();
			if (set.AdjustInvert)
				ip.Invert();
			if (set.AdjustSepia)
				ip.Sepia();

			if (set.AdjustBlue != 0 || set.AdjustGreen != 0 || set.AdjustRed != 0)
				ip.translate(set.AdjustRed, set.AdjustGreen, set.AdjustBlue, 0);

			bool blnDoOverlay = !String.IsNullOrEmpty(set.OverlayFile) && System.IO.File.Exists(HostingEnvironment.MapPath("~/" + set.OverlayFile));
			blnDoOverlay = (blnDoOverlay || set.OverlayColor != Color.Transparent);
			if (blnDoOverlay) {
				ip.OverlayOpacity = set.OverlayOpacity;
				ip.OverlayX = set.OverlayX;
				ip.OverlayY = set.OverlayY;
				ip.OverlayOffsetX = set.OverlayOffsetX;
				ip.OverlayOffsetY = set.OverlayOffsetY;
				ip.OverlayAlign = set.OverlayAlign;
				ip.OverlayValign = set.OverlayValign;
				ip.Overlay(HostingEnvironment.MapPath("~/" + set.OverlayFile));
				if(set.OverlayColor != Color.Transparent)
					ip.OverlayColor(set.OverlayColor);
			}

			set.AdjustBrightness = Math.Max(set.AdjustBrightness, -100);
			set.AdjustBrightness = Math.Min(set.AdjustBrightness, 100);
			if (set.AdjustBrightness != 0)
				ip.SetBrightness((float)set.AdjustBrightness / 100f);

			set.AdjustContrast = Math.Max(set.AdjustContrast, -100);
			set.AdjustContrast = Math.Min(set.AdjustContrast, 100);
			if (set.AdjustContrast != 0) {
				float ffContrast = 1.0f - (((float)set.AdjustContrast / -100f));
				ip.SetContrast(ffContrast);
			}

			ip.FillAlign = set.FillAlign;
			ip.FillValign = set.FillValign;
			if (set.FillWidth > 0 || set.FillHeight > 0) {
				if (set.FillPaddingHorizontal > 0 || set.FillPaddingVertical > 0)
					ip.Fill((set.FillWidth == 0 ? set.Width : set.FillWidth), (set.FillHeight == 0 ? set.Height : set.FillHeight), set.FillColor, set.FillPaddingHorizontal, set.FillPaddingVertical);
				else
					ip.Fill((set.FillWidth == 0 ? set.Width : set.FillWidth), (set.FillHeight == 0 ? set.Height : set.FillHeight), set.FillColor);
			}
			ip.CaptionAlign = set.CaptionAlign;
			ip.CaptionValign = set.CaptionValign;
			ip.CaptionPaddingHorizontal = set.CaptionPaddingHorizontal;
			ip.CaptionPaddingVertical = set.CaptionPaddingVertical;
			ip.CaptionOffsetX = set.CaptionOffsetX;
			ip.CaptionOffsetY = set.CaptionOffsetY;

			if (!String.IsNullOrEmpty(set.CaptionText)) {
				if (set.CaptionFontStyle == "italic")
					ip.DrawCaption(set.CaptionText, set.CaptionColor, set.CaptionFontFamily, set.CaptionFontSize, FontStyle.Italic);
				else if (set.CaptionFontStyle == "bold")
					ip.DrawCaption(set.CaptionText, set.CaptionColor, set.CaptionFontFamily, set.CaptionFontSize, FontStyle.Bold);
				else
					ip.DrawCaption(set.CaptionText, set.CaptionColor, set.CaptionFontFamily, set.CaptionFontSize);
			}

			if (set.RotateAngle != 0)
				ip.Rotate(set.RotateAngle, set.RotateExpandingBounding);

			if (set.CacheActive && !forceProcess) {
				ip.Save(strCacheFilepath);
				System.IO.File.SetLastWriteTimeUtc(strCacheFilepath, (new System.IO.FileInfo(set.File)).LastWriteTimeUtc); 
				bmpb = (Bitmap)Bitmap.FromFile(strCacheFilepath);
			} else
				bmpb = (Bitmap)Bitmap.FromStream(ip.Save());
		} else { 
			// caching active
			if (System.IO.File.Exists(strCacheFilepath))
				bmpb = (Bitmap)Bitmap.FromFile(strCacheFilepath);
		}

		return bmpb;
	}

	public bool IsReusable {
		get {
			return false;
		}
	}

}