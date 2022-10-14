using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using tye.exceptions;
using System.Globalization;

namespace tye
{
	public partial class _Default : System.Web.UI.Page
	{
		protected TextBox login;
		protected Button login_btn;
		protected TextBox login_tb;
		protected TextBox h4xbox;
		protected PlaceHolder error_ph;
		protected HtmlGenericControl page_header = new HtmlGenericControl("div");

		private int intPageId;
		private int intSubmenuId;
        private int testID;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["menu"] == null)
			{
				Menu objMenu = new Menu();
				objMenu.IntLanguageId = 1;
				objMenu.StrAccess = "access_surfer";
				Session["menu"] = objMenu;
				objMenu = null;
			}
			try
			{
				if (!(Request.Browser.JavaScript == true)) {
					throw new NoJs();
				}
		
				if (Session["login"] == null)
				{
					Login objLogin = new Login();

					objLogin.BlnSucces = false;
					objLogin.IntUserType = 1;
					objLogin.StrIp = Request.UserHostAddress.ToString();

					Session["login"] = objLogin;
				
					objLogin = null;
				}

				if (((Login)Session["login"]).BlnSucces == true)
				{			
					Button logout_btn = new Button();

					logout_btn.ID = "logout_btn";
					logout_btn.Text = "LOGOUT";
					logout_btn.Click += new EventHandler(doLogOut);
					logout_btn.CausesValidation = false;

					login_div.Controls.Add(logout_btn);		
				}
				else
				{
					main_body.Attributes["onload"] = "main_form.login_tb.focus();";

					Button login_btn = new Button();

					login_btn.ID = "login_btn";
					login_btn.Text = "LOGIN";
					login_btn.Click += new EventHandler(doLogin);

					login_div.Controls.Add(login_btn);

					Label login_arrows = new Label();

					login_arrows.ID = "login_arrows";
					login_arrows.Text = "» ";

					login_div.Controls.Add(login_arrows);
					//lefttop_div.Controls.Add(login_arrows);

					TextBox login_tb = new TextBox();

					login_tb.ID = "login_tb";
					login_tb.MaxLength = 10;
					login_tb.TextMode = TextBoxMode.Password;

					lefttop_div.Controls.Add(login_tb);

					TextBox h4xbox = new TextBox();

					h4xbox.ID = "h4xbox";
					h4xbox.Enabled = false;
									
					login_div.Controls.Add(h4xbox);
					throw new NoAccess();
				}
				Login tmpLogin = (Login)Session["login"];
				if (tmpLogin.IntUserType != 4)
					throw new NoAccess();
				
				intSubmenuId = Convert.ToInt32(Request.QueryString["submenu"]);
				intPageId = Convert.ToInt32(Request.QueryString["page"]);
				
				intPageId = ((Menu)Session["menu"]).getFirstId(intPageId);
				
				if (!(intSubmenuId > 0))
				{
					intSubmenuId = ((Menu)Session["menu"]).getFirstSubmenuId(intPageId);	
				}

                // tilføj flag til sprogvalg til siden (hvis man ikke er logget ind)
				if(((Menu)Session["menu"]).StrAccess == "access_surfer") {
					DrawFlags();
				}

				((Menu)Session["menu"]).FillMenu(intPageId,menu_td);
				((Menu)Session["menu"]).FillSubmenu(intPageId,intSubmenuId,submenu_td);

				bool isFound = false;

                if (intPageId == 1) {
                    isFound = true;
                }
                else
                {
                    for (int i = 0; i < ((Menu)Session["menu"]).ArrPageAccess.Length; i++)
                    {
                        int id = ((Menu)Session["menu"]).ArrPageAccess[i];
                        if (intPageId == ((Menu)Session["menu"]).ArrPageAccess[i])
                        {
                            isFound = true;
                            break;
                        }
                    }
                }
				if (isFound == false) {
					throw new NoAccess();
				}
				
				//Tilføjer nyhedstd
				news_td.Controls.Add(new tye.uc.pages.news().getLastNews(((Menu)Session["menu"]).IntLanguageId));

				//Tilføjer indhold til siden
				Database db = new Database();

			}

			catch(FileNotFound fnf)
			{
				Label error_lbl = new Label();

				error_lbl.Text = fnf.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";
				
				content_div.Controls.Clear();
				content_div.Controls.Add(error_lbl);
			}

			catch(NoAccess na)
			{
				Label error_lbl = new Label();

				error_lbl.Text = na.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";
				
				content_div.Controls.Clear();
				content_div.Controls.Add(error_lbl);
			}
            goEvalContents();
		}

        private void goEvalContents()
        {
            if ((Request.QueryString["editid"] + "x" == "x") && IsPostBack == false)
            {
                MultiView1.ActiveViewIndex = 0;
                Database db = new Database();
                MySqlDataReader RS = db.select("SELECT id, name, priority FROM tests WHERE languageid = 1 ORDER BY name");
                while (RS.Read())
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    string[] flags = {"danish_flag.gif", "norwegian_flag.gif", "english_flag.gif", "german_flag.gif"};
                    for (int i = 0; i < 4; i++)
                    {
                        ImageButton lb = new ImageButton();
                        lb.ImageUrl = "gfx/" + flags[i];
                        lb.PostBackUrl = "?page=" + intPageId + "&editid=" + RS["priority"].ToString() + "&lang=" + (i + 1);
                        lb.Width = 25;
                        TableCell itd = new TableCell();
                        itd.Controls.Add(lb); tr.Controls.Add(itd);
                    }
                    td.Text = RS["name"].ToString();
                    td.Width = 300;
                    tr.Controls.Add(td);
                    ex_table.Controls.Add(tr);
                }
                db.objDataReader.Close();
                db = null;
            }
            if(Request.QueryString["editid"] + "x" != "x")
            {
                MultiView1.ActiveViewIndex = 1;
                string strLang = "mital";
                string strNav = "?page=" + Request.QueryString["page"] + "&editid=" + Request.QueryString["editid"] + "&lang=" + Request.QueryString["lang"];
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
                    alb.NavigateUrl  = strNav + "&editsubid=" + mandaFields[a].ToString();
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
                    if(intEditSub!=0)
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
            FredCK.FCKeditorV2.FCKeditor fckeditor = (FredCK.FCKeditorV2.FCKeditor)Page.FindControl("editor_fck");
            if (fckeditor == null) { return; }

            string strToSave = Request.QueryString["editsubid"].ToString();
            string strToSaveTable = "";
            int lang = Convert.ToInt16(Request.QueryString["lang"].ToString());
            int editid = Convert.ToInt16(Request.QueryString["editid"].ToString());
            string strSQL = "";

            if (strToSave == "important" || strToSave == "intro" || strToSave == "purpose") {
                strToSaveTable = "tests";
            }
            else {
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
            string redir = "Admin_Exercises.aspx?page=1208&editid=" + Request.QueryString["editid"].ToString() + "&lang=" + lang;
            Response.Redirect(redir);
        }

		private void DrawFlags()
		{
			Database db = new Database();
			string strSql = "SELECT id,name,imgpath,imgalt FROM language WHERE isactive = 1 ORDER BY id;";
			MySqlDataReader objDr = db.select(strSql);
			while (objDr.Read() == true)
			{
			}
			db.objDataReader.Close();
			db = null;
		}

        public void doLogin(object Sender, System.EventArgs E)
        {
            ((Login)Session["login"]).StrPassword = Request.Form["login_tb"].Replace("'", "");
            ((Login)Session["login"]).StrPassword = ((Login)Session["login"]).StrPassword.Trim();

            LogLogin objLl = new LogLogin();
            objLl.StrIp = Request.UserHostAddress;
            objLl.StrPassword = ((Login)Session["login"]).StrPassword;

            try
            {
                ((Login)Session["login"]).ValidateLogin();
                objLl.Insert(objLl);

                Response.Redirect(Request.RawUrl);
            }
            catch (WrongPassword wp)
            {
                /*
                 * show an error message saying that the account is locked
                 */

                Label error_lbl = new Label();

                error_lbl.Text = wp.Message(((Menu)Session["menu"]).IntLanguageId);
                error_lbl.ID = "error_lbl";

                content_div.Controls.Clear();
                content_div.Controls.Add(error_lbl);

                objLl.Insert(objLl);
            }
            catch (LicenceExpired le)
            {
                /*
                 * show an error message saying that the account is locked
                 */

                Label error_lbl = new Label();

                error_lbl.Text = le.Message(((Menu)Session["menu"]).IntLanguageId);
                error_lbl.ID = "error_lbl";

                content_div.Controls.Clear();
                content_div.Controls.Add(error_lbl);

                objLl.Insert(objLl);
                Session.Abandon();
            }

            objLl = null;
        }

        public void doLogOut(object Sender, System.EventArgs E)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx");
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
