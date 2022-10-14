namespace tye.uc.pages
{
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
	using tye.exceptions;
	using MySql.Data.MySqlClient;
	using System.IO;

	public partial class admin_banner_admin : uc_pages
	{
		private HtmlInputFile hiBanner = new HtmlInputFile();
		private TextBox tbWebsite = new TextBox();
		private TextBox tbAlt = new TextBox();
		private CheckBox[] arrCb = new CheckBox[10];
		private int intCbs;
		private const float fltImgMaxSize = (50 * 1024);
		private PlaceHolder phError = new PlaceHolder();
		private UploadError ue = new UploadError();
		private CheckBox cbActive = new CheckBox();
		private int intBannerId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strMode = Request.QueryString["mode"];
			intBannerId = Convert.ToInt32(Request.QueryString["id"]);

			try{
				switch(IntSubmenuId){
					case 184:
						switch(strMode){
							case "edit":
								updatePage();
								break;
							case "delete":
								delete();
								break;
							default:
								list();
								break;
						}
						break;
					default:
						insertPage();
						break;
				}
			}
			catch(UploadError ule){
				phError.Controls.Add(new LiteralControl(ule.Message(1)));
			}
		}

		private void insertPage(){

			if(Session["noerror"] != null){
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			this.Controls.Add(phError);

			Main_form.Enctype = "multipart/form-data";
			
			this.Controls.Add(new LiteralControl("Billede: * "));

			RequiredFieldValidator rfvFile = new RequiredFieldValidator();
			rfvFile.ControlToValidate = "file";
			rfvFile.Display = ValidatorDisplay.Dynamic;
			rfvFile.ErrorMessage = "Du skal vælge et banner du vil tilføje.";

			this.Controls.Add(rfvFile);

			this.Controls.Add(new LiteralControl("<br/>"));

			hiBanner.ID = "file";
			hiBanner.Style.Add("width","475px");

			this.Controls.Add(hiBanner);

			this.Controls.Add(new LiteralControl("<br/><br/>Link: * "));

			RequiredFieldValidator rfvLink = new RequiredFieldValidator();
			rfvLink.ControlToValidate = "link";
			rfvLink.Display = ValidatorDisplay.Dynamic;
			rfvLink.ErrorMessage = "Du skal skrive et link.";

			this.Controls.Add(rfvLink);

			this.Controls.Add(new LiteralControl("<br/>"));

			tbWebsite.ID = "link";
			tbWebsite.Style.Add("width","475px");
			tbWebsite.MaxLength = 255;

			this.Controls.Add(tbWebsite);

			this.Controls.Add(new LiteralControl("<br/><br/>Alt: * "));

			RequiredFieldValidator rfvAlt = new RequiredFieldValidator();
			rfvAlt.ControlToValidate = "alt";
			rfvAlt.Display = ValidatorDisplay.Dynamic;
			rfvAlt.ErrorMessage = "Du skal skrive en beskrivende tekst til banneret.";

			this.Controls.Add(rfvAlt);

			this.Controls.Add(new LiteralControl("<br/>"));

			tbAlt.ID = "alt";
			tbAlt.Style.Add("width","475px");
			tbAlt.MaxLength = 255;

			this.Controls.Add(tbAlt);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			Database db = new Database();
			string strSql = "SELECT COUNT(*) AS found FROM language";
			intCbs = Convert.ToInt32(db.scalar(strSql));

			for(int i = 0;i < intCbs;i++){
				arrCb[i] = new CheckBox();
			}

			strSql = "SELECT id,name FROM language ORDER BY name";
			MySqlDataReader objDr = db.select(strSql);

			int intCount = 0;

			while(objDr.Read()){
				arrCb[intCount].Text = " " + objDr["name"].ToString();
				this.Controls.Add(new LiteralControl("<br/>"));
				this.Controls.Add(arrCb[intCount]);
				this.Controls.Add(new LiteralControl("<br/>"));
			intCount++;	
			}

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			
			cbActive.Text = " Gør dette banner aktivt.";
			
			this.Controls.Add(cbActive);

            db.objDataReader.Close();
			db = null;

			Button submit = new Button();
			submit.Text = "Opret banner";
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Click +=new EventHandler(saveBanner);

			this.Controls.Add(submit);
		}
		
		private void updatePage(){
			if(Session["noerror"] != null){
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}
			this.Controls.Add(new LiteralControl("Link: * "));

			RequiredFieldValidator rfvLink = new RequiredFieldValidator();
			rfvLink.ControlToValidate = "link";
			rfvLink.Display = ValidatorDisplay.Dynamic;
			rfvLink.ErrorMessage = "Du skal skrive et link.";

			this.Controls.Add(rfvLink);

			this.Controls.Add(new LiteralControl("<br/>"));

			tbWebsite.ID = "link";
			tbWebsite.Style.Add("width","475px");
			tbWebsite.MaxLength = 255;

			this.Controls.Add(tbWebsite);

			this.Controls.Add(new LiteralControl("<br/><br/>Alt: * "));

			RequiredFieldValidator rfvAlt = new RequiredFieldValidator();
			rfvAlt.ControlToValidate = "alt";
			rfvAlt.Display = ValidatorDisplay.Dynamic;
			rfvAlt.ErrorMessage = "Du skal skrive en beskrivende tekst til banneret.";

			this.Controls.Add(rfvAlt);

			this.Controls.Add(new LiteralControl("<br/>"));

			tbAlt.ID = "alt";
			tbAlt.Style.Add("width","475px");
			tbAlt.MaxLength = 255;

			this.Controls.Add(tbAlt);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			Database db = new Database();
			string strSql = "SELECT COUNT(*) AS found FROM language";
			intCbs = Convert.ToInt32(db.scalar(strSql));

			for(int i = 0;i < intCbs;i++){
				arrCb[i] = new CheckBox();
			}

			strSql = "SELECT id,name FROM language ORDER BY name";
			MySqlDataReader objDr = db.select(strSql);

			int intCount = 0;

			while(objDr.Read()){
				arrCb[intCount].Text = " " + objDr["name"].ToString();
				this.Controls.Add(new LiteralControl("<br/>"));
				this.Controls.Add(arrCb[intCount]);
				this.Controls.Add(new LiteralControl("<br/>"));
			intCount++;	
			}

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			cbActive.Text = " Gør dette banner aktivt.";
			
			this.Controls.Add(cbActive);

            db.objDataReader.Close();
			db = null;

			Button submit = new Button();
			submit.Text = "Opdater banner";
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Click += new EventHandler(updateBanner);

			this.Controls.Add(submit);

			db = new Database();
			strSql = "SELECT id,website,alt,clicks,isactive FROM banner WHERE id = " +intBannerId;
			objDr = db.select(strSql);

			if(objDr.Read()){
			
				tbWebsite.Text = objDr["website"].ToString();
				tbAlt.Text = objDr["alt"].ToString();
				if(Convert.ToInt32(objDr["isactive"]) == 1){
					cbActive.Checked = true;
				}
				Database db1 = new Database();
				string strSql1 = "SELECT id FROM language ORDER BY name";
				MySqlDataReader objDr1 = db1.select(strSql1);
				
				intCount = 0;

				while(objDr1.Read()){
				
					Database db2 = new Database();
					string strSql2 = "SELECT COUNT(*) AS found FROM banner_language WHERE bannerid = " + Convert.ToInt32(objDr["id"]) + " AND languageid = " + Convert.ToInt32(objDr1["id"]);
					intCbs = Convert.ToInt32(db2.scalar(strSql2));

					if(intCbs > 0){
						arrCb[intCount].Checked = true;
					}
					intCount++;

					db2 = null;
				}
				db1.objDataReader.Close();
				db1 = null;
			
			}

			db.objDataReader.Close();
			db = null;
		}

		private void delete(){
		
		Database db = new Database();
		string strSql = "SELECT path FROM banner WHERE id = " + intBannerId;
		string strPath = db.scalar(strSql).ToString();
		
		if(File.Exists(Files.strServerSavePath + strPath)){
			File.Delete(Files.strServerSavePath + strPath);
		}										  

		db = new Database();
		strSql = "DELETE FROM banner_language WHERE bannerid = " + intBannerId + ";";
		strSql += "DELETE FROM banner WHERE id = " + intBannerId + ";";
		db.execSql(strSql);

		db = null;

		list();
		}

		public void list()
			//lister de bannere der er i db'en
		{
			Literal js = new Literal();
			
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function Delete(intid){\n";
			js.Text += "var confirmDelete = window.confirm('Er du sikker på du vil slette dette banner?');\n";
			js.Text += "if(confirmDelete){\n";
			js.Text += "location.href = '?page="+IntPageId+"&submenu="+IntSubmenuId+"&mode=delete&id='+intid;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);

			string strSql = "SELECT id,path,website,alt,clicks FROM banner ORDER BY id DESC;";
			Database db = new Database();
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)){
				this.Controls.Add(new LiteralControl("Ingen bannere fundet i databasen..."));
			}

			HtmlGenericControl bannerDiv = new HtmlGenericControl();

			while(objDr.Read()){
				bannerDiv = new HtmlGenericControl();
				bannerDiv.Style.Add("padding","10px");
				bannerDiv.Style.Add("text-align","center");
				bannerDiv.Style.Add("float","left");
				bannerDiv.InnerHtml = "<a href='" + objDr["website"] + "' target='_blank'><img src='" + Files.strServerFilePath + objDr["path"].ToString() + "' style='border:0px;' id='banner_" + objDr["id"].ToString() + "' alt='" + objDr["alt"].ToString() + "' title='" + objDr["alt"].ToString() + "' /></a>";
				bannerDiv.InnerHtml += "<br/><a href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id=" + objDr["id"].ToString() + "'>Rediger</a> | <a href='#' onclick='Delete(" + objDr["id"].ToString() + ");'>Slet</a>";
				this.Controls.Add(bannerDiv);
			}

			db.objDataReader.Close();
			db = null;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		private void saveBanner(object sender, EventArgs e)
		{
			if(Page.IsValid){
				try{
				HttpPostedFile objFileInput = hiBanner.PostedFile;

				//Kontrol af om filen eksisterer i forvejen.
				string strReplacedFilename = objFileInput.FileName.Substring(objFileInput.FileName.LastIndexOf("\\")+1).ToString().Replace(" ","_").Replace("æ","ae").Replace("ø","oe").Replace("å","aa").Replace("'","").Replace("''","").Replace("^","_");

				int i = 1;

				while(File.Exists(Files.strServerSavePath + strReplacedFilename))
				{
					if (strReplacedFilename.IndexOf("^") != -1)
					{
						strReplacedFilename = strReplacedFilename.Substring(strReplacedFilename.IndexOf("^")+1,strReplacedFilename.Length - strReplacedFilename.IndexOf("^")-1);
					}
					strReplacedFilename = i + "^" + strReplacedFilename;
					i++;
				}
				
				//Kontrol af filtype, størrelse, dimentioner
				string[] arrType = objFileInput.ContentType.Split(Convert.ToChar("/"));

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
					Bitmap image = new Bitmap(objFileInput.InputStream);

					if (image.Width > 100)
					{
						ue.StrError = "Billedet er for stort, det må max fylde 100px i bredden.";

						throw ue;
					}
					else
					{
						
						objFileInput.SaveAs(Files.strServerSavePath + strReplacedFilename);
						
						Wysiwyg wys = new Wysiwyg();
						Database db = new Database();

						string strSql = "INSERT INTO banner (path,website,alt,isactive) VALUES('";
						strSql += strReplacedFilename + "','";
						
						if(tbWebsite.Text.IndexOf("http://") == -1){
							strSql += "http://";
						}
						
						strSql += wys.ToDb(2,tbWebsite.Text) + "','";

						strSql += wys.ToDb(2,tbAlt.Text) + "',";

						if(cbActive.Checked){
							strSql += "1)";
						}else{
							strSql += "0)";
						}

						db.execSql(strSql);
						db = null;

						db = new Database();
						strSql = "SELECT id FROM banner ORDER BY id DESC LIMIT 0,1;";
						int intBannerId = Convert.ToInt32(db.scalar(strSql));
						db = null;

						if(cbActive.Checked){
							db = new Database();
							
							strSql = "SELECT id,name FROM language ORDER BY name";
							MySqlDataReader objDr = db.select(strSql);

							int intCount = 0;

							while(objDr.Read()){
								if(arrCb[intCount].Checked){
									Database db1 = new Database();
									string strSql1 = "INSERT INTO banner_language (bannerid,languageid) VALUES(" + intBannerId + "," + Convert.ToInt32(objDr["id"]) + ");";
									db1.execSql(strSql1);									
									
									db1 = null;
								}
								
							intCount++;	
							}
							db.objDataReader.Close();
							db = null;
						}

						Session["noerror"] = "<div id='noerror'>Banneret er nu oprettet.</div>";
					
						Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId);
					}
				}
				}
				catch(UploadError ule){
					phError.Controls.Add(new LiteralControl(ule.Message(1)+ "<br/><br/>"));
				}
			}
			
		}

		private void updateBanner(object sender, EventArgs e) {
			if(Page.IsValid){
				Wysiwyg wys = new Wysiwyg();
				Database db = new Database();

				string strSql = "UPDATE banner SET website = '";

				if(tbWebsite.Text.IndexOf("http://") == -1){
					strSql += "http://";
				}

				strSql += wys.ToDb(2,tbWebsite.Text) + "',alt = '" + wys.ToDb(2,tbAlt.Text) + "',isactive = ";

				if(cbActive.Checked){
					strSql += "1";
				}else{
					strSql += "0";
				}

				strSql += " WHERE id = " + intBannerId + ";";
				strSql += "DELETE FROM banner_language WHERE bannerid = " + intBannerId + ";";
				db.execSql(strSql);
				db = null;

				db = new Database();
				strSql = "SELECT id,name FROM language ORDER BY name";
				MySqlDataReader objDr = db.select(strSql);

				int intCount = 0;

				while(objDr.Read()){
					if(arrCb[intCount].Checked){
						Database db1 = new Database();
						string strSql1 = "INSERT INTO banner_language (bannerid,languageid) VALUES(" + intBannerId + "," + Convert.ToInt32(objDr["id"]) + ");";
						db1.execSql(strSql1);
						db1 = null;
					}
					
				intCount++;	
				}
				db.objDataReader.Close();
				db = null;

				Session["noerror"] = "<div id='noerror'>Banneret er nu opdateret.</div>";
			
				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id=" + intBannerId);

			}
		}
	}
}
