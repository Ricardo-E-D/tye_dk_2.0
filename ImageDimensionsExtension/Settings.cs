using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Settings
/// </summary>
public class Settings
{
	public Settings()
	{
	}

	public string File { get; set; }
	public int Height { get; set; }
	public int Width { get; set; }

	public string CacheFilepath { get; set; }

	public bool CacheActive { get; set; }
	private int _cacheTtl = (60 * 60 * 24 * 2);
	public int CacheTtl { get { return _cacheTtl; } set { _cacheTtl = value; } }
	public string CacheFilename { get; set; }

	private string _saveFormat = "jpg";
	public string SaveFormat { get { return _saveFormat; } set { _saveFormat = value; } }

	public int AdjustBrightness { get; set; }
	public int AdjustContrast { get; set; }
	public int AdjustDesaturate { get; set; }
	public bool AdjustGrayscale { get; set; }
	public bool AdjustInvert { get; set; }
	public bool AdjustSepia { get; set; }
	private int _adjustBlue = 0;
	public int AdjustBlue {
		get { return _adjustBlue; }
		set { 
			_adjustBlue = value;
			if (_adjustBlue < -255)
				_adjustBlue = -255;
			if (_adjustBlue > 255)
				_adjustBlue = 255;
		}
	}
	private int _adjustGreen = 0;
	public int AdjustGreen {
		get { return _adjustGreen; }
		set {
			_adjustGreen = value;
			if (_adjustGreen < -255)
				_adjustGreen = -255;
			if (_adjustGreen > 255)
				_adjustGreen = 255;
		}
	}
	private int _adjustRed = 0;
	public int AdjustRed {
		get { return _adjustRed; }
		set {
			_adjustRed = value;
			if (_adjustRed < -255)
				_adjustRed = -255;
			if (_adjustRed > 255)
				_adjustRed = 255;
		}
	}

	private monosolutions.Utils.Media.Image.ImageProcessing.Alignment _captionAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Left;
	private monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment _captionValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
	public monosolutions.Utils.Media.Image.ImageProcessing.Alignment CaptionAlign { get { return _captionAlign; } set { _captionAlign = value; } }
	public monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment CaptionValign { get { return _captionValign; } set { _captionValign = value; } }

	private System.Drawing.Color _captionColor = System.Drawing.Color.FromArgb(int.Parse("ff333333", System.Globalization.NumberStyles.AllowHexSpecifier));
	public System.Drawing.Color CaptionColor { get { return _captionColor; } set { _captionColor = value; } }
	private string _captionFontFamily = "Arial";
	public string CaptionFontFamily { get { return _captionFontFamily; } set { _captionFontFamily = (String.IsNullOrEmpty(value) ? "Arial" : value); } }
	public string CaptionFontStyle { get; set; }
	public int CaptionFontSize { get; set; }
	public int CaptionOffsetX { get; set; }
	public int CaptionOffsetY { get; set; }
	public int CaptionPaddingHorizontal { get; set; }
	public int CaptionPaddingVertical { get; set; }
	public string CaptionText { get; set; }

	public bool Crop { get; set; }
	private monosolutions.Utils.Media.Image.ImageProcessing.Alignment _cropAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
	private monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment _cropValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
	public monosolutions.Utils.Media.Image.ImageProcessing.Alignment CropAlign { get { return _cropAlign; } set { _cropAlign = value; } }
	public monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment CropValign { get { return _cropValign; } set { _cropValign = value; } }

	private monosolutions.Utils.Media.Image.ImageProcessing.Alignment _fillAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Center;
	private monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment _fillValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Middle;
	public monosolutions.Utils.Media.Image.ImageProcessing.Alignment FillAlign { get { return _fillAlign; } set { _fillAlign = value; } }
	public monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment FillValign { get { return _fillValign; } set { _fillValign = value; } }
	private System.Drawing.Color _fillColor = System.Drawing.Color.FromArgb(int.Parse("ffffffff", System.Globalization.NumberStyles.AllowHexSpecifier));
	public System.Drawing.Color FillColor { get { return _fillColor; } set { _fillColor = value; } }
	public int FillHeight { get; set; }
	public int FillPaddingHorizontal { get; set; }
	public int FillPaddingVertical { get; set; }
	public int FillWidth { get; set; }

	private monosolutions.Utils.Media.Image.ImageProcessing.Alignment _overlayAlign = monosolutions.Utils.Media.Image.ImageProcessing.Alignment.Right;
	private monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment _overlayValign = monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment.Bottom;
	public monosolutions.Utils.Media.Image.ImageProcessing.Alignment OverlayAlign { get { return _overlayAlign; } set { _overlayAlign = value; } }
	public monosolutions.Utils.Media.Image.ImageProcessing.VerticalAlignment OverlayValign { get { return _overlayValign; } set { _overlayValign = value; } }
	public int OverlayX { get; set; }
	public int OverlayY { get; set; }
	public int OverlayOffsetX { get; set; }
	public int OverlayOffsetY { get; set; }
	private int _overlayOpacity = 100;
	public int OverlayOpacity { get { return _overlayOpacity; } set { _overlayOpacity = value; } }
	public string OverlayFile { get; set; }
	private System.Drawing.Color _overlayColor = System.Drawing.Color.Transparent;
	public System.Drawing.Color OverlayColor { get { return _overlayColor; } set { _overlayColor = value; } }

	public int RotateAngle { get; set; }
	public bool RotateExpandingBounding { get; set; }
}