using System;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.UI.HtmlControls;
using tye.exceptions;

namespace tye
{

	public class Files : System.Web.UI.Page
	{
		public Files()
		{

		}

		private string strName;
		private string strFileType;
		private int intLanguageId;
		private HttpPostedFile objFileInput;
		private DateTime datAddedTime;
		private string strDescription;
		private const float fltImgMaxSize = (500 * 1024);
		private const float fltPdfMaxSize = (5000 * 1024);
		private const float fltDocMaxSize = (500 * 1024);
		private UploadError ue = new UploadError();
		private string strReplacedFilename;
		public static string strServerFilePath = "../upload/";
		//public static string strServerSavePath = @"C:\TrainYourEyes\upload\";
        public static string strServerSavePath = "";
		public static string strCtl = "ctl5";

		public string StrName
		{ 
			get
			{
				return strName;
			}
			set
			{
				strName = value;			
			}
		}

		public string StrFileType
		{ 
			get
			{
				return strFileType;
			}
			set
			{
				strFileType = value;			
			}
		}

		public int IntLanguageId
		{ 
			get
			{
				return intLanguageId;
			}
			set
			{
				intLanguageId = value;			
			}
		}

		public HttpPostedFile ObjFileInput
		{ 
			get
			{
				return objFileInput;
			}
			set
			{
				objFileInput = value;			
			}
		}

		public DateTime DatAddedTime
		{ 
			get
			{
				return datAddedTime;
			}
			set
			{
				datAddedTime = value;			
			}
		}

		public string StrDescription
		{ 
			get
			{
				return strDescription;
			}
			set
			{
				strDescription = value;			
			}
		}
		
		public UploadError upload()
		{
			strReplacedFilename = objFileInput.FileName.Substring(ObjFileInput.FileName.LastIndexOf("\\")+1).ToString().Replace(" ","_").Replace("æ","ae").Replace("ø","oe").Replace("å","aa").Replace("'","").Replace("''","").Replace("^","_");

			int i = 1;

			while(File.Exists(strServerSavePath + strReplacedFilename))
			{
				if (strReplacedFilename.IndexOf("^") != -1)
				{
					strReplacedFilename = strReplacedFilename.Substring(strReplacedFilename.IndexOf("^")+1,strReplacedFilename.Length - strReplacedFilename.IndexOf("^")-1);
				}
				strReplacedFilename = i + "^" + strReplacedFilename;
				i++;
			}

		string[] arrType = objFileInput.ContentType.Split(Convert.ToChar("/"));

			switch (strFileType)
			{
				case "image": //Billeder

                    if (arrType[1] != "gif" && arrType[1] != "jpeg" && arrType[1] != "jpg" && arrType[1] != "pjpeg")
					{
						ue.StrError = "Filen skal være af typen 'JPG' eller 'GIF'" + arrType[1];

						throw ue;
					} 
					else if (objFileInput.ContentLength > fltImgMaxSize)
					{
						ue.StrError = "Filen er for stor, den må max fylde " + fltImgMaxSize + " KB.";
					
						throw ue; 
					} 
					else
					{
						Bitmap image = new Bitmap(ObjFileInput.InputStream);

						if (image.Width > 470 || image.Height > 450)
						{
							ue.StrError = "Billedet er for stort, det må max fylde 470x450 px";

							throw ue;
						}
						else
						{
							
							objFileInput.SaveAs(strServerSavePath + strReplacedFilename);
							
							if(image.Width > 100)
							{
								int intWidth = 100;
								int intHeight = 100;
								
								Size s = new Size(intWidth,intHeight);

								Bitmap thumb = new Bitmap(image,s);
								
								thumb.Save(strServerSavePath + "thumb_" + strReplacedFilename);
							}
							else
							{
								objFileInput.SaveAs(strServerSavePath + "thumb_" + strReplacedFilename);
							}
											
							datAddedTime = Convert.ToDateTime(File.GetCreationTime(strServerSavePath + strReplacedFilename));
								
							Wysiwyg wys = new Wysiwyg();
							Database db = new Database();

							string strSql = "INSERT INTO file_image (addedtime,name,description,path) VALUES(CURRENT_TIMESTAMP(),'"+wys.ToDb(2,StrName)+"','"+wys.ToDb(1,strDescription)+"','"+strReplacedFilename+"')";
							
							db.execSql(strSql);
                            db.dbDispose();
							db = null;
							wys = null;

							Session["noerror"] = "<div id='noerror'>Filen (" + strReplacedFilename + " - " + objFileInput.ContentLength / 1000 + " KB) er nu gemt.</div>";
						}
					}

					break;
				case "application": // PDF og doc-filer
				
					if (arrType[1] == "nothingmuch")
                    //msword" && arrType[1] != "pdf" && arrType[1] != "ms-powerpoint" && arrType[1] != "octet-stream"
					{
						ue.StrError = "Filen skal være af typen 'PDF', 'DOC' eller 'PPT'" + arrType[1];

						throw ue;
					} 
					else
					{				
						objFileInput.SaveAs(strServerSavePath + strReplacedFilename);
																		
						datAddedTime = Convert.ToDateTime(File.GetCreationTime(strServerSavePath + strReplacedFilename));
								
						Wysiwyg wys = new Wysiwyg();
						Database db = new Database();

						string strSql = "INSERT INTO file_files (languageid,addedtime,name,description,path) VALUES(" + IntLanguageId + ",CURRENT_TIMESTAMP(),'"+wys.ToDb(2,StrName)+"','"+wys.ToDb(1,strDescription)+"','"+strReplacedFilename+"')";
							
						db.execSql(strSql);
                        db.dbDispose();
						db = null;
						wys = null;

						Session["noerror"] = "<div id='noerror'>Filen (" + strReplacedFilename + " - " + objFileInput.ContentLength / 1000 + " KB) er nu gemt.</div>";
					}
					
					break;
				default: // Exception
					ue.StrError = "Du kan ikke uploade filer af typen '" + arrType[1] + "'.";

					throw ue;

			}

			return ue;

		}
	}
}
