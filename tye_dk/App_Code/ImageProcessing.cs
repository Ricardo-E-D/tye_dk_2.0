using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ImageProcessing
{
	/// <summary>
	/// General class for manipulating images
	/// </summary>
	public class ImageProcessing
    {
		// basic constructor
		public ImageProcessing()
        {
        }

		/// <summary>
		/// Retuns an imageCodesInfo matching the given MIME type 
		/// </summary>
		/// <param name="MimeType">String representation of MIME type to get (ie. "image/jpeg")</param>
		/// <returns>ImageCodecInfo</returns>
        public ImageCodecInfo GetEncoderInfo(string MimeType) {
            ImageCodecInfo Result = null;
            ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageDecoders();
            for(int i = 0; i < Encoders.Length; i++) {
                if(Encoders[i].MimeType == MimeType) {
                    Result = Encoders[i];
                }
            }
			return Result;
        }

		private BitmapData LockBits(Bitmap Image, ImageLockMode LockMode) {
			return Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), LockMode, Image.PixelFormat);
		}

		/// <summary>
		/// Resizes an image to given maximum dimensions and saves to file
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <param name="strSaveFilePath">Full save path</param>
		/// <returns>True if everything went well</returns>
		public Boolean ResizeImageAndSaveToFile(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution, string strSaveFilePath)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = ResizeImage(strSourceImagepath, imgFormat, fmtSaveFormat, intSaveQuality, intTargetResolution);
			if (strBitmap != null) // if memory stream actually contains data
			{
				Bitmap bmp = (Bitmap)Bitmap.FromStream(strBitmap); // create bitmap from stream
				if (fmtSaveFormat == ImageFormat.Jpeg) // if we want to save image as JPG
				{
					ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
					EncoderParameters Params = new EncoderParameters(1); 
					Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality); // set save quality to parameter
					bmp.Save(strSaveFilePath, GetEncoderInfo("image/jpeg"), Params); // save image with correct encoder
					return true;
				}
				else
				{
					bmp.Save(strSaveFilePath, fmtSaveFormat); // save image as given format (anything but JPG)
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Resizes an image to given maximum dimensions and returns a stream
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <returns>True if everything went well</returns>
		public MemoryStream ResizeImageAndSaveToMemory(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = ResizeImage(strSourceImagepath, imgFormat, fmtSaveFormat, intSaveQuality, intTargetResolution);
			if (strBitmap != null) // if memory stream actually contains data
			{
				return strBitmap; // then just return it
			}
			else
			{
				return null; 
			}
		}

		/// <summary>
		/// Method that actually resizes bitmap data
		/// </summary>
		/// <param name="strSourceImagepath">Full path to source image file</param>
		/// <param name="imgFormat">Integer array of width, height</param>
		/// <param name="fmtSaveFormat">Imageformat to use when saving</param>
		/// <param name="intSaveQuality">Integer save quality 0-100 (valid for jpg only)</param>
		/// <param name="intTargetResolution">Integer target resolution (dpi) (both horizontal and vertical)</param>
		/// <returns>Memorystream of processed bitmap data</returns>
		private MemoryStream ResizeImage(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution)
		{
            Bitmap oImage;
            Graphics thumbnail;
			PixelFormat pxfSource;

			double toWidth = Convert.ToDouble(imgFormat[0]);
			double toHeight = Convert.ToDouble(imgFormat[1]);
			double Width = 0.0;
			double Height = 0.0;
			int intSourceResolution = 0;

            double newWidth = 0.0;
			double newHeight = 0.0;
			
			oImage = (Bitmap)Bitmap.FromFile(strSourceImagepath, false);
			
			pxfSource = oImage.PixelFormat;
			Width = oImage.Width;
			Height = oImage.Height;
			string gt = (Width / Height).ToString();
			double ratio = (double)(Width / Height);
			double toRatio = (double)(toWidth / toHeight);

			if(Width > toWidth) {
				newWidth = toWidth;
				newHeight = (int)Math.Floor(newWidth / ratio);
			}
			if(newHeight > toHeight) {
				newHeight = toHeight;
				newWidth = (int)Math.Floor(newHeight * ratio);
			}
                
			if(newWidth <= 0)
				newWidth = Width;
			if(newHeight <= 0) 
				newHeight = Height;

			intSourceResolution = (int)oImage.HorizontalResolution;
			if(intTargetResolution == 0) {
				intTargetResolution = intSourceResolution;
			}

			// Resize image
			Bitmap tmpPic = new Bitmap(oImage, (int)newWidth, (int)newHeight);
			tmpPic.SetResolution(intTargetResolution, intTargetResolution);
			tmpPic.Palette = oImage.Palette;

			thumbnail = System.Drawing.Graphics.FromImage(tmpPic);
			thumbnail.InterpolationMode = InterpolationMode.HighQualityBicubic;
			thumbnail.PixelOffsetMode = PixelOffsetMode.HighQuality;
            thumbnail.SmoothingMode = SmoothingMode.HighQuality;
			thumbnail.DrawImage(oImage, 0, 0, (int)newWidth, (int)newHeight);

			MemoryStream stream = new MemoryStream();

            if(fmtSaveFormat == ImageFormat.Jpeg) {
                ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters Params = new EncoderParameters(1);
                Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality);
				tmpPic.Save(stream, GetEncoderInfo("image/jpeg"), Params);
			} else {
				tmpPic.Save(stream, fmtSaveFormat);
			}

			// cleanup objects and return data
            thumbnail.Dispose();
			thumbnail = null;
			oImage.Dispose();
			oImage = null;
			return stream;
        }

		/// <summary>
		/// Crops an image to given maximum dimensions and saves to file
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <param name="strSaveFilePath">Full save path</param>
		/// <returns>True if everything went well</returns>
		public Boolean CropImageAndSaveToFile(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution, string strSaveFilePath)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = CropImage(strSourceImagepath, imgFormat, fmtSaveFormat, intSaveQuality, intTargetResolution);
			if (strBitmap != null) // if memory stream actually contains data
			{
				Bitmap bmp = (Bitmap)Bitmap.FromStream(strBitmap); // create bitmap from stream
				if (fmtSaveFormat == ImageFormat.Jpeg) // if we want to save image as JPG
				{
					ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
					EncoderParameters Params = new EncoderParameters(1);
					Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality); // set save quality to parameter
					bmp.Save(strSaveFilePath, GetEncoderInfo("image/jpeg"), Params); // save image with correct encoder
					return true;
				} else {
					bmp.Save(strSaveFilePath, fmtSaveFormat); // save image as given format (anything but JPG)
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Crops an image to given maximum dimensions and returns a stream
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <returns>True if everything went well</returns>
		public MemoryStream CropImageAndSaveToMemory(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = CropImage(strSourceImagepath, imgFormat, fmtSaveFormat, intSaveQuality, intTargetResolution);
			if (strBitmap != null) // if memory stream actually contains data
			{
				return strBitmap; // then just return it
			} else {
				return null;
			}
		}

		/// <summary>
		/// Method that actually crops bitmap data
		/// </summary>
		/// <param name="strSourceImagepath">Full path to source image file</param>
		/// <param name="imgFormat">Integer array of width, height</param>
		/// <param name="fmtSaveFormat">Imageformat to use when saving</param>
		/// <param name="intSaveQuality">Integer save quality 0-100 (valid for jpg only)</param>
		/// <param name="intTargetResolution">Integer target resolution (dpi) (both horizontal and vertical)</param>
		/// <returns>Memorystream of processed bitmap data</returns>
		private MemoryStream CropImage(string strSourceImagepath, int[] imgFormat, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution)
		{
			Bitmap oImage;
            Graphics thumbnail;
			PixelFormat pxfSource;

			double toWidth = Convert.ToDouble(imgFormat[0]);
			double toHeight = Convert.ToDouble(imgFormat[1]);
			double Width = 0.0;
			double Height = 0.0;
			int starty = 0;
			int startx = 0;

			int intSourceResolution = 0;

			double newWidth = 0;
			double newHeight = 0;

			//oImage = (Bitmap)System.Drawing.Image.FromFile(strSourceImagepath, false);
			oImage = (Bitmap)Bitmap.FromFile(strSourceImagepath, false);

			pxfSource = oImage.PixelFormat;
			Width = oImage.Width;
			Height = oImage.Height;
			double ratio = Width / Height;
			MemoryStream stream = new MemoryStream();

			try {
				Bitmap bmpImage;
				Bitmap bmpCrop;
				Graphics gphCrop;
                // Evaluate ratio
				if (Convert.ToInt16(Width / Height) >= Convert.ToInt16(toWidth / toHeight))
				{
					if(Height >= toHeight) {
                        newHeight = toHeight;
						newWidth = (int)Math.Floor(ratio * newHeight);
                    } else {
						newHeight = Height;
						newWidth = (int)Math.Floor(ratio * newHeight);
                    }
                    if (newWidth >= toWidth) {
						starty = 0;
						startx = (int)Math.Floor((newWidth - toWidth) / 2);
					} 
				} else {
				   if(Width >= toWidth) {
					   newWidth = toWidth;
					   newHeight = (int)Math.Floor(newWidth / ratio);
				   } else {
					   newWidth = Width;
					   newHeight = (int)Math.Floor(newWidth / ratio);
				   }
                   if(newHeight >= toHeight) {
					   startx = 0;
					   starty = (int)Math.Floor((newHeight - toHeight) / 2);
				   }
				}
                //Resize and crop
				Bitmap tmpPic = new Bitmap(oImage, (int)newWidth, (int)newHeight);
				Rectangle recCrop;
				Rectangle recDest;
				intSourceResolution = (int)oImage.HorizontalResolution;
				if(intTargetResolution == 0) {
					intTargetResolution = intSourceResolution;
				}

                tmpPic.SetResolution(intTargetResolution, intTargetResolution);
                thumbnail = Graphics.FromImage(tmpPic);
                thumbnail.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnail.PixelOffsetMode = PixelOffsetMode.HighQuality;
                thumbnail.SmoothingMode = SmoothingMode.HighQuality;
				thumbnail.DrawImage(oImage, 0, 0, (int)newWidth, (int)newHeight);
                if(newWidth > toWidth)
					newWidth = toWidth;
                if(newHeight > toHeight)
					newHeight = toHeight;
                bmpImage = new Bitmap(tmpPic);
				recCrop = new Rectangle(startx, starty, (int)newWidth, (int)newHeight);
                bmpCrop = new Bitmap(recCrop.Width, recCrop.Height, bmpImage.PixelFormat);
                gphCrop = Graphics.FromImage(bmpCrop);
                gphCrop.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gphCrop.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gphCrop.SmoothingMode = SmoothingMode.HighQuality;
				recDest = new Rectangle(0, 0, (int)newWidth, (int)newHeight);
                gphCrop.DrawImage(bmpImage, recDest, recCrop.X, recCrop.Y, recCrop.Width, recCrop.Height, GraphicsUnit.Pixel);

                if(fmtSaveFormat == ImageFormat.Jpeg) {
                    ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
                    EncoderParameters Params = new EncoderParameters(1);
                    Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality);
					bmpCrop.Save(stream, GetEncoderInfo("image/jpeg"), Params);
                } else {
					bmpCrop.Save(stream, fmtSaveFormat);
                }
			}
			catch(Exception) {
				return null;
			}
			oImage.Dispose();
			oImage = null;
			return stream;
		}

		/// <summary>
		/// Crops an image to given maximum dimensions and saves to file
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <param name="strSaveFilePath">Full save path</param>
		/// <returns>True if everything went well</returns>
		public Boolean CropImageByCoordinatesAndSaveToFile(string strSourceImagepath, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution, string strSaveFilePath, int StartX, int StartY, int Width, int Height)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = cropImageByCoordinates(strSourceImagepath, fmtSaveFormat, intSaveQuality, intTargetResolution, StartX, StartY, Width, Height);
			if (strBitmap != null) // if memory stream actually contains data
			{
				Bitmap bmp = (Bitmap)Bitmap.FromStream(strBitmap); // create bitmap from stream
				if (fmtSaveFormat == ImageFormat.Jpeg) // if we want to save image as JPG
				{
					ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
					EncoderParameters Params = new EncoderParameters(1);
					Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality); // set save quality to parameter
					bmp.Save(strSaveFilePath, GetEncoderInfo("image/jpeg"), Params); // save image with correct encoder
					return true;
				} else {
					bmp.Save(strSaveFilePath, fmtSaveFormat); // save image as given format (anything but JPG)
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Crops an image to given maximum dimensions and returns a stream
		/// </summary>
		/// <param name="strSourceImagepath">Filepath to source image</param>
		/// <param name="imgFormat">Target width and height</param>
		/// <param name="fmtSaveFormat">Target image save format</param>
		/// <param name="intSaveQuality">Save quality (only applies to JPG)</param>
		/// <param name="intTargetResolution">Target resolution. If value is 0 (zero) the source image resolution is used</param>
		/// <returns>True if everything went well</returns>
		public MemoryStream CropImageByCoordinatesAndSaveToMemory(string strSourceImagepath, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution, int StartX, int StartY, int Width, int Height)
		{
			// create new memory stream to hold return data from resize method
			MemoryStream strBitmap = cropImageByCoordinates(strSourceImagepath, fmtSaveFormat, intSaveQuality, intTargetResolution, StartX, StartY, Width, Height );
			if (strBitmap != null) // if memory stream actually contains data
			{
				return strBitmap; // then just return it
			} else {
				return null;
			}
		}

		/// <summary>
		/// Method that actually crops bitmap data
		/// </summary>
		/// <param name="strSourceImagepath">Full path to source image file</param>
		/// <param name="imgFormat">Integer array of width, height</param>
		/// <param name="fmtSaveFormat">Imageformat to use when saving</param>
		/// <param name="intSaveQuality">Integer save quality 0-100 (valid for jpg only)</param>
		/// <param name="intTargetResolution">Integer target resolution (dpi) (both horizontal and vertical)</param>
		/// <returns>Memorystream of processed bitmap data</returns>
		private MemoryStream cropImageByCoordinates(string strSourceImagepath, ImageFormat fmtSaveFormat, int intSaveQuality, int intTargetResolution, int StartX, int StartY, int TargetWidth, int TargetHeight)
		{
			Bitmap oImage;
			PixelFormat pxfSource;
			double Width = 0.0;
			double Height = 0.0;
			int intSourceResolution = 0;

			oImage = (Bitmap)Bitmap.FromFile(strSourceImagepath, false);

			pxfSource = oImage.PixelFormat;
			Width = oImage.Width;
			Height = oImage.Height;
			
			MemoryStream stream = new MemoryStream();

			if (StartX + TargetWidth > (int)Width)
				StartX = (int)Width - TargetWidth;

			if (StartY + TargetHeight > (int)Height)
				StartY = (int)Height - TargetHeight;

			if (StartX < 0 || StartY < 0)
				throw new ApplicationException("Image dimensions are wrong");

			try {
				Bitmap bmpImage;
				Bitmap bmpCrop;
				Graphics gphCrop;
				//Resize and crop
				Bitmap tmpPic = new Bitmap(oImage, (int)Width, (int)Height);
				Rectangle recCrop;
				Rectangle recDest;
				intSourceResolution = (int)oImage.HorizontalResolution;
				if (intTargetResolution == 0) {
					intTargetResolution = intSourceResolution;
				}

				tmpPic.SetResolution(intTargetResolution, intTargetResolution);
				bmpImage = new Bitmap(tmpPic);
				recCrop = new Rectangle(StartX, StartY, (int)TargetWidth, (int)TargetHeight);
				bmpCrop = new Bitmap(recCrop.Width, recCrop.Height, bmpImage.PixelFormat);
				gphCrop = Graphics.FromImage(bmpCrop);
				gphCrop.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gphCrop.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gphCrop.SmoothingMode = SmoothingMode.HighQuality;
				recDest = new Rectangle(0, 0, (int)TargetWidth, (int)TargetHeight); // define size of cropping rectangle (start x and y is ALWAYS 0(zero);
				gphCrop.DrawImage(bmpImage, recDest, recCrop.X, recCrop.Y, recCrop.Width, recCrop.Height, GraphicsUnit.Pixel);

				if (fmtSaveFormat == ImageFormat.Jpeg) {
					ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
					EncoderParameters Params = new EncoderParameters(1);
					Params.Param[0] = new EncoderParameter(Encoder.Quality, intSaveQuality);
					bmpCrop.Save(stream, GetEncoderInfo("image/jpeg"), Params);
				} else {
					bmpCrop.Save(stream, fmtSaveFormat);
				}
			}
			catch (Exception) {
				return null;
			}
			oImage.Dispose();
			oImage = null;
			return stream;
		}

		public Size GetImageSize(string filepath)
		{
			if (File.Exists(filepath))
			{
				Bitmap oImage = (Bitmap)System.Drawing.Image.FromFile(filepath, false);
				int Width = oImage.Width;
				int Height = oImage.Height;
				oImage.Dispose();
				Size z = new Size(Width, Height);
				return z;
			}
			else
			{
				return new Size(0, 0);
			}
		}
		public List<int> GetImageSizeAndResolution(string filepath)
		{
			if (File.Exists(filepath))
			{
				Bitmap oImage = (Bitmap)System.Drawing.Image.FromFile(filepath, false);
				int Width = oImage.Width;
				int Height = oImage.Height;
				int Resolution = Convert.ToInt16(oImage.HorizontalResolution);
				oImage.Dispose();
				List<int> lstOutput = new List<int>();
				lstOutput.Add(Width);
				lstOutput.Add(Height);
				lstOutput.Add(Resolution);
				return lstOutput;
			}
			else
			{
				return new List<int>();
			}
		}


		/// <summary>
		/// No use at the moment (actually I've forgotten what it's intended for, but method went with the grayscale and negative methods) (found 'em on the internet ;-) )
		/// </summary>
		/// <param name="img"></param>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="alpha"></param>
		/// <returns></returns>
		private Boolean translate(System.Drawing.Image img, Single red, Single green, Single blue, Single alpha) {
			Single sr;
			Single sg;
			Single sb;
			Single sa;
			
            // normalize the color components to 1
            sr = red / 255;
            sg = green / 255;
            sb = blue / 255;
            sa = alpha / 255;

            // create the color matrix
            ColorMatrix cm = new ColorMatrix(new Single[][] {new Single[] {1, 0, 0, 0, 0}, new Single[] {0, 1, 0, 0, 0}, new Single[] {0, 0, 1, 0, 0}, new Single[] {0, 0, 0, 1, 0}, new Single[] {sr, sg, sb, sa, 1} } );
			
            // apply the matrix to the image
            return draw_adjusted_image(img, cm);

		}
		
		/// <summary>
		/// Applies color matrixes to images (ie. manipulates color)
		/// </summary>
		/// <param name="img">Image object to manipulate</param>
		/// <param name="cm">Color Matrix to apply</param>
		/// <returns></returns>
		private Boolean draw_adjusted_image(System.Drawing.Image img, ColorMatrix cm)
		{
			try {
				Bitmap bmp = new Bitmap(img); //create a copy of the source image 
				ImageAttributes imgattr = new ImageAttributes();
				Rectangle rc = new Rectangle(0,0, img.Width, img.Height);
				Graphics g = Graphics.FromImage(img);
				
				// associate the ColorMatrix object with an ImageAttributes object
				imgattr.SetColorMatrix(cm);
				// draw the copy of the source image back over the original image, 
				g.DrawImage(bmp, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);
				// applying the ColorMatrix
				g.Dispose();
                return true;
			   }
            catch(Exception) {
                return false;
			}
		}

		/// <summary>
		/// Manipulates an image to grayscale
		/// </summary>
		/// <param name="img">Image object</param>
		/// <returns>Image</returns>
		private Boolean translateToGrayscaleImage(System.Drawing.Image img)
		{
			Single[] m = new Single[] { 2, 3 };
			ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { 0.299F, 0.299F, 0.299F, 0, 0 }, new float[] { 0.587F, 0.587F, 0.587F, 0, 0 }, new float[] { 0.114F, 0.114F, 0.114F, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
	        return draw_adjusted_image(img, cm);
		}

		/// <summary>
		/// Manipulates an image to negative colors
		/// </summary>
		/// <param name="img">Image object</param>
		/// <returns>Image</returns>
		private Boolean translateToNegativeImage(System.Drawing.Image img)
		{
			ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { -1, 0, 0, 0, 0 }, new float[] { 0, -1, 0, 0, 0 }, new float[] { 0, 0, -1, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
			return draw_adjusted_image(img, cm);
		}
        
    }
}