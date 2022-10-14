namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;

	public partial class news : uc_pages
	{
		private int intId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			intId = Convert.ToInt32(Request.QueryString["id"]);

			string strMode = Request.QueryString["mode"];
			if(strMode == "details"){
				details();
			}else{
				listNews();
			}
		}

		public HtmlGenericControl getLastNews(int intLanguageId){
			HtmlGenericControl newsDiv = new HtmlGenericControl("div");

			newsDiv.Attributes["class"] = "newsDiv";

			HtmlImage newsImg = new HtmlImage();
			newsImg.Style.Add("margin-bottom","15px");
			newsImg.Src = "gfx/news_icon.gif";
			
			newsDiv.Controls.Add(newsImg);

			Database db = new Database();
			string strSql = "SELECT id,header,body FROM news WHERE languageid = " + intLanguageId + " ORDER BY sticky DESC,addedtime DESC LIMIT 0,2";
			MySqlDataReader objDr = db.select(strSql);

			while(objDr.Read()){
				newsDiv.Controls.Add(new LiteralControl("<div style='text-align:left;margin-bottom:10px;'>"));
				HtmlAnchor aLink = new HtmlAnchor();
				switch(intLanguageId){
					case 1:
						aLink.HRef = "?page=173&mode=details&id="+objDr["id"].ToString();
						break;
					case 2:
						aLink.HRef = "?page=174&mode=details&id="+objDr["id"].ToString();
						break;
					case 3:
						aLink.HRef = "?page=175&mode=details&id="+objDr["id"].ToString();
						break;
					case 4:
						aLink.HRef = "?page=1207&mode=details&id="+objDr["id"].ToString();
						break;
				}
				aLink.InnerHtml = objDr["header"].ToString();

				newsDiv.Controls.Add(aLink);

				int intMaxlenght = objDr["body"].ToString().Replace("<br/>","").Length;

				if(intMaxlenght > 200){
					intMaxlenght = 200;
				}
				
				newsDiv.Controls.Add(new LiteralControl("<br/>"+objDr["body"].ToString().Replace("<br/>","").Substring(0,intMaxlenght)));
				newsDiv.Controls.Add(new LiteralControl("</div>"));
			}

			if(objDr.HasRows){
				HtmlAnchor aAll = new HtmlAnchor();
				switch(intLanguageId){
					case 1:
						aAll.HRef = "?page=173";
						aAll.InnerHtml = "&raquo; Se alle nyheder";
						break;
					case 2:
						aAll.HRef = "?page=174";
						aAll.InnerHtml = "&raquo; Ljæs alle nyheter";
						break;
					case 3:
						aAll.HRef = "?page=175";
						aAll.InnerHtml = "&raquo; List all news";
						break;
					case 4:
						aAll.HRef = "?page=1207";
						aAll.InnerHtml = "&raquo; List all news";
						break;
				}
				newsDiv.Controls.Add(aAll);
				newsDiv.Controls.Add(new LiteralControl("<br/><br/>"));
			}

			db.objDataReader.Close();
			db = null;	

			return newsDiv;

		}

		private void details(){
			Database db = new Database();
			string strSql = "SELECT news.addedtime,header,body,CONCAT(firstname,' ',lastname) AS name FROM news INNER JOIN user_admin ON userid = author WHERE news.id = " + intId;
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				this.Controls.Add(new LiteralControl("<div class='page_subheader'>"+ objDr["header"].ToString() +"</div>"));
				this.Controls.Add(new LiteralControl(Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy HH:mm")));
				this.Controls.Add(new LiteralControl(" | " + objDr["name"].ToString()));
				this.Controls.Add(new LiteralControl("<p>"+ objDr["body"].ToString() +"</p>"));	

				HtmlAnchor aBack = new HtmlAnchor();
                switch (((tye.Menu)Session["menu"]).IntLanguageId)
                {
					case 1:
						aBack.HRef = "../../?page=173";
						aBack.InnerHtml = "&raquo; Tilbage til oversigten";
						break;
					case 2:
						aBack.HRef = "../../?page=174";
						aBack.InnerHtml = "&raquo; Tilbak til oversigten";
						break;
					case 3:
						aBack.HRef = "../../?page=175";
						aBack.InnerHtml = "&raquo; Back to the archive";
						break;
					case 4:
						aBack.HRef = "../../?page=1207";
						aBack.InnerHtml = "&raquo; Zurück";
						break;
				}
				this.Controls.Add(aBack);
			}

			db.objDataReader.Close();
			db = null;
		}

		private void listNews(){
			Database db = new Database();
            string strSql = "SELECT DATE_FORMAT(addedtime,'%d-%m-%Y %H:%i') AS dato ,header,id FROM news WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " ORDER BY sticky DESC,addedtime DESC;";
			
			DataGrid objDg = new DataGrid();
			objDg.ID = "data_table";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 1;
			objDg.Width = 480;
			objDg.CssClass = "data_table";
			objDg.GridLines = GridLines.None;

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "dato";

            switch (((tye.Menu)Session["menu"]).IntLanguageId)
            {
				case 1:
					objBc.HeaderText = "Dato";
					break;
				case 2:
					objBc.HeaderText = "Dato";
					break;
				case 3:
					objBc.HeaderText = "Date";
					break;
				case 4:
					objBc.HeaderText = "Datum";
					break;
			}

			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "header";

            switch (((tye.Menu)Session["menu"]).IntLanguageId)
            {
				case 1:
					objBc.HeaderText = "Overskrift";
					break;
				case 2:
					objBc.HeaderText = "Overskrift";
					break;
				case 3:
					objBc.HeaderText = "Header";
					break;
				case 4:
					objBc.HeaderText = "Überschrift";
					break;
			}

			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 280;
			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&mode=details&id={0}";

            switch (((tye.Menu)Session["menu"]).IntLanguageId)
            {
				case 1:
					objHlc.HeaderText = "Detaljer";
					objHlc.Text = "Læs mere";
					break;
				case 2:
					objHlc.HeaderText = "Detaljer";
					objHlc.Text = "Ljæs mere";
					break;
				case 3:
					objHlc.HeaderText = "Details";
					objHlc.Text = "More";
					break;				
				case 4:
					objHlc.HeaderText = "Details";
					objHlc.Text = "Weiter";
					break;
			}
			
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);
		
			objDg.DataBind();

			this.Controls.Add(objDg);
			
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
	}
}
