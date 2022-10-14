using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

namespace tye.popups
{
	public partial class Preview : System.Web.UI.Page
	{
		protected HtmlGenericControl preview_body;
		protected Literal js = new Literal();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				
				js.Text = "<script type='text/javascript'>\n";
				js.Text += "function Decode(){\n";
				js.Text += "var strTempText = opener.document.main_form._"+Files.strCtl+"_content.value;\n";
					js.Text += @"strTempText = strTempText.replace(/\[liste:tal\]/g,'<ol>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/liste:tal\]/g,'</ol>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[liste:prik\]/g,'<ul>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/liste:prik\]/g,'</ul>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[punkt\]/g,'<li>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/punkt\]/g,'</li>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\r\n|\r/g,'<br/>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[fed\]/g,'<span class=\'bold_text\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/fed\]/g,'</span>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\kursiv\]/g,'<span class=\'italic_text\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/kursiv\]/g,'</span>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[overskrift\]/g,'<div class=\'page_subheader\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/overskrift\]/g,'</div>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[venstre stillet\]/g,'<div class=\'object_left\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/venstre stillet\]/g,'</div>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[højre stillet\]/g,'<div class=\'object_right\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/højre stillet\]/g,'</div>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[centreret\]/g,'<div class=\'object_center\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/centreret\]/g,'</div>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[link\,/g,'<a href=\'#\'');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\,intern\]/g,'\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\,ekstern\]/g,'\'>');";
					js.Text += "\n";
					js.Text += @"strTempText = strTempText.replace(/\[\/link\]/g,'</a>');";
					js.Text += "\n";
					js.Text += @"strTempText = ReplaceImg(strTempText);";
					js.Text += "\n";

					js.Text += "document.getElementById('content').innerHTML = strTempText;\n";	

					//js.Text += "document.write(opener.document.main_form._ctl0_content.value);\n";
					//js.Text += "document.write('<br/><br/>');\n";
					//js.Text += "document.write(strTempText);\n";


				js.Text += "}\n";
						
				js.Text += "function Refresh()\n";
				js.Text += "{\n";
					js.Text += "var sUrl = unescape(window.location.pathname);\n";
					js.Text += "window.location.href = sUrl;\n";
				js.Text += "}\n";

				js.Text += "function ReplaceImg(strTempText){\n";
					
					Database db = new Database();

					string strSql = "SELECT path,description,name FROM file_image;";

					MySqlDataReader objDr = db.select(strSql);

					while (objDr.Read())
					{
						js.Text += "strTempText = strTempText.replace('[billede: " + objDr["name"].ToString() + " /]','<img src=\"" + Files.strServerFilePath + objDr["path"].ToString() + "\" id=\"" + objDr["name"].ToString() + "\" alt=\"" + objDr["description"].ToString() + "\" style=\"border: 0px;\" />');\n";
					}

					db.objDataReader.Close();
                    db.dbDispose();
                    objDr.Close();
					db = null;

					js.Text += "return strTempText;\n";
				js.Text += "}\n";

				js.Text += "</script>\n";

				head_ph.Controls.Add(js);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
