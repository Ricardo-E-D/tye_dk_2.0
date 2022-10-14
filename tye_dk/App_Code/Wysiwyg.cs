using System;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace tye
{
	/// <summary>
	/// Summary description for Wysiwyg.
	/// </summary>
	public class Wysiwyg : System.Web.UI.Page
	{
		private string strTempText;
		private string strLinkAddress;
		private string strEndTags;
		private string[] arrEndTags;  
		private string strFileIcon;

		public Wysiwyg()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public string Encode(string strText)
		{

			string[] arrLess = (string[])strText.Split(Convert.ToChar("<"));

			for (int n = 0;n < arrLess.Length;n++)
			{
		
				string[] arrMore = (string[])arrLess[n].Split(Convert.ToChar(">"));

				for (int i = 0;i < arrMore.Length;i++)
				{
					if (arrMore[i].StartsWith("img") == true)
					{
						string[] arrImage;
						arrImage = arrMore[i].Split(Convert.ToChar("'"));

						strTempText += "[billede: " + arrImage[3] + " /]";
					}
					else if (arrMore[i].StartsWith("a href") == true)
					{
						string[] arrLink;
	
						arrLink = arrMore[i].Split(Convert.ToChar("'"));

						strLinkAddress = arrLink[1];
							
						if (arrMore[i].IndexOf("http://") != -1)
						{
							strLinkAddress += ",ekstern";
						}
						else
						{
							strLinkAddress += ",intern";
						}
								
						strLinkAddress = "[link," + strLinkAddress + "]";
			
						if (strTempText.Length == 0)
						{
							strTempText = strLinkAddress;
						}
						else
						{
							strTempText += strLinkAddress;
						}
					}
					else if (arrMore[i].StartsWith("span") == true)
					{
						if (arrMore[i].IndexOf("bold_text") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[fed]";
							}
							else
							{
								strTempText += "[fed]";
							}
														
							strEndTags += ",[/fed]";
							
						}
						else if (arrMore[i].IndexOf("italic_text") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[kursiv]";
							}
							else
							{
								strTempText += "[kursiv]";
							}
								
							strEndTags += ",[/kursiv]";	
						}
						else if (arrMore[i].IndexOf("file_link") != -1)
						{
							string[] arrFile;
							arrFile = arrMore[i].ToString().Split(Convert.ToChar("'"));

							if (strTempText.Length == 0)
							{
								strTempText = "[fil: " + arrFile[1] + " /]";
							}
							else
							{
								strTempText += "[fil: " + arrFile[1] + " /]";
							}
	
							arrLess[n+1] = "";
							arrLess[n+2] = "";
							arrLess[n+3] = "";
							arrLess[n+4] = arrLess[n+4].ToString().Replace("/span","");
							
						}
					}
					else if(arrMore[i].StartsWith("ul") == true)
					{
						if (strTempText.Length == 0)
						{
							strTempText = "[liste:prik]";
						}
						else
						{
							strTempText += "[liste:prik]";
						}
								
						strEndTags += ",[/liste:prik]";	
					}
					else if(arrMore[i].StartsWith("/ul") == true)
					{
						arrEndTags = strEndTags.Split(Convert.ToChar(","));

						strTempText += arrEndTags[arrEndTags.GetUpperBound(0)];

						strEndTags = "";

						for (int x = 1;x < arrEndTags.GetUpperBound(0);x++)
						{
							strEndTags += "," + arrEndTags[x].ToString();
						}
					}
					else if(arrMore[i].StartsWith("ol") == true)
					{
						if (strTempText.Length == 0)
						{
							strTempText = "[liste:tal]";
						}
						else
						{
							strTempText += "[liste:tal]";
						}
								
						strEndTags += ",[/liste:tal]";	
					}
					else if(arrMore[i].StartsWith("/ol") == true)
					{
						arrEndTags = strEndTags.Split(Convert.ToChar(","));

						strTempText += arrEndTags[arrEndTags.GetUpperBound(0)];

						strEndTags = "";

						for (int x = 1;x < arrEndTags.GetUpperBound(0);x++)
						{
							strEndTags += "," + arrEndTags[x].ToString();
						}
					}
					else if(arrMore[i].StartsWith("li") == true)
					{
						if (strTempText.Length == 0)
						{
							strTempText = "[punkt]";
						}
						else
						{
							strTempText += "[punkt]";
						}
								
						strEndTags += ",[/punkt]";	
					}
					else if(arrMore[i].StartsWith("/li") == true)
					{
						arrEndTags = strEndTags.Split(Convert.ToChar(","));

						strTempText += arrEndTags[arrEndTags.GetUpperBound(0)];

						strEndTags = "";

						for (int x = 1;x < arrEndTags.GetUpperBound(0);x++)
						{
							strEndTags += "," + arrEndTags[x].ToString();
						}
					}
					else if (arrMore[i].StartsWith("/span") == true)
					{
						try {
							arrEndTags = strEndTags.Split(Convert.ToChar(","));

							strTempText += arrEndTags[arrEndTags.GetUpperBound(0)];

							strEndTags = "";

							for (int x = 1;x < arrEndTags.GetUpperBound(0);x++)
							{
								strEndTags += "," + arrEndTags[x].ToString();
							}
						}
						catch {
							
						}
					
					}
					else if (arrMore[i].StartsWith("div") == true)
					{
						if (arrMore[i].IndexOf("page_subheader") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[overskrift]";
							}
							else
							{
								strTempText += "[overskrift]";
							}
							
							strEndTags += ",[/overskrift]";
						}
						else if (arrMore[i].IndexOf("object_left") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[venstre stillet]";
							}
							else
							{
								strTempText += "[venstre stillet]";
							}

							strEndTags += ",[/venstre stillet]";
						}
						else if (arrMore[i].IndexOf("object_right") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[højre stillet]";
							}
							else
							{
								strTempText += "[højre stillet]";
							}

							strEndTags += ",[/højre stillet]";
						}
						else if (arrMore[i].IndexOf("object_center") != -1)
						{
							if (strTempText.Length == 0)
							{
								strTempText = "[centreret]";
							}
							else
							{
								strTempText += "[centreret]";
							}
							strEndTags += ",[/centreret]";
						}
					}							
					else if (arrMore[i].StartsWith("/div") == true)
					{
						arrEndTags = strEndTags.Split(Convert.ToChar(","));

						strTempText += arrEndTags[arrEndTags.GetUpperBound(0)];

						strEndTags = "";

						for (int x = 1;x < arrEndTags.GetUpperBound(0);x++)
						{
							strEndTags += "," + arrEndTags[x].ToString();
						}
					}					
					else if (arrMore[i].StartsWith("/a") == true)
					{
						strTempText += "[/link]";
					}
					else
					{
						strTempText += arrMore[i];
					}
				}
			}
			return strTempText;
		}

		public string Decode(string strText)
		{
			strTempText = strText.Replace("[fed]","<span class='bold_text'>").Replace("[/fed]","</span>");
			strTempText = strTempText.Replace("[kursiv]","<span class='italic_text'>").Replace("[/kursiv]","</span>");
			strTempText = strTempText.Replace("[overskrift]","<div class='page_subheader'>").Replace("[/overskrift]","</div>");
			strTempText = strTempText.Replace("[venstre stillet]","<div class='object_left'>").Replace("[/venstre stillet]","</div>");
			strTempText = strTempText.Replace("[højre stillet]","<div class='object_right'>").Replace("[/højre stillet]","</div>");
			strTempText = strTempText.Replace("[centreret]","<div class='object_center'>").Replace("[/centreret]","</div>");
			strTempText = strTempText.Replace("[liste:tal]","<ol>").Replace("[/liste:tal]","</ol>");
			strTempText = strTempText.Replace("[liste:prik]","<ul>").Replace("[/liste:prik]","</ul>");
			strTempText = strTempText.Replace("[punkt]","<li>").Replace("[/punkt]","</li>");
			strTempText = strTempText.Replace("[link,","<a href='").Replace(",intern]","'>").Replace(",ekstern]","' target='_blank'>").Replace("[/link]","</a>");
			strTempText = DecodeImg(strTempText);
			strTempText = DecodeFiles(strTempText);
			return strTempText;
		}
		
		private string DecodeImg(string strTempText)
		{
			Database db = new Database();

			string strSql = "SELECT id,name,path,description FROM file_image";

			MySqlDataReader objDr = db.select(strSql);

			while(objDr.Read())
			{
				strTempText = strTempText.Replace("[billede: " + objDr["name"] + " /]","<img src='" + Files.strServerFilePath + objDr["path"] + "' id='"+objDr["name"]+"' alt='"+objDr["description"]+"' style='border: 0px;' />");
			}
			//rettet
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strTempText;
		}

		private string DecodeFiles(string strTempText)
		{
			Database db = new Database();

			string strSql = "SELECT id,name,path,description FROM file_files";

			MySqlDataReader objDr = db.select(strSql);

			while(objDr.Read())
			{
				strFileIcon = "<img src='gfx/";

				if(objDr["path"].ToString().EndsWith("doc"))
				{
					strFileIcon += "doc";
				}
				else if(objDr["path"].ToString().EndsWith("pdf"))
				{
					strFileIcon += "pdf";
				}
				else if(objDr["path"].ToString().EndsWith("ppt"))
				{
					strFileIcon += "ppt";
				}
				else {
					strFileIcon += "x";
				}
				string strFilename = objDr["name"].ToString();
            strFilename = strFilename.Replace("&oslash;", "ø").Replace("&aring;", "å").Replace("&aelig;", "æ");
            strFilename = strFilename.Replace("&Oslash;", "Ø").Replace("&Aring;", "Å").Replace("&Aelig;", "Æ");
				//objDr["name"]
				strFileIcon += "_icon.gif' alt='" + objDr["description"].ToString() + "' id='" + objDr["name"].ToString() + "' style='border:0px;vertical-align:middle;padding:3px;' />";

				strTempText = strTempText.Replace("[fil: " + strFilename + " /]","<span id='" + objDr["name"] + "' class='file_link'>" + strFileIcon + " <a href='" + Files.strServerFilePath + objDr["path"].ToString() + "' target='_blank'>" + objDr["name"].ToString() + "</a></span>");

			}
			//rettet
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strTempText;
		}

		public string ToDb(int intMode,string strText)
		{
			if (intMode == 1)
			{
				strText = strText.Replace("'","''").Replace("`","''").Replace("´","''").Replace("\n","<br/>").Replace("”","''").Replace("“","''").Replace("’","''");
			}
			else if (intMode == 2)
			{
				strText = strText.Replace("'","''").Replace("`","''").Replace("´","''").Replace("”","''").Replace("“","''").Replace("’","''");
			}

			return strText;
		}

		public string FromDb(int intMode,string strText)
		{
			if (intMode == 1)
			{
				strText = strText.Replace("<br/>","\n");
			}
			
			return strText;
		}			

		public bool IsNumeric(string str) 
		{
			Regex r = new Regex(@"^[\+\-]?\d*\.?[Ee]?[\+\-]?\d*$", RegexOptions.Compiled); 
			str = str.Trim();

			Match m = r.Match(str);

			return (m.Success); 
		}

		public double GetNumeric(double dblVal)
		{
			if(dblVal < 0.00)
			{
				dblVal = dblVal * -1.00;
			}

			return dblVal;
		}
	}
}
