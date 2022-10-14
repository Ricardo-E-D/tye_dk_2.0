namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Configuration;
	using System.Collections;
	using System.Web;
	using System.Web.Security;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.WebControls.WebParts;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;

	public partial class Admin_Exercises : uc_pages
	{
		private int intPageId = 0;
		private int testID = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			intPageId = Convert.ToInt32(Request.QueryString["page"]);
			
			if ((Request.QueryString["editid"] + "x" == "x") && IsPostBack == false) {
				MultiView1.ActiveViewIndex = 0;
				Database db = new Database();
				MySqlDataReader RS = db.select("SELECT id, name, priority FROM tests WHERE languageid = 1 ORDER BY name");
				while (RS.Read())
				{
					TableRow tr = new TableRow();
					TableCell td = new TableCell();
					string[] flags = { "danish_flag.gif", "norwegian_flag.gif", "english_flag.gif", "german_flag.gif" };
					for (int i = 0; i < 4; i++) {
						if(Shared.UserIsDist() && (Shared.UserLang() - 1) != i) {
							continue;
						}
						td.Text += "<a href=\"?page=" + intPageId + "&editid=" + RS["priority"].ToString() + "&lang=" + (i + 1) + "\">";
						td.Text += "<img src=\"gfx/" + flags[i] + "\" border=\"0\">";
						td.Text += "</a>&nbsp;";
					}
					tr.Controls.Add(td);
					td = new TableCell();
					td.Text = "<b>" + RS["name"].ToString() + "</b>";
					td.Width = 300;
					tr.Controls.Add(td);
					ex_table.Controls.Add(tr);
				}
				db.objDataReader.Close();
				db = null;
			}
			if (Request.QueryString["editid"] + "x" != "x")
			{
				MultiView1.ActiveViewIndex = 1;
				string strLang = "mital";
				string strNav = "../../?page=" + Request.QueryString["page"] + "&editid=" + Request.QueryString["editid"] + "&lang=" + Request.QueryString["lang"];
				Database db = new Database();
				MySqlDataReader RS = db.select("SELECT tests.id, tests.languageid, tests.name, tests.purpose, tests.intro, tests.important, tests_steps.priority, tests_steps.body, tests_steps.priority AS stepid FROM tests LEFT JOIN tests_steps ON tests.id = tests_steps.testid WHERE tests.priority = " + Request.QueryString["editid"] + " AND languageid = " + Request.QueryString["lang"] + " ORDER BY tests_steps.priority");
				switch (Convert.ToInt16(Request.QueryString["lang"]))
				{
					case 1:
						strLang = " - Dansk";
						break;
					case 2:
						strLang = " - Norsk";
						break;
					case 3:
						strLang = " - Engelsk";
						break;
					case 4:
						strLang = " - Tysk";
						break;
				}
				while (RS.Read())
				{
					TableCell td = new TableCell();
					HyperLink editlb = new HyperLink();

					string strSubID = "";
					string strStepID = RS["stepid"].ToString();
					if (Request.QueryString["editsubid"] != null) { strSubID = Request.QueryString["editsubid"].ToString(); }
					editlb.Text = RS["priority"].ToString();
					editlb.NavigateUrl = strNav + "&editsubid=" + strStepID;
					editlb.ID = "editLink" + RS["priority"].ToString();
					if (strSubID == strStepID) { editlb.Font.Bold = true; editlb.ForeColor = Color.Black; editlb.Font.Size = new FontUnit(11); }
					td.ID = "editNavTableCell" + RS["priority"].ToString();
					td.Controls.Add(editlb);
					editNavTableRowOne.Cells.Add(td);
				}
				if (RS.HasRows) { ex_editLabel.Text = "Rediger tekst til '" + RS["name"].ToString() + "'" + strLang; }
				string[] mandaLinks = { "Form&aring;l", "Intro", "Vigtigt" };
				string[] mandaFields = { "purpose", "intro", "important" };
				for (int a = 0; a < 3; a++)
				{
					string strSubID = "";
					if (Request.QueryString["editsubid"] != null) { strSubID = Request.QueryString["editsubid"].ToString(); }
					TableCell atd = new TableCell();
					HyperLink alb = new HyperLink();
					alb.Text = mandaLinks[a];
					alb.ID = "edit" + mandaLinks[a].ToString();
					alb.NavigateUrl = strNav + "&editsubid=" + mandaFields[a].ToString();
					if (strSubID == mandaFields[a].ToString()) { alb.Font.Bold = true; alb.ForeColor = Color.Black; alb.Font.Size = new FontUnit(11); }
					atd.Controls.Add(alb);
					atd.ID = "edittd" + mandaLinks[a].ToString();
					editNavTableRowOne.Cells.AddAt(a, atd);
				}
				db.objDataReader.Close();
				db = null;

				if (Request.QueryString["editsubid"] + "x" != "x")
				{
					int intEditSub = 0; string strEditSub = "";
					try
					{
						intEditSub = int.Parse(Request.QueryString["editsubid"].ToString());
					}
					catch
					{
						strEditSub = Request.QueryString["editsubid"].ToString();
					}

					db = new Database();
					string strSQL = "SELECT * FROM tests LEFT JOIN tests_steps ON tests.id = tests_steps.testid WHERE tests.priority = " + Request.QueryString["editid"] + " AND languageid =" + Request.QueryString["lang"];
					if (intEditSub != 0)
					{
						strSQL += " AND tests_steps.priority = " + intEditSub;
					}
					strSQL += " ORDER BY tests_steps.priority";

					RS = db.select(strSQL);
					if (RS.Read())
					{
						testID = Convert.ToInt32(RS["testID"].ToString());
						FredCK.FCKeditorV2.FCKeditor nF = new FredCK.FCKeditorV2.FCKeditor();
						nF = new FredCK.FCKeditorV2.FCKeditor();
						nF.ID = "editor_fck"; nF.BasePath = "FCKeditor/"; nF.BaseHref = ""; nF.Width = 400; nF.Height = 400;
						nF.SkinPath = "skins/silver/"; nF.ToolbarSet = "small";
						if (intEditSub != 0)
						{
							nF.Value = RS["body"].ToString();
						}
						else
						{
							nF.Value = RS[strEditSub].ToString();
						}
						importantCell.Controls.Add(nF);

						Button saveBtn = new Button();
						saveBtn.Text = "Gem";
						saveBtn.ID = "saveBtn";
						saveBtn.Click += new EventHandler(saveThisText);
						TableRow savetr = new TableRow(); TableCell emptytd = new TableCell();
						TableCell savetd = new TableCell();
						savetd.ColumnSpan = 2;
						savetd.Controls.Add(saveBtn);
						savetr.Controls.Add(emptytd);
						savetr.Controls.Add(savetd);
						editTable.Controls.Add(savetr);
					}
					db.objDataReader.Close();
					db = null;
				}
			}
		}
		private void saveThisText(object Sender, EventArgs E)
		{
			foreach (Control obj in this.Controls)
			{
				if(obj.ID != null)
					Response.Write(obj.ID.ToString() + "<br>");
			}
			FredCK.FCKeditorV2.FCKeditor fckeditor = (FredCK.FCKeditorV2.FCKeditor)this.FindControl("editor_fck");
			if (fckeditor == null) { return; }

			string strToSave = Request.QueryString["editsubid"].ToString();
			string strToSaveTable = "";
			int lang = Convert.ToInt16(Request.QueryString["lang"].ToString());
			int editid = Convert.ToInt16(Request.QueryString["editid"].ToString());
			string strSQL = "";

			if (strToSave == "important" || strToSave == "intro" || strToSave == "purpose")
			{
				strToSaveTable = "tests";
			}
			else
			{
				strToSaveTable = "tests_steps";
			}

			strSQL = "UPDATE " + strToSaveTable + " SET ";
			if (strToSaveTable == "tests")
			{
				strSQL += strToSave + " = '" + fckeditor.Value.Replace("'", "''") + "' WHERE priority = " + editid + " AND languageid = " + lang;
			}
			else
			{
				strSQL += "body = '" + fckeditor.Value.Replace("'", "''") + "' WHERE priority = " + strToSave + " AND testid = " + testID;
			}
			Database db = new Database();
			db.execSql(strSQL);
			db.Dispose();
			db = null;
			string redir = "?page=1208&editid=" + Request.QueryString["editid"].ToString() + "&lang=" + lang;
			Response.Redirect(redir);
		}
	}
}