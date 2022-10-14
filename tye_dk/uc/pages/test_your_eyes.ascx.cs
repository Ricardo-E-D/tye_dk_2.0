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
	using tye.exceptions;

	public partial class test_your_eyes : uc_pages
	{
		protected string strChooseText;
		protected string strSubmitText;
		protected int intQuestionaireId;
		protected ListBox[] arrListBox = new ListBox[13];
		protected int intPriority = 1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			for(int i = 0;i < 13;i++)
			{
				arrListBox[i] = new ListBox();
			}

			if(Questionaire.intStep == 0)
			{
				Questionaire.intStep = 1;
				
				Session["questionaire"] = new Questionaire();

			}

			if(Session["questionaire"] == null)
			{
				Questionaire.intStep = 0;

				Response.Redirect("?page=" + IntPageId);
			}

			try
			{
				switch(Questionaire.intStep)
				{
					case 1:
						drawStep1();
						break;
					case 2:
						drawStep2();
						break;
					case 3:
						drawStep3();
						break;
					case 4:
						drawStep4();
						break;
				}
			}
			catch(NoDataFound ndf)
			{
				ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId);
			}
		}

		private void drawStep1()
		{
			Database db = new Database();

			string strSql = "SELECT id,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntPageId + " AND step = 1;";
            			
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read())
			{
				if(!(Page.IsPostBack))
				{
					Page_header.InnerHtml += " - 1/4";
				}
				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-bottom:10px;'>" + objDr["header"].ToString() + "</div>"));

				this.Controls.Add(new LiteralControl("<p>" + objDr["instruction"].ToString() + "</p>"));

				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-top:10px;margin-bottom:5px;'>" + objDr["subheader"].ToString() + "</div>"));

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
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

				RequiredFieldValidator objRfv = new RequiredFieldValidator();
				objRfv.ErrorMessage = "x&nbsp;";
				objRfv.ControlToValidate = "listbox_" + objDr["priority"].ToString();
				objRfv.Display = ValidatorDisplay.Dynamic;

				objHtc.Controls.Add(objRfv);

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]) + " ORDER BY priority, _value;";

				arrListBox[Convert.ToInt32(intPriority) - 1].Style.Add("width","80px");
				arrListBox[Convert.ToInt32(intPriority) - 1].ID = "listbox_" + objDr["priority"].ToString();

				arrListBox[Convert.ToInt32(intPriority) - 1].DataSource = db_list.select(strSql_list);

				arrListBox[Convert.ToInt32(intPriority) - 1].DataValueField = "_value";
				arrListBox[Convert.ToInt32(intPriority) - 1].DataTextField = "_option";

				arrListBox[Convert.ToInt32(intPriority) - 1].DataBind();

				ListItem objLi = new ListItem();

				objLi.Value = "";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[Convert.ToInt32(intPriority) - 1].Items.Insert(0,objLi);

				arrListBox[Convert.ToInt32(intPriority) - 1].Rows = 1;

				objHtc.Controls.Add(arrListBox[Convert.ToInt32(intPriority) - 1]);

				db_list.objDataReader.Close();
				db_list = null;

				objHtr.Controls.Add(objHtc);
		
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = strSubmitText + " (2/4)";
			submit.Click += new EventHandler(toStep2);
			submit.Attributes["style"] = "margin-top:20px;width:475px;";

			this.Controls.Add(submit);
			
		}
		private void drawStep2()
		{
			for(int i = 0;i < 2;i++)
			{
				arrListBox[i] = new ListBox();
			}

			Database db = new Database();

			string strSql = "SELECT id,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntPageId + " AND step = 2;";
            			
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read())
			{
				if(!(Page.IsPostBack))
				{
					Page_header.InnerHtml += " - 2/4";
				}
				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-bottom:10px;'>" + objDr["header"].ToString() + "</div>"));

				this.Controls.Add(new LiteralControl("<p>" + objDr["instruction"].ToString() + "</p>"));

				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-top:10px;margin-bottom:5px;'>" + objDr["subheader"].ToString() + "</div>"));

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
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

				RequiredFieldValidator objRfv = new RequiredFieldValidator();
				objRfv.ErrorMessage = "x&nbsp;";
				objRfv.ControlToValidate = "listbox_" + objDr["priority"].ToString();
				objRfv.Display = ValidatorDisplay.Dynamic;

				objHtc.Controls.Add(objRfv);

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[Convert.ToInt32(intPriority) - 1].Style.Add("width","80px");
				arrListBox[Convert.ToInt32(intPriority) - 1].ID = "listbox_" + objDr["priority"].ToString();

				arrListBox[Convert.ToInt32(intPriority) - 1].DataSource = db_list.select(strSql_list);

				arrListBox[Convert.ToInt32(intPriority) - 1].DataValueField = "_value";
				arrListBox[Convert.ToInt32(intPriority) - 1].DataTextField = "_option";

				arrListBox[Convert.ToInt32(intPriority) - 1].DataBind();

				ListItem objLi = new ListItem();

				objLi.Value = "";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[Convert.ToInt32(intPriority) - 1].Items.Insert(0,objLi);

				arrListBox[Convert.ToInt32(intPriority) - 1].Rows = 1;

				objHtc.Controls.Add(arrListBox[Convert.ToInt32(intPriority) - 1]);

				db_list.objDataReader.Close();
				db_list = null;

				objHtr.Controls.Add(objHtc);
		
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = strSubmitText + " (3/4)";
			submit.Click += new EventHandler(toStep3);
			submit.Attributes["style"] = "margin-top:20px;width:475px;";

			this.Controls.Add(submit);
		}

		private void drawStep3()
		{
			for(int i = 0;i < 2;i++)
			{
				arrListBox[i] = new ListBox();
			}

			Database db = new Database();

			string strSql = "SELECT id,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + Convert.ToInt32(Request.QueryString["page"]) + " AND step = 3;";
            			
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read())
			{
				if(!(Page.IsPostBack))
				{
					Page_header.InnerHtml += " - 3/4";
				}
				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-bottom:10px;'>" + objDr["header"].ToString() + "</div>"));

				this.Controls.Add(new LiteralControl("<p>" + objDr["instruction"].ToString() + "</p>"));

				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-top:10px;margin-bottom:5px;'>" + objDr["subheader"].ToString() + "</div>"));

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
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

				RequiredFieldValidator objRfv = new RequiredFieldValidator();
				objRfv.ErrorMessage = "x&nbsp;";
				objRfv.ControlToValidate = "listbox_" + objDr["priority"].ToString();
				objRfv.Display = ValidatorDisplay.Dynamic;

				objHtc.Controls.Add(objRfv);

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[Convert.ToInt32(intPriority) - 1].Style.Add("width","80px");
				arrListBox[Convert.ToInt32(intPriority) - 1].ID = "listbox_" + objDr["priority"].ToString();

				arrListBox[Convert.ToInt32(intPriority) - 1].DataSource = db_list.select(strSql_list);

				arrListBox[Convert.ToInt32(intPriority) - 1].DataValueField = "_value";
				arrListBox[Convert.ToInt32(intPriority) - 1].DataTextField = "_option";

				arrListBox[Convert.ToInt32(intPriority) - 1].DataBind();

				ListItem objLi = new ListItem();

				objLi.Value = "";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[Convert.ToInt32(intPriority) - 1].Items.Insert(0,objLi);

				arrListBox[Convert.ToInt32(intPriority) - 1].Rows = 1;

				objHtc.Controls.Add(arrListBox[Convert.ToInt32(intPriority) - 1]);

				db_list.objDataReader.Close();
				db_list = null;

				objHtr.Controls.Add(objHtc);
		
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = strSubmitText + " (4/4)";
			submit.Click += new EventHandler(toStep4);
			submit.Attributes["style"] = "margin-top:20px;width:475px;";

			this.Controls.Add(submit);
		}

		private void drawStep4()
		{
			Database db = new Database();

			string strSql = "SELECT id,submittext FROM questionaire WHERE pageid = " + Convert.ToInt32(Request.QueryString["page"]) + " AND step = 4;";
            			
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			if(objDr.Read())
			{
				if(!(Page.IsPostBack))
				{
					Page_header.InnerHtml += " - 4/4";
				}

				strSubmitText = objDr["submittext"].ToString();

				intQuestionaireId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();
			db = null;

			int q1 = 0;
			int q2 = 0;
			int q3 = 0;
			int q4 = 0;
			int q5 = 0;

			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[0])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[1])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[2])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[3])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[4])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[5])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[6])==1) 
			{
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[7])==1) 
			{
				q1++;
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[8])==1) 
			{
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[9])==1) 
			{
				q2++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[10])==1) 
			{
				q2++;
			}

			//step 2
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep2[0])==1) 
			{
				q3++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep2[1])==1) 
			{
				q3++;
			}

			//step 3
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep3[0])==1) 
			{
				q4++;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep3[1])==1) 
			{
				q4++;
			}
		
			if (q2<12) 
			{
				q5=4;
			}
			if (q2<7) 
			{
				q5=3;
			}
			if (q2<3) 
			{
				q5=2;
			}
			if (q1<3 && Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[12]) >= Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[11])) 
			{
				q5=1;
			}

			db = new Database();

			strSql = "SELECT body FROM questionaire_results WHERE type = "+q5+" AND questionaireid = "+intQuestionaireId;

			objDr = db.select(strSql);

			if(objDr.Read())
			{
				this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
			}
		
			db.objDataReader.Close();
			db = null;
	
		

			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[6])==1 || q4>0) 
			{
				// Tillægsgruppe 1
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 1 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
		
				db.objDataReader.Close();
				db = null;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[9])==1) 
			{
				// Tillægsgruppe 2
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 2 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
		
				db.objDataReader.Close();
				db = null;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[10])==1) 
			{
				// Tillægsgruppe 3
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 3 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
		
				db.objDataReader.Close();
				db = null;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[8])==1 && q3>0) 
			{
				// Tillægsgruppe 4
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 4 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
		
				db.objDataReader.Close();
				db = null;
			}
			if (Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[12])<Convert.ToInt32(((Questionaire)Session["questionaire"]).ArrStep1[11])) 
			{
				// Tillægsgruppe 5
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 5 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
		
				db.objDataReader.Close();
				db = null;
			}
				db = new Database();

				strSql = "SELECT body FROM questionaire_results WHERE subtype = 6 AND questionaireid = "+intQuestionaireId;

				objDr = db.select(strSql);

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p>" + objDr["body"].ToString() + "</p>"));
				}
			
				db.objDataReader.Close();
				db = null;

				Button submit = new Button();

				submit.ID = "submit";
				submit.Text = strSubmitText;
				submit.Style.Add("width","475px");				
				submit.Style.Add("margin-top","20px");	
				submit.Click +=new EventHandler(toStep1);

				this.Controls.Add(submit);
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

		private void toStep2(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Session["questionaire"] = new Questionaire();
 
				for(int i = 0;i < 13;i++)
				{
					((Questionaire)Session["questionaire"]).ArrStep1[i] = Convert.ToDouble(arrListBox[i].SelectedValue);
				}

				Questionaire.intStep = 2;

				Response.Redirect("?page=" + IntPageId);
			}
		}

		private void toStep3(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				for(int i = 0;i < 2;i++)
				{
					((Questionaire)Session["questionaire"]).ArrStep2[i] = Convert.ToDouble(arrListBox[i].SelectedValue);
				}

				Questionaire.intStep = 3;

				Response.Redirect("?page=" + IntPageId);
			}
		}

		private void toStep4(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				for(int i = 0;i < 2;i++)
				{
					((Questionaire)Session["questionaire"]).ArrStep3[i] = Convert.ToDouble(arrListBox[i].SelectedValue);
				}

				Questionaire.intStep = 4;

				Response.Redirect("?page=" + IntPageId);
			}
		}

		private void toStep1(object sender, EventArgs e)
		{
			Session["questionaire"] = null;

			Questionaire.intStep = 0;

			Response.Redirect("?page=" + IntPageId);

		}
	}
}
