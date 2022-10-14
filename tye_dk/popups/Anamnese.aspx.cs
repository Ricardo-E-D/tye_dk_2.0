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
using tye.exceptions;

namespace tye.popups
{
	public partial class Anamnese : System.Web.UI.Page
	{


		protected void Page_Load(object sender, System.EventArgs e)
		{
			int IntSubmenuId = 0;
			string strChooseText = "";
			string strSubmitText = "";
			int	intQuestionaireId = 0;
			ListBox[] arrListBox = new ListBox[20];
			string[] arrInfos;
			int intClientId = Convert.ToInt32(Request.QueryString["clientid"]);
			int intIsFirst = Convert.ToInt32(Request.QueryString["isfirst"]);

			switch(((Menu)Session["menu"]).IntLanguageId)
			{
				case 1:
					IntSubmenuId = 104;
					break;
				case 2:
					IntSubmenuId = 108;
					break;
				case 3:
					IntSubmenuId = 112;
					break;
				case 4:
					IntSubmenuId = 1179;
					break;
			}

			for(int i = 0;i < 20;i++)
			{
				arrListBox[i] = new ListBox();
				arrListBox[i].Enabled = false;
			}

			Database db = new Database();

			string strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;	


			db = new Database();

			strSql = "SELECT id,title,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 1;";	
			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
                db.objDataReader.Close(); 
                db.dbDispose();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read())
			{
				mainform.Controls.Add(new LiteralControl("<div class='page_subheader' style='margin-bottom:10px;'>" + objDr["title"].ToString() + "</div>"));

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
                db.dbDispose();
				db = null;
			}

			if(db != null)
			{
				db.objDataReader.Close();
                db.dbDispose();
				db = null;
			}

			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
			
			db = new Database();

			objDr = db.select(strSql);
            	
			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "data_table";

			int intPriority = 1;

			while(objDr.Read())
			{
				HtmlTableRow objHtr = new HtmlTableRow();

				HtmlTableCell objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:15px;font-weight:bold;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = intPriority.ToString() + ".";

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:360px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["question"].ToString();

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:100px;text-align:right;";
				objHtc.Attributes["class"] = "data_table_item";

				//RequiredFieldValidator objRfv = new RequiredFieldValidator();
				//objRfv.ErrorMessage = "x&nbsp;";
				//objRfv.ControlToValidate = "listbox_" + objDr["priority"].ToString();
				//objRfv.Display = ValidatorDisplay.Dynamic;
				//objHtc.Controls.Add(objRfv);

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[intPriority-1].Style.Add("width","80px");
				arrListBox[intPriority-1].ID = "listbox_" + objDr["priority"].ToString();

				arrListBox[intPriority-1].DataSource = db_list.select(strSql_list);

				arrListBox[intPriority-1].DataValueField = "_value";
				arrListBox[intPriority-1].DataTextField = "_option";

				arrListBox[intPriority-1].DataBind();

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[intPriority-1].Items.Insert(0,objLi);

				arrListBox[intPriority-1].Rows = 1;

				objHtc.Controls.Add(arrListBox[intPriority-1]);

				db_list.objDataReader.Close();
				db_list = null;

				objHtr.Controls.Add(objHtc);
		
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			mainform.Controls.Add(objHt);
/*
			mainform.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlTable objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.ID = "buttons";
			objHt1.Border = 0;
			objHt1.Style.Add("width","475px");
			objHt1.Style.Add("height","20px");

			HtmlTableRow objHtr1 = new HtmlTableRow();

			HtmlTableCell objHtc1 = new HtmlTableCell();

			objHtc1.Style.Add("width","50px");
			objHtc1.Style.Add("height","20px");
			
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");

			back.InnerHtml = "<<";

			back.HRef = "../../?page=" + IntPageId + "&mode=" + strMode + "&id=" + intId + "&step=1";

			if(intIsFirst == 1)
			{
				objHtc1.Controls.Add(back);
			}

			objHtr1.Controls.Add(objHtc1);

			HtmlTableCell objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","375px");
			objHtc2.Style.Add("text-align","center");
			
			
			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = arrInfos[13].ToString();
			submit.Click += new EventHandler(saveStep2);

			objHtc2.Controls.Add(submit);

			

			objHtr1.Controls.Add(objHtc2);

			HtmlTableCell objHtc3 = new HtmlTableCell();

			objHtc3.Style.Add("width","50px");
			objHtc3.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");

			forward.InnerHtml = ">>";

			if(intStepSaved < 2)
			{
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=3";

			objHtc3.Controls.Add(forward);

			objHtr1.Controls.Add(objHtc3);

			objHt1.Controls.Add(objHtr1);

			mainform.Controls.Add(objHt1);
*/
			db = new Database();

			strSql = "SELECT a_1,a_2,a_3,a_4,a_5,a_6,a_7,a_8,a_9,a_10,a_11,a_12,a_13,a_14,a_15,a_16,a_17,a_18,a_19,a_20 FROM a_anamnese WHERE clientid = "+intClientId+" AND isfirst = "+intIsFirst;
			objDr = db.select(strSql);

			if(objDr.Read())
			{
				for(int i = 0;i < 20;i++)
				{
					string selectedvalue = "0";
					try{
						   selectedvalue = objDr["a_" + (i+1)].ToString();
					   }
					catch (Exception ex) {
						selectedvalue = "0";
					}

					arrListBox[i].SelectedValue = selectedvalue;

//					arrListBox[i].SelectedValue = objDr["a_" + (i+1)].ToString();
				}
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
