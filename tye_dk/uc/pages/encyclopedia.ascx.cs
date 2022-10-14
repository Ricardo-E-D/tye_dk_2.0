namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;

	public partial class encyclopedia : uc_pages
	{
		protected string[] arrInfos;
		protected int intWordId;
		protected string strMode;
		protected ListBox word = new ListBox();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];
			intWordId = Convert.ToInt32(Request.QueryString["id"]);
			
			drawWordSelect();

			if(strMode == "details" && intWordId > 0)
			{
				drawWordDesc();

				word.SelectedValue = intWordId.ToString();
			}

		}

		private void drawWordDesc()
		{
			Database db = new Database();

			string strSql = "SELECT word,description FROM encyclopedia WHERE id = " + intWordId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				HtmlGenericControl word_div = new HtmlGenericControl("div");
				
				word_div.ID = "word_div";
				word_div.Attributes["class"] = "page_subheader";
				word_div.Style.Add("margin-top","20px;");
				word_div.Style.Add("margin-bottom","10px;");
				word_div.InnerHtml = objDr["word"].ToString();

				this.Controls.Add(word_div);

				HtmlGenericControl desc_div = new HtmlGenericControl("div");

				desc_div.ID = "desc_div";
				desc_div.InnerHtml = objDr["description"].ToString();

				this.Controls.Add(desc_div);
			}

			db.objDataReader.Close();
			db = null;
		}

		private void drawWordSelect()
		{
			Database db = new Database();

			string strSql = "SELECT body FROM content WHERE menuid = " + IntPageId;

			MySqlDataReader objDr = db.select(strSql);
			
			if(objDr.Read())
			{
				arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
			}

			db.objDataReader.Close();
			db = null;
		
			word.ID = "word";
			word.Rows = 1;
			word.Style.Add("width","475px");

			db = new Database();

            strSql = "SELECT id,word FROM encyclopedia WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " AND " + ((tye.Menu)Session["menu"]).StrAccess + " = 1 ORDER BY word;";

			word.DataSource = db.select(strSql);
			word.DataTextField = "word";
			word.DataValueField = "id";
			
			word.DataBind();

			ListItem objLi = new ListItem();
			objLi.Text = arrInfos[0].ToString();
			objLi.Value = "0";
			objLi.Selected = true;

			word.Items.Insert(0,objLi);

			word.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&mode=details&id='+this.value;";

			this.Controls.Add(word);

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
