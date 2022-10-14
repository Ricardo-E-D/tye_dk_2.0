namespace tye.uc.pages {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Mail;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using System.Web.UI;
	using exceptions;

	public partial class opt_client_admin : uc_pages {
		private string strMode;
		protected string empty;
		protected string strSql;
		private string[] arrInfos;

		protected TextBox usercode = new TextBox();
		protected TextBox name = new TextBox();
		protected TextBox address = new TextBox();
		protected TextBox zipcode = new TextBox();
		protected TextBox city = new TextBox();
		protected TextBox birthdate = new TextBox();
		protected TextBox phone = new TextBox();
		protected TextBox fax = new TextBox();
		protected TextBox email = new TextBox();
		protected ListBox domination = new ListBox();
		protected CheckBox access_www = new CheckBox();
		protected HtmlAnchor forward = new HtmlAnchor();
		protected HtmlAnchor back = new HtmlAnchor();
		protected HtmlAnchor program = new HtmlAnchor();

		protected ListBox[] arrListBox = new ListBox[24];
		protected TextBox[] arrTextBox = new TextBox[16];
		protected TextBox[] arrTb = new TextBox[72];
		protected RadioButton[] arrRb = new RadioButton[4];

		protected int intId;
		protected int intStep;
		protected int intStepSaved = 0;
		protected string strPassword;
		protected int intPriority = 1;
		protected int intArrInfosCount = 0;
		protected int intIsFirst = 1;

		protected string strChooseText;
		protected string strSubmitText;
		protected int intQuestionaireId;
		protected string strTitle;
		protected string jsErrorMsg;
		protected string strRef = "";

		protected void Page_Load(object sender, System.EventArgs e) {
			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);
			strRef = Request.QueryString["ref"];
			empty = Request.Params["empty"];

			try {
				switch(strMode) {
					case "jump2program":
						drawAddPageStep7();
						break;
					case "control": //Opret kontrolmåling
						drawControlPage();
						break;
					case "add": //Henter tilføjelsesskemaet til startmåling
						
						intStep = Convert.ToInt32(Request.QueryString["step"]);

						if(intStep == 0) {
							intStep = 1;
						}

						Database db = new Database();

						strSql = "SELECT optician_keys.id,optician_keys.password,temp_clients.step FROM optician_keys LEFT JOIN temp_clients ON optician_keys.id = temp_clients.passwordid WHERE optician_keys.id = " + intId; 
						MySqlDataReader objDr = db.select(strSql);

						if(!(objDr.HasRows)) {
							throw new NoDataFound();
						}
						else {
							if(objDr.Read()) {
								if(objDr["step"] != DBNull.Value) {
									intStepSaved = Convert.ToInt32(objDr["step"]);
								}
								else {
									intStepSaved = 0;
								}

								strPassword = objDr["password"].ToString();
							}
							if(intStep > 1 && intStep > intStepSaved && (intStep - intStepSaved) > 1) { //Kontrollerer at man ikke forsøger ulovligt trin
								throw new NoAccess();
							}
						}

						db.objDataReader.Close();
						db = null;

					switch(intStep) {
						case 1:
							drawAddPageStep1();
							break;
						case 2:
							drawAddPageStep2();
							break;
						case 3:
							drawAddPageStep3();
							break;
						case 4:
							drawAddPageStep4();
							break;
						case 5:
							drawAddPageStep5();
							break;
						case 6:
							drawAddPageStep6();
							break;
						case 7:
							drawAddPageStep7();
							break;
					}

						break;

					case "end": //Henter tilføjelsesskemaet til slutmåling
						
						intIsFirst = 0;

						intStep = Convert.ToInt32(Request.QueryString["step"]);

						if(intStep == 0) {
							intStep = 2;
						}

						db = new Database();

						strSql = "SELECT step FROM temp_clients WHERE userid = " + intId;  
						objDr = db.select(strSql);

						if(!(objDr.HasRows)) {
							if(intStep > 2) {
								db.objDataReader.Close();
								db.Dispose();
								db = null;
								throw new NoDataFound();
							}
						}
						else {
							if(objDr.Read()) {
								if(objDr["step"] != DBNull.Value) {
									intStepSaved = Convert.ToInt32(objDr["step"]);
								}
								else {
									intStepSaved = 0;
								}
							}
							if(intStep > 2 && intStep > intStepSaved && (intStep - intStepSaved) > 1) { //Kontrollerer at man ikke forsøger ulovligt trin
								db.objDataReader.Close();
								db.Dispose();
								db = null;
								throw new NoAccess();
							}
						}

						db.objDataReader.Close();
						db = null;
				
					switch(intStep) {
						case 2:
							drawAddPageStep2();
							break;
						case 3:
							drawAddPageStep3();
							break;
						case 4:
							drawAddPageStep4();
							break;
						case 5:
							drawAddPageStep5();
							break;
						case 6:
							drawAddPageStep6();
							break;
						case 7:
							drawAddPageStep7();
							break;
					}
						break;

					case "clear":
						clearKey(intId);
						break;

					default: //Henter default side afhængingt af sideid
						drawKeyList();
						break;
				}
			}
	
				//Fejlhåndtering

			catch(NoDataFound ndf) {
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}
			catch(NoAccess na) {
                this.Controls.Add(new LiteralControl(na.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}
			//catch(Exception E)
			//{
			//this.Controls.Add(new LiteralControl(E.Message ));
			//}
		}

		private void drawControlPage() {
			Literal js = new Literal();
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function popup(strname){\n";
			js.Text += "window.open('popups/21_test_popup.aspx?id='+strname,'Instruction','width=400,height=500,resizeable=no,toolbars=no,scrollbars=yes');\n";
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);
			
			for(int i = 0;i < 7;i++) {
				arrListBox[i] = new ListBox();
				arrTextBox[i] = new TextBox();
			}

			Database db = new Database();

			strSql = "SELECT id,header,instruction,subheader,choosetext,submittext,title FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 2;";	
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read()) {
				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));

				this.Controls.Add(new LiteralControl("<p><span class='page_subheader'>" + objDr["title"].ToString() + "</span> (<a href='javascript:void(0);' onclick=\"popup('1');\">" + arrInfos[17].ToString() + "</a>)</p>"));

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
			objHt.ID = "convergence";

			while(objDr.Read()) {
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

				Database db_list = new Database();
				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[intPriority-1].Style.Add("width","80px");
				arrListBox[intPriority-1].ID = "listbox_1_" + objDr["priority"].ToString();

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
			db = null;

			this.Controls.Add(objHt);

			intPriority = 1;

			db = new Database();
			strSql = "SELECT id,title,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 3;";	
			objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read()) {
				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));

				this.Controls.Add(new LiteralControl("<p><span class='page_subheader'>" + objDr["title"].ToString() + "</span> (<a href='javascript:void(0);' onclick=\"popup('2');\">" + arrInfos[20].ToString() + "</a>)</p>"));

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();

			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
			db = new Database();
			objDr = db.select(strSql);
            	
			objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "motilitet";

			while(objDr.Read()) {
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

				objHtc.Controls.Add(new LiteralControl("<br/>" + arrInfos[19].ToString() + " "));

				arrTextBox[intPriority-1].ID = "textbox_" + objDr["priority"].ToString();
				arrTextBox[intPriority-1].Width = 200;
				arrTextBox[intPriority-1].Style.Add("width","200px");

				objHtc.Controls.Add(arrTextBox[intPriority-1]);	

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

				arrListBox[intPriority+2].Style.Add("width","80px");
				arrListBox[intPriority+2].ID = "listbox_" + objDr["priority"].ToString();

				arrListBox[intPriority+2].DataSource = db_list.select(strSql_list);

				arrListBox[intPriority+2].DataValueField = "_value";
				arrListBox[intPriority+2].DataTextField = "_option";

				arrListBox[intPriority+2].DataBind();

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[intPriority+2].Items.Insert(0,objLi);

				arrListBox[intPriority+2].Rows = 1;

				objHtc.Controls.Add(arrListBox[intPriority+2]);

				db_list.objDataReader.Close();
				db_list = null;	

				objHtr.Controls.Add(objHtc);
		
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);

			this.Controls.Add(new LiteralControl("<p><span class='page_subheader'>" + arrInfos[21].ToString() + "</span></p>"));

			this.Controls.Add(new LiteralControl("<div class='bold_text'>" + arrInfos[22].ToString() + "</div>"));

			arrTextBox[4].Columns = 50;
			arrTextBox[4].Rows = 5;
			arrTextBox[4].Style.Add("width","475px");
			arrTextBox[4].Style.Add("height","100px");
			arrTextBox[4].TextMode = TextBoxMode.MultiLine;

			this.Controls.Add(arrTextBox[4]);

			this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-top:15px;'>" + arrInfos[23].ToString() + "</div>"));

			arrTextBox[5].Columns = 50;
			arrTextBox[5].Rows = 5;
			arrTextBox[5].Style.Add("width","475px");
			arrTextBox[5].Style.Add("height","100px");
			arrTextBox[5].TextMode = TextBoxMode.MultiLine;

			this.Controls.Add(arrTextBox[5]);

			Button submit = new Button();
			submit.Text = strSubmitText;
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","20px");
			submit.Click += new EventHandler(saveControl);

			this.Controls.Add(submit);

		}

		private void clearKey(int intKeyId) { //Nulstiller den pågældende nøgle
			Database db = new Database();

			string strSql = "DELETE FROM temp_clients WHERE passwordid = " + intId;

			db.execSql(strSql);

			db = null;

			Response.Redirect("?page=" + IntPageId);
		}

		private void drawKeyList() { //Viser tilgængelige nøgler for den pågældende optiker
			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function Clear(id){\n";
			js.Text += "var confirm = window.confirm('Er du sikker på at du vil nulstille denne nøgle?');\n";
			js.Text += "if(confirm){\n";
			js.Text += "location.href='?page=" + IntPageId + "&mode=clear&id='+id;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			Head_ph.Controls.Add(js);

			Database db = new Database();

			strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
			}

			db.objDataReader.Close();
			db = null;

			db = new Database();

			strSql = "SELECT id,password,DATE_FORMAT(addedtime,'%d-%m-%Y') AS thedate FROM optician_keys WHERE opticianid = " + ((Optician)Session["user"]).IntUserId + " AND optician_keys.isactive = 1 ORDER BY addedtime";
		
			objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			db.objDataReader.Close();

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

			objBc.DataField = "id";
			objBc.HeaderText = arrInfos[20].ToString();
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "password";
			objBc.HeaderText = arrInfos[21].ToString();
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "thedate";
			objBc.HeaderText = arrInfos[22].ToString();
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=add&id={0}";
			objHlc.HeaderText = arrInfos[23].ToString();
			objHlc.Text = arrInfos[23].ToString();
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";


			objDg.Columns.Add(objHlc);
			
			objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "javascript:Clear({0});";
			objHlc.HeaderText = arrInfos[24].ToString();
			objHlc.Text = arrInfos[24].ToString();
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			this.Controls.Add(objDg);

			db.objDataReader.Close();
			db = null;
		}

		private void drawAddPageStep1() { //Trin 1 af tilføjelsesskemaet
			if(!(Page.IsPostBack)) {
				Page_header.InnerHtml += " - 1/6";
			}

			Database db = new Database();

			strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
			}

			db.objDataReader.Close();
			db = null;	


			this.Controls.Add(new LiteralControl("<div class='page_subheader' style='margin-bottom:15px;'>" + arrInfos[1].ToString() + "</div>"));
			
			// se træningsprogram ;					
			program.ID = "Jump2Program";						
			program.InnerHtml = "Træningsprogram";
			program.HRef = "../../?page=" + IntPageId + "&mode=jump2program&id=" + intId+"&empty=empty";	
			if(intStepSaved == 0) {
				this.Controls.Add( new LiteralControl("<div align=right>"));
				this.Controls.Add(program);
				this.Controls.Add( new LiteralControl("</div>"));
			}
			// se træningsprogram ;
			this.Controls.Add(new LiteralControl("<p>" + arrInfos[0].ToString() + "</p>"));
			this.Controls.Add(new LiteralControl(arrInfos[2].ToString() + ":<br/>"));

			usercode.ID = "usercode";
			usercode.Width = 12;
			usercode.Style.Add("width","55px");
			usercode.ReadOnly = true;
			usercode.Style.Add("background","#cccccc");

			this.Controls.Add(usercode);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[3].ToString() + ": * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();

			name_val.ID = "name_val";
			name_val.ControlToValidate = "name";
			name_val.ErrorMessage = arrInfos[14].ToString();
			name_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(name_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 50;
            name.MaxLength = 255;
			name.Style.Add("width","200px");

			this.Controls.Add(name);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[4].ToString() + ":"));

			//RequiredFieldValidator address_val = new RequiredFieldValidator();
			//address_val.ID = "address_val";
			//address_val.ControlToValidate = "address";
			//address_val.ErrorMessage = arrInfos[14].ToString();
			//address_val.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(address_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			address.ID = "address";
			address.Width = 50;
            address.MaxLength = 255;
			address.Style.Add("width","200px");

			this.Controls.Add(address);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[5].ToString() + ":"));
			
			//RequiredFieldValidator zipcode_val = new RequiredFieldValidator();
			//zipcode_val.ID = "zipcode_val";
			//zipcode_val.ControlToValidate = "zipcode";
			//zipcode_val.ErrorMessage = arrInfos[14].ToString();
			//zipcode_val.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(zipcode_val);			

			this.Controls.Add(new LiteralControl("<br/>"));

			zipcode.ID = "zipcode";
			zipcode.Width = 8;
            zipcode.MaxLength = 10;
			zipcode.Style.Add("width","45px");

			this.Controls.Add(zipcode);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[6].ToString() + ":"));
			
			//RequiredFieldValidator city_val = new RequiredFieldValidator();
			//city_val.ID = "city_val";
			//city_val.ControlToValidate = "city";
			//city_val.ErrorMessage = arrInfos[14].ToString();
			//city_val.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(city_val);			

			this.Controls.Add(new LiteralControl("<br/>"));
			
			city.ID = "city";
			city.Width = 200;
            city.MaxLength = 255;
			city.Style.Add("width","200px");

			this.Controls.Add(city);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[7].ToString() + ":"));
			
			//RequiredFieldValidator birthdate_req = new RequiredFieldValidator();
			//birthdate_req.ID = "birthdate_req";
			//birthdate_req.ControlToValidate = "birthdate";
			//birthdate_req.ErrorMessage = arrInfos[15].ToString();
			//birthdate_req.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(birthdate_req);	

			RangeValidator birthdate_val = new RangeValidator();

			birthdate_val.Type = ValidationDataType.Date;
			birthdate_val.MaximumValue = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
			birthdate_val.MinimumValue = "01/01/1900";
			birthdate_val.ID = "birthdate_val";
			birthdate_val.ControlToValidate = "birthdate";
			birthdate_val.ErrorMessage = arrInfos[15].ToString();
			birthdate_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(birthdate_val);			

			this.Controls.Add(new LiteralControl("<br/>"));
			
			birthdate.ID = "birthdate";
			birthdate.Width = 15;
			birthdate.MaxLength = 10;
			birthdate.Text = "01/01/1900";
			birthdate.Style.Add("width","85px");
			birthdate.Attributes.Add("onfocus","if(this.value == '01/01/1900')this.value = '';");
			birthdate.Attributes.Add("onblur","if(this.value == '')this.value = '01/01/1900';");

			this.Controls.Add(birthdate);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[8].ToString() + ":"));

			//RequiredFieldValidator phone_val = new RequiredFieldValidator();
			//phone_val.ID = "phone_val";
			//phone_val.ControlToValidate = "phone";
			//phone_val.ErrorMessage = arrInfos[14].ToString();
			//phone_val.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(phone_val);
	
			this.Controls.Add(new LiteralControl("<br/>"));

			phone.ID = "phone";
			phone.Width = 200;
            phone.MaxLength = 50;
			phone.Style.Add("width","200px");

			this.Controls.Add(phone);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[9].ToString() + ":"));
			
			//RequiredFieldValidator fax_val = new RequiredFieldValidator();
			//fax_val.ID = "fax_val";
			//fax_val.ControlToValidate = "phone";
			//fax_val.ErrorMessage = arrInfos[14].ToString();
			//fax_val.Display = ValidatorDisplay.Dynamic;
			//this.Controls.Add(fax_val);
	
			this.Controls.Add(new LiteralControl("<br/>"));

			fax.ID = "fax";
			fax.Width = 200;
            fax.MaxLength = 50;
			fax.Style.Add("width","200px");

			this.Controls.Add(fax);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[10].ToString() + ": *"));

			RequiredFieldValidator email_val = new RequiredFieldValidator();
			email_val.ControlToValidate = "email";
			email_val.ErrorMessage = arrInfos[16].ToString();
			email_val.ID = "email_val";
			email_val.Display = ValidatorDisplay.Dynamic;	
			this.Controls.Add(email_val);

			RegularExpressionValidator email_reg = new RegularExpressionValidator();
			email_reg.ControlToValidate = "email";
			email_reg.ErrorMessage = arrInfos[16].ToString();
			email_reg.ID = "email_reg";
			email_reg.Display = ValidatorDisplay.Dynamic;				
			email_reg.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
			this.Controls.Add(email_reg);

			/*
			CustomValidator email_exist_val = new CustomValidator();
			email_exist_val.ID = "email_exist_val";
			email_exist_val.ErrorMessage = arrInfos[17].ToString();
			email_exist_val.ControlToValidate = "email";
			email_exist_val.Display = ValidatorDisplay.Dynamic;
			email_exist_val.ServerValidate += new ServerValidateEventHandler(email_exist_val_servervalidate);
			this.Controls.Add(email_exist_val);
			*/

			this.Controls.Add(new LiteralControl("<br/>"));

			email.ID = "email";
			email.Width = 65;
			email.MaxLength = 255;
			email.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(email);	
		
			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_www.ID = "access_www";
			access_www.Text = arrInfos[11].ToString();
			access_www.Checked = true;

			this.Controls.Add(access_www);

			this.Controls.Add(new LiteralControl("<p>" + arrInfos[12].ToString() + "</p><br/><br/><br/>"));

			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.ID = "buttons";
			objHt.Border = 0;
			objHt.Style.Add("width","475px");
			objHt.Style.Add("height","20px");

			HtmlTableRow objHtr = new HtmlTableRow();

			HtmlTableCell objHtc1 = new HtmlTableCell();

			objHtc1.Style.Add("width","50px");
			objHtc1.Style.Add("height","20px");

			HtmlAnchor back = new HtmlAnchor();
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");
			back.HRef = "../../?page=" + IntPageId;
			back.InnerHtml = "<<";

			objHtc1.Controls.Add(back);

			objHtr.Controls.Add(objHtc1);

			HtmlTableCell objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","375px");
			objHtc2.Style.Add("text-align","center");
			
			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = arrInfos[13].ToString();
			submit.Click += new EventHandler(saveStep1);

			objHtc2.Controls.Add(submit);

			objHtr.Controls.Add(objHtc2);

			HtmlTableCell objHtc3 = new HtmlTableCell();

			objHtc3.Style.Add("width","50px");
			objHtc3.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");

			forward.InnerHtml = ">>";

			if(intStepSaved == 0) {
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&mode=" + strMode + "&id=" + intId + "&step=2";

			objHtc3.Controls.Add(forward);

			objHtr.Controls.Add(objHtc3);

			objHt.Controls.Add(objHtr);

			this.Controls.Add(objHt);

			// Her sættes værdier hvis punktet har været gemt

			usercode.Text = strPassword;

			if(intStepSaved > 0) {
				db = new Database();

				strSql = "SELECT name,birthdate,address,zipcode,city,phone,fax,email,access_www FROM temp_clients WHERE passwordid = " + intId;

				objDr = db.select(strSql);

				if(objDr.Read()) {
					name.Text = objDr["name"].ToString();
					address.Text = objDr["address"].ToString();
					zipcode.Text = objDr["zipcode"].ToString();
					city.Text = objDr["city"].ToString();
					phone.Text = objDr["phone"].ToString();
					fax.Text = objDr["fax"].ToString();
					email.Text = objDr["email"].ToString();
					birthdate.Text = objDr["birthdate"].ToString().Substring(0,10).Replace("-","/");

					if(Convert.ToInt32(objDr["access_www"]) == 0) {
						access_www.Checked = false;
					}
				}
			
				db.objDataReader.Close();
				db = null;
			}
		}

		private void drawAddPageStep2() { //Viser trin 2 af tilføjelsesskemaet
			for(int i = 0;i < 20;i++) {
				arrListBox[i] = new ListBox();
			}

			Database db = new Database();

			strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
			}

			db.objDataReader.Close();
			db = null;	


			db = new Database();

			strSql = "SELECT id,title,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 1;";	
			objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read()) {
				if(!(Page.IsPostBack)) {
					Page_header.InnerHtml += " - 2/6";
				}

				this.Controls.Add(new LiteralControl("<div class='page_subheader' style='margin-bottom:10px;'>" + objDr["title"].ToString() + "</div>"));

				this.Controls.Add(new LiteralControl("<div class='bold_text'>" + objDr["header"].ToString() + "</div>"));
				// se træningsprogram ;					
				program.ID = "Jump2Program";						
				program.InnerHtml = "Træningsprogram";
				program.HRef = "../../?page=" + IntPageId + "&mode=jump2program&id=" + intId;	
				if(intStepSaved < 2) {
					this.Controls.Add( new LiteralControl("<div align=right>"));
					this.Controls.Add(program);
					this.Controls.Add( new LiteralControl("</div>"));
				}			
				// se træningsprogram ;
				this.Controls.Add(new LiteralControl("<p>" + objDr["instruction"].ToString() + "</p>"));

				this.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-bottom:5px;'>" + objDr["subheader"].ToString() + "</div>"));

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

			while(objDr.Read()) {
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
			db = null;

			this.Controls.Add(objHt);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

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

			if(intIsFirst == 1) {
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

			if(intStepSaved < 2) {
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=3";

			objHtc3.Controls.Add(forward);

			objHtr1.Controls.Add(objHtc3);

			objHt1.Controls.Add(objHtr1);

			this.Controls.Add(objHt1);

			if(intStepSaved > 1) { //Sætter værdier hvis punktet har været gemt
				db = new Database();

				strSql = "SELECT 2_1,2_2,2_3,2_4,2_5,2_6,2_7,2_8,2_9,2_10,2_11,2_12,2_13,2_14,2_15,2_16,2_17,2_18,2_19,2_20 FROM temp_clients WHERE ";
				
				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;
			
				objDr = db.select(strSql);

				if(objDr.Read()) {
					for(int i = 0;i < 20;i++) {
						arrListBox[i].SelectedValue = objDr["2_" + (i+1)].ToString();
					}
				}

				db.objDataReader.Close();
				db = null;
			}
			
		}

		private void drawAddPageStep3() { //Viser trin 3 af tilføjelsesskemaet
			for(int i = 0;i < 3;i++) {
				arrListBox[i] = new ListBox();
			}

			Database db = new Database();

			strSql = "SELECT id,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 2;";	
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read()) {
				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));

				if(!(Page.IsPostBack)) {
					Page_header.InnerHtml += " - 3/6";
				}

				this.Controls.Add(new LiteralControl("<p>" + arrInfos[0].ToString() + "</p>"));
				// se træningsprogram ;					
				program.ID = "Jump2Program";						
				program.InnerHtml = "Træningsprogram";
				program.HRef = "../../?page=" + IntPageId + "&mode=jump2program&id=" + intId;	
				if(intStepSaved < 3) {
					this.Controls.Add( new LiteralControl("<div align=right>"));
					this.Controls.Add(program);
					this.Controls.Add( new LiteralControl("</div>"));
				}			
				// se træningsprogram ;
				HtmlTable objHt = new HtmlTable();

				for(int i = 0;i < 5;i++) {
					intArrInfosCount++;

					HtmlTableRow objHtr = new HtmlTableRow();

					HtmlTableCell objHtc = new HtmlTableCell();

					objHtc.Style.Add("width","100px");
					objHtc.InnerHtml = arrInfos[intArrInfosCount].ToString();
					objHtc.Style.Add("vertical-align","top");
					objHtc.Style.Add("font-weight","bold");

					objHtr.Controls.Add(objHtc);

					HtmlTableCell objHtc1 = new HtmlTableCell();

					objHtc1.Style.Add("width","375px");
					objHtc1.Style.Add("vertical-align","top");

					HtmlGenericControl ol = new HtmlGenericControl("ol");
					ol.Style.Add("margin-top","0px;");

					int j = 0;

					switch(i) {
						case 0:
							j = 1;
							break;
						case 1:
							j = 2;
							break;
						case 2:
							j = 4;
							break;
						case 3:
							j = 3;
							break;
						case 4:
							j = 1;
							break;
					}

					for(int x = 0;x < j;x++) {
						intArrInfosCount++;

						HtmlGenericControl li = new HtmlGenericControl("li");

						li.InnerHtml = arrInfos[intArrInfosCount].ToString();

						ol.Controls.Add(li);
					}

					objHtc1.Controls.Add(ol);

					objHtr.Controls.Add(objHtc1);

					objHt.Controls.Add(objHtr);

				}

				this.Controls.Add(objHt);

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
				db = null;
			}
			
			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
			
			db = new Database();

			objDr = db.select(strSql);
            	
			HtmlTable objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt1.Attributes["class"] = "data_table";
			objHt1.ID = "data_table";

			while(objDr.Read()) {
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
		
				objHt1.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt1);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.ID = "buttons";
			objHt1.Border = 0;
			objHt1.Style.Add("width","475px");
			objHt1.Style.Add("height","20px");

			HtmlTableRow objHtr1 = new HtmlTableRow();

			HtmlTableCell objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","50px");
			objHtc2.Style.Add("height","20px");
			
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");

			back.InnerHtml = "<<";

			back.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=2";

			objHtc2.Controls.Add(back);

			objHtr1.Controls.Add(objHtc2);

			objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","375px");
			objHtc2.Style.Add("text-align","center");
			
			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = strSubmitText;
			submit.Click += new EventHandler(saveStep3);

			objHtc2.Controls.Add(submit);

			objHtr1.Controls.Add(objHtc2);

			HtmlTableCell objHtc3 = new HtmlTableCell();

			objHtc3.Style.Add("width","50px");
			objHtc3.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");

			forward.InnerHtml = ">>";

			if(intStepSaved < 3) {
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=4";

			objHtc3.Controls.Add(forward);

			objHtr1.Controls.Add(objHtc3);

			objHt1.Controls.Add(objHtr1);

			this.Controls.Add(objHt1);

			if(intStepSaved > 2) { //Henter værdier hvis punktet har været gemt
				db = new Database();

				strSql = "SELECT 3_1,3_2,3_3 FROM temp_clients WHERE ";
				
				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;
			
				objDr = db.select(strSql);

				if(objDr.Read()) {
					for(int i = 0;i < 3;i++) {
						arrListBox[i].SelectedValue = objDr["3_" + (i+1)].ToString();
					}
				}

				db.objDataReader.Close();
				db = null;
			}

		}

		private void drawAddPageStep4() { // Henter trin 4 af tilføjelsesskemaet
			for(int i = 0;i < 4;i++) {
				arrListBox[i] = new ListBox();
				arrTextBox[i] = new TextBox();
			}

			Database db = new Database();

			strSql = "SELECT id,header,instruction,subheader,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 3;";	
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
			
			if(objDr.Read()) {
				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));

				if(!(Page.IsPostBack)) {
					Page_header.InnerHtml += " - 4/6";
				}

				this.Controls.Add(new LiteralControl("<p>" + arrInfos[0].ToString() + "</p>"));
				// se træningsprogram ;					
				program.ID = "Jump2Program";						
				program.InnerHtml = "Træningsprogram";
				program.HRef = "../../?page=" + IntPageId + "&mode=jump2program&id=" + intId;	
				if(intStepSaved < 4) {
					this.Controls.Add( new LiteralControl("<div align=right>"));
					this.Controls.Add(program);
					this.Controls.Add( new LiteralControl("</div>"));
				}			
				// se træningsprogram ;
				HtmlTable objHt = new HtmlTable();

				for(int i = 0;i < 5;i++) {
					intArrInfosCount++;

					HtmlTableRow objHtr = new HtmlTableRow();

					HtmlTableCell objHtc = new HtmlTableCell();

					objHtc.Style.Add("width","100px");
					objHtc.InnerHtml = arrInfos[intArrInfosCount].ToString();
					objHtc.Style.Add("vertical-align","top");
					objHtc.Style.Add("font-weight","bold");

					objHtr.Controls.Add(objHtc);

					HtmlTableCell objHtc1 = new HtmlTableCell();

					objHtc1.Style.Add("width","375px");

					HtmlGenericControl ol = new HtmlGenericControl("ol");

					int j = 0;

					switch(i) {
						case 0:
							j = 1;
							break;
						case 1:
							j = 2;
							break;
						case 2:
							j = 5;
							break;
						case 3:
							j = 4;
							break;
						case 4:
							j = 1;
							break;
					}

					for(int x = 0;x < j;x++) {
						intArrInfosCount++;

						HtmlGenericControl li = new HtmlGenericControl("li");

						li.InnerHtml = arrInfos[intArrInfosCount].ToString();

						ol.Controls.Add(li);
					}

					objHtc1.Controls.Add(ol);

					objHtr.Controls.Add(objHtc1);

					objHt.Controls.Add(objHtr);

				}

				this.Controls.Add(objHt);

				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);

				db.objDataReader.Close();
				db = null;
			}
			
			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
			
			db = new Database();

			objDr = db.select(strSql);
            	
			HtmlTable objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt1.Attributes["class"] = "data_table";
			objHt1.ID = "data_table";

			while(objDr.Read()) {
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

				objHtc.Controls.Add(new LiteralControl("<br/>" + arrInfos[19].ToString() + " "));

				arrTextBox[intPriority-1].ID = "textbox_" + objDr["priority"].ToString();
                arrTextBox[intPriority - 1].MaxLength = 255;
				arrTextBox[intPriority-1].Style.Add("width","200px");

				objHtc.Controls.Add(arrTextBox[intPriority-1]);	

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
		
				objHt1.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt1);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.ID = "buttons";
			objHt1.Border = 0;
			objHt1.Style.Add("width","475px");
			objHt1.Style.Add("height","20px");

			HtmlTableRow objHtr1 = new HtmlTableRow();

			HtmlTableCell objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","50px");
			objHtc2.Style.Add("height","20px");
			
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");

			back.InnerHtml = "<<";

			back.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=3";

			objHtc2.Controls.Add(back);

			objHtr1.Controls.Add(objHtc2);

			objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","375px");
			objHtc2.Style.Add("text-align","center");
			
			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = strSubmitText;
			submit.Click += new EventHandler(saveStep4);

			objHtc2.Controls.Add(submit);

			objHtr1.Controls.Add(objHtc2);

			HtmlTableCell objHtc3 = new HtmlTableCell();

			objHtc3.Style.Add("width","50px");
			objHtc3.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");

			forward.InnerHtml = ">>";

			if(intStepSaved < 4) {
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=5";

			objHtc3.Controls.Add(forward);

			objHtr1.Controls.Add(objHtc3);

			objHt1.Controls.Add(objHtr1);

			this.Controls.Add(objHt1);

			if(intStepSaved > 3) { //Henter værdier hvis punktet har været gemt
				db = new Database();

				strSql = "SELECT 4_1,4_1_1,4_2,4_2_1,4_3,4_3_1,4_4,4_4_1 FROM temp_clients WHERE ";
				
				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;
			
				objDr = db.select(strSql);

				if(objDr.Read()) {
					for(int i = 0;i < 4;i++) {
						arrListBox[i].SelectedValue = objDr["4_" + (i+1)].ToString();
						arrTextBox[i].Text = objDr["4_" + (i+1) + "_1"].ToString();
					}
				}

				db.objDataReader.Close();
				db = null;
			}
		}

		private void drawAddPageStep5() { //Henter trin 5 af tilføjelsesskemaet
			for(int i = 0;i < 3;i++) {
				arrListBox[i] = new ListBox();
			}

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function disableObject(intCounter){\n";
			js.Text += "var selectBox = document.getElementById('_"+Files.strCtl+"_listbox_3_'+(intCounter + 1));\n";
			js.Text += "var textBox = document.getElementById('_"+Files.strCtl+"_textbox_3_'+ (intCounter + 5));\n";
			js.Text += "if(selectBox.value != '0'){\n";
			js.Text += "textBox.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "else\n";
			js.Text += "{\n";
			js.Text += "textBox.disabled = false;\n";
			js.Text += "}\n";
			js.Text += "if(textBox.value != ''){\n";
			js.Text += "selectBox.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "else\n";
			js.Text += "{\n";
			js.Text += "selectBox.disabled = false;\n";
			js.Text += "}\n";
			js.Text += "}\n";

			js.Text += "function disableOnLoad(){\n";
			js.Text += "var selectBox;\n";
			js.Text += "var textBox;\n";
			js.Text += "for(var i = 3;i < 12;i++){\n";
			js.Text += "selectBox = document.getElementById('_"+Files.strCtl+"_listbox_3_'+(i + 1));\n";
			js.Text += "textBox = document.getElementById('_"+Files.strCtl+"_textbox_3_'+ (i + 5));\n";
			js.Text += "if(selectBox.value != '0'){\n";
			js.Text += "textBox.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "else\n";
			js.Text += "{\n";
			js.Text += "textBox.disabled = false;\n";
			js.Text += "}\n";
			js.Text += "if(textBox.value != ''){\n";
			js.Text += "selectBox.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "else\n";
			js.Text += "{\n";
			js.Text += "selectBox.disabled = false;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";

			Page_body.Attributes["onload"] = "disableOnLoad();";

			HtmlGenericControl headerDiv = new HtmlGenericControl("div");

			headerDiv.Attributes["class"] = "bold_text";
			headerDiv.Style.Add("margin-bottom","10px");

			this.Controls.Add(headerDiv);
		
			Database db = new Database();

			strSql = "SELECT id,title,choosetext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 2;";	
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
		
			if(objDr.Read()) {
				intQuestionaireId = Convert.ToInt32(objDr["id"]);
				strTitle = objDr["title"].ToString();
				strChooseText = objDr["choosetext"].ToString();
			}

			db.objDataReader.Close();
			db = null;	
	
			//Henter værdier fra trin 3 af tilføjelsesskemaet

			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
		
			db = new Database();

			objDr = db.select(strSql);
            
			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "data_table";

			HtmlTableRow objHtr = new HtmlTableRow();

			HtmlTableCell objHtc = new HtmlTableCell();

			objHtc.ColSpan = 3;
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = strTitle + " - 3/6";

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			// se træningsprogram ;					
			objHtr = new HtmlTableRow();
			objHtc = new HtmlTableCell();
			program.ID = "Jump2Program";						
			program.InnerHtml = "Træningsprogram";
			program.HRef = "../../?page=" + IntPageId + "&mode=jump2program&id=" + intId;	
			if(intStepSaved < 5) {				
				objHtc.Controls.Add(program);				
			}	
			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);
			// se træningsprogram ;


			while(objDr.Read()) {
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();

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

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[intPriority-1].Style.Add("width","80px");
				arrListBox[intPriority-1].ID = "listbox_" + objDr["priority"].ToString();
				arrListBox[intPriority-1].Enabled = false;

				arrListBox[intPriority-1].DataSource = db_list.select(strSql_list);

				arrListBox[intPriority-1].DataValueField = "_value";
				arrListBox[intPriority-1].DataTextField = "_option";

				arrListBox[intPriority-1].DataBind();

				arrListBox[intPriority-1].Rows = 1;

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[intPriority-1].Items.Insert(0,objLi);

				objHtc.Controls.Add(arrListBox[intPriority-1]);

				db_list.objDataReader.Close();
				db_list = null;

				objHtr.Controls.Add(objHtc);
	
				objHt.Controls.Add(objHtr);

				intPriority++;
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);

			db = new Database();
			strSql = "SELECT 3_1,3_2,3_3 FROM temp_clients WHERE ";
			
			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				for(int i = 0;i < 3;i++) {
					arrListBox[i].SelectedValue = objDr["3_" + (i+1)].ToString();
				}
			}

			db.objDataReader.Close();
			db = null;

			//Henter værdier fra trin 4 af tilføjelsesskemaet

			intPriority = 1;

			for(int i = 0;i < 4;i++) {
				arrListBox[i] = new ListBox();
				arrTextBox[i] = new TextBox();
			}

			db = new Database();

			strSql = "SELECT id,title,choosetext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 3;";	
			objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}
		
			if(objDr.Read()) {
				intQuestionaireId = Convert.ToInt32(objDr["id"]);
				strTitle = objDr["title"].ToString();
				strChooseText = objDr["choosetext"].ToString();
			}

			db.objDataReader.Close();
			db = null;	

			objHtr = new HtmlTableRow();

			objHtc = new HtmlTableCell();

			objHtc.ColSpan = 3;
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = strTitle + " - 4/6";

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			strSql = "SELECT questions.id,question,priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
		
			db = new Database();

			objDr = db.select(strSql);
            
			while(objDr.Read()) {
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:15px;font-weight:bold;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = intPriority.ToString() + ".";

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:360px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["question"].ToString();

				objHtc.Controls.Add(new LiteralControl("<br/>Noter: "));

				arrTextBox[intPriority-1].ID = "textbox_2_" + objDr["priority"].ToString();
				arrTextBox[intPriority-1].MaxLength = 255;
				arrTextBox[intPriority-1].Style.Add("width","200px");
				arrTextBox[intPriority-1].Enabled = false;

				objHtc.Controls.Add(arrTextBox[intPriority-1]);	

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:100px;text-align:right;";
				objHtc.Attributes["class"] = "data_table_item";

				Database db_list = new Database();

				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[intPriority-1].Style.Add("width","80px");
				arrListBox[intPriority-1].ID = "listbox_2_" + objDr["priority"].ToString();
				arrListBox[intPriority-1].Enabled = false;

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
			db = null;

			this.Controls.Add(objHt);

			// Værdier sættes i trin 4

			db = new Database();
			strSql = "SELECT 4_1,4_1_1,4_2,4_2_1,4_3,4_3_1,4_4,4_4_1 FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				for(int i = 0;i < 4;i++) {
					arrListBox[i].SelectedValue = objDr["4_" + (i+1)].ToString();
					arrTextBox[i].Text = objDr["4_" + (i+1) + "_1"].ToString();
				}
			}

			db.objDataReader.Close();
			db = null;

			// Henter de felter der kan udfyldes for dette trin

			for(int i = 0;i < 13;i++) {
				arrListBox[i] = new ListBox();
			}

			for(int i = 0;i < 16;i++) {
				arrTextBox[i] = new TextBox();
			}

			objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "question_table";

			db = new Database();

			strSql = "SELECT id,title,header,instruction,choosetext,submittext FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 4;";	
			objDr = db.select(strSql);

			if(objDr.Read()) {
				headerDiv.InnerHtml = objDr["header"].ToString();

				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));
				strSubmitText = objDr["submittext"].ToString();
				intQuestionaireId = Convert.ToInt32(objDr["id"]);
				strChooseText = objDr["choosetext"].ToString();
				strTitle = objDr["title"].ToString();
			}

			db.objDataReader.Close();
			db = null;

            //js.Text += "function checkTextbox(textbox){\n";
            //js.Text += "if(isNaN(textbox.value) == true && textbox.value.indexOf(',') < 1){\n";
            //js.Text += "textbox.value = '';\n";
            //js.Text += "window.alert('"+arrInfos[12].ToString()+"');\n";
            //js.Text += "}\n";
            //js.Text += "}\n";
            //js.Text += "</script>\n";

            js.Text += "function checkTextbox(textbox){\n";
            js.Text += "return(do_check_price(textbox, false));\n";
            //js.Text += "var num = parseInt(textbox.value);\n";
            //js.Text += "if (isNaN(num)) {\n";
            //js.Text += "textbox.value = '0';\n";
            //js.Text += "alert('" + arrInfos[12].ToString() + "');\n";
            //js.Text += "textbox.select();\n";
            //js.Text += "textbox.focus();";
            //js.Text += "return(false); }\n";
            //js.Text += "else {\n";
            //js.Text += "textbox.value = num;\n if(textbox.value.length>3) { textbox.value = textbox.value.substr(0, 3); }\n";
            js.Text += "}\n</script>";

			Head_ph.Controls.Add(js);

			db = new Database();
			strSql = "SELECT questions.id,questions.question,questions.priority FROM questions INNER JOIN questionaire_question ON questionid = questions.id WHERE questionaire_question.questionaireid = " + intQuestionaireId + " ORDER BY priority";
			objDr = db.select(strSql);

			if(!(objDr.HasRows)) {
				throw new NoDataFound();
			}

			objHtr = new HtmlTableRow();

			objHtc = new HtmlTableCell();

			objHtc.ColSpan = 4;
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = strTitle;

			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);
			objHt.Controls.Add(objHtr);

			objHtr = new HtmlTableRow();

			if(objDr.Read()) {			
				objHtc = new HtmlTableCell();
				objHtc.InnerHtml = objDr["question"].ToString();
				objHtc.Style.Add("width","130px");
				objHtc.Attributes["class"] = "data_table_item";
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.Style.Add("width","345px");
				objHtc.ColSpan = 3;

				//RequiredFieldValidator objRfv = new RequiredFieldValidator();
				//objRfv.ErrorMessage = "x&nbsp;";
				//objRfv.ControlToValidate = "listbox_3_1";
				//objRfv.Display = ValidatorDisplay.Static;
				//objHtc.Controls.Add(objRfv);

				arrListBox[0].ID = "listbox_3_1";
				arrListBox[0].Rows = 1;

				Database db_list = new Database();
				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[0].DataSource = db_list.select(strSql_list);
				arrListBox[0].DataValueField = "_value";
				arrListBox[0].DataTextField = "_option";
				arrListBox[0].DataBind();

				db_list.objDataReader.Close();
				db_list = null;

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[0].Items.Insert(0,objLi);

				objHtc.Controls.Add(arrListBox[0]);

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}

			if(objDr.Read()) {
				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();
				objHtc.InnerHtml = objDr["question"].ToString();
				objHtc.Attributes["class"] = "data_table_item";
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.ColSpan = 3;
					
				//RequiredFieldValidator objRfv = new RequiredFieldValidator();
				//objRfv.ErrorMessage = "x&nbsp;";
				//objRfv.ControlToValidate = "listbox_3_2";
				//objRfv.Display = ValidatorDisplay.Static;
				//objHtc.Controls.Add(objRfv);

				arrListBox[1].ID = "listbox_3_2";
				arrListBox[1].Rows = 1;

				Database db_list = new Database();
				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[1].DataSource = db_list.select(strSql_list);
				arrListBox[1].DataValueField = "_value";
				arrListBox[1].DataTextField = "_option";
				arrListBox[1].DataBind();

				db_list.objDataReader.Close();
				db_list = null;

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[1].Items.Insert(0,objLi);

				objHtc.Controls.Add(arrListBox[1]);

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}
			
			if(objDr.Read()) {
				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();
				objHtc.InnerHtml = objDr["question"].ToString();
				objHtc.Attributes["class"] = "data_table_item";
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.ColSpan = 3;

				//RequiredFieldValidator objRfv = new RequiredFieldValidator();
				//objRfv.ErrorMessage = "x&nbsp;";
				//objRfv.ControlToValidate = "listbox_3_3";
				//objRfv.Display = ValidatorDisplay.Static;
				//objHtc.Controls.Add(objRfv);

				arrListBox[2].ID = "listbox_3_3";
				arrListBox[2].Rows = 1;

				Database db_list = new Database();
				string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(objDr["id"]);

				arrListBox[2].DataSource = db_list.select(strSql_list);
				arrListBox[2].DataValueField = "_value";
				arrListBox[2].DataTextField = "_option";
				arrListBox[2].DataBind();

				db_list.objDataReader.Close();
				db_list = null;

				ListItem objLi = new ListItem();

				objLi.Value = "0";
				objLi.Text = strChooseText;
				objLi.Selected = true;

				arrListBox[2].Items.Insert(0,objLi);

				objHtc.Controls.Add(arrListBox[2]);

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}

			for(int i = 0;i < 5;i++) {
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = arrInfos[i].ToString();
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.ColSpan = 3;

				//RequiredFieldValidator objRfv = new RequiredFieldValidator();
				//objRfv.ErrorMessage = "x&nbsp;";
				//objRfv.ControlToValidate = "textbox_3_" + (i+1);
				//objRfv.Display = ValidatorDisplay.Static;
				//objHtc.Controls.Add(objRfv);

				arrTextBox[i].ID = "textbox_3_" + (i+1);
				arrTextBox[i].MaxLength = 255;
				arrTextBox[i].Style.Add("width","200px");

				objHtc.Controls.Add(arrTextBox[i]);

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);
			}
	
			objHtr = new HtmlTableRow();

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.InnerHtml = arrInfos[5].ToString();
			objHtc.Style.Add("width","130px");
			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.Style.Add("width","95px");

			//RequiredFieldValidator objRfv1 = new RequiredFieldValidator();
			//objRfv1.ErrorMessage = "x&nbsp;";
			//objRfv1.ControlToValidate = "textbox_3_6";
			//objRfv1.Display = ValidatorDisplay.Static;
			//objHtc.Controls.Add(objRfv1);

			objHtc.Controls.Add(new LiteralControl(arrInfos[6].ToString()));

			arrTextBox[5].ID = "textbox_3_6";
			arrTextBox[5].Width = 10;
			arrTextBox[5].Style.Add("width","30px");
			arrTextBox[5].Attributes["onchange"] = "checkTextbox(this);";

			objHtc.Controls.Add(arrTextBox[5]);

			objHtc.Controls.Add(new LiteralControl(" cm."));

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.Style.Add("width","250px");
			objHtc.ColSpan = 2;

			//objRfv1 = new RequiredFieldValidator();
			//objRfv1.ErrorMessage = "x&nbsp;";
			//objRfv1.ControlToValidate = "textbox_3_7";
			//objRfv1.Display = ValidatorDisplay.Static;
			//objHtc.Controls.Add(objRfv1);

			objHtc.Controls.Add(new LiteralControl(arrInfos[7].ToString()));

			arrTextBox[6].ID = "textbox_3_7";
			arrTextBox[6].Width = 10;
			arrTextBox[6].Style.Add("width","30px");
			arrTextBox[6].Attributes["onchange"] = "checkTextbox(this);";

			objHtc.Controls.Add(arrTextBox[6]);

			objHtc.Controls.Add(new LiteralControl(" cm."));

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			int intCounter = 3;
			string[] arrQuestions = new string[6];
			int[] arrIds = new int[6];

			for(int i = 0;i < 4;i++) {
				if(objDr.Read()) {
					arrQuestions[i] = objDr["question"].ToString();
					arrIds[i] = Convert.ToInt32(objDr["id"]);
				}
			}

			db.objDataReader.Close();
			db = null;

			for(int i = 8;i < 11;i++) {
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();
				objHtc.RowSpan = 3;
				objHtc.Style.Add("width","130px");
				objHtc.Style.Add("vertical-align","top");
				objHtc.InnerHtml = arrInfos[i].ToString();
				objHtc.Attributes["class"] = "data_table_item";

				objHtr.Controls.Add(objHtc);

				for(int j = 0;j < 3;j++) {
					if(j > 0) {
						objHtr = new HtmlTableRow();
					}

					objHtc = new HtmlTableCell();
					objHtc.Style.Add("width","105px");
					
					objHtc.Attributes["class"] = "data_table_item";

					//CustomValidator cu = new CustomValidator();
					//cu.ID = "cu_" + intCounter;
					//cu.ControlToValidate = arrListBox[intCounter].ID;
					//cu.ErrorMessage = "x&nbsp;";
					//cu.Display = ValidatorDisplay.Static;
					
					//switch(intCounter)
					//{
					//case 3:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_3);
					//break;
					//case 4:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_4);
					//break;
					//case 5:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_5);
					//break;
					//case 6:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_6);
					//break;
					//case 7:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_7);
					//break;
					//case 8:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_8);
					//break;
					//case 9:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_9);
					//break;
					//case 10:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_10);
					//break;
					//case 11:
					//cu.ServerValidate += new ServerValidateEventHandler(cu_11);
					//break;
					//}
				
					//objHtc.Controls.Add(cu);

					objHtc.Controls.Add(new LiteralControl(arrQuestions[j]));

					objHtr.Controls.Add(objHtc);

					objHtc = new HtmlTableCell();
					objHtc.Style.Add("width","105px");
					objHtc.Attributes["class"] = "data_table_item";
	
					arrListBox[intCounter].ID = "listbox_3_" + (intCounter+1);
					arrListBox[intCounter].Rows = 1;
					arrListBox[intCounter].Attributes["onchange"] = "disableObject(" + intCounter + ");";
					arrListBox[intCounter].Attributes["onload"] = "disableObject(" + intCounter + ");";

					Database db_list = new Database();
					string strSql_list = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + Convert.ToInt32(arrIds[j]) + " ORDER BY priority;";

					arrListBox[intCounter].DataSource = db_list.select(strSql_list);
					arrListBox[intCounter].DataValueField = "_value";
					arrListBox[intCounter].DataTextField = "_option";
					arrListBox[intCounter].DataBind();

					db_list.objDataReader.Close();
					db_list = null;

					ListItem objLi = new ListItem();

					objLi.Value = "0";
					objLi.Text = strChooseText;
					objLi.Selected = true;

					arrListBox[intCounter].Items.Insert(0,objLi);

					objHtc.Controls.Add(arrListBox[intCounter]);

					objHtr.Controls.Add(objHtc);
					
					objHtc = new HtmlTableCell();
					objHtc.Style.Add("width","135px");
					objHtc.Attributes["class"] = "data_table_item";

					objHtc.Controls.Add(new LiteralControl(arrInfos[11].ToString() + "&nbsp;"));

					arrTextBox[intCounter+4].ID = "textbox_3_" + (intCounter+5);
					arrTextBox[intCounter+4].Width = 15;
					arrTextBox[intCounter+4].Style.Add("width","75px");
                    //arrTextBox[intCounter + 4].Attributes["onchange"] = "checkTextbox(this);"; //added mital
                    arrTextBox[intCounter + 4].MaxLength = 100;
                    arrTextBox[intCounter+4].Attributes["onkeyup"] = "disableObject(" + intCounter + ");";
					arrTextBox[intCounter+4].Attributes["onload"] = "disableObject(" + intCounter + ");";

					objHtc.Controls.Add(arrTextBox[intCounter+4]);

					objHtr.Controls.Add(objHtc);

					objHt.Controls.Add(objHtr);						

					intCounter++;
				}
			}

			this.Controls.Add(objHt);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlTable objHt1 = new HtmlTable();

			objHt1.CellPadding = 0;
			objHt1.CellSpacing = 0;
			objHt1.ID = "buttons";
			objHt1.Border = 0;
			objHt1.Style.Add("width","475px");
			objHt1.Style.Add("height","20px");

			HtmlTableRow objHtr1 = new HtmlTableRow();

			HtmlTableCell objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","50px");
			objHtc2.Style.Add("height","20px");
			
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");

			back.InnerHtml = "<<";

			back.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=4";

			objHtc2.Controls.Add(back);

			objHtr1.Controls.Add(objHtc2);

			objHtc2 = new HtmlTableCell();

			objHtc2.Style.Add("width","375px");
			objHtc2.Style.Add("text-align","center");
			
			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = strSubmitText;
			submit.Click += new EventHandler(saveStep5);

			objHtc2.Controls.Add(submit);

			objHtr1.Controls.Add(objHtc2);

			HtmlTableCell objHtc3 = new HtmlTableCell();

			objHtc3.Style.Add("width","50px");
			objHtc3.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");

			forward.InnerHtml = ">>";

			if(intStepSaved < 5) {
				forward.Visible = false;
			}

			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=6";

			objHtc3.Controls.Add(forward);

			objHtr1.Controls.Add(objHtc3);

			objHt1.Controls.Add(objHtr1);

			this.Controls.Add(objHt1);

			if(intStepSaved > 4) {
				Database db1 = new Database();
				string strSql1 = "SELECT 5_1,5_2,5_3,5_4,5_5,5_6,5_7,5_8,5_9,5_10,5_11,5_12,5_13,5_14,5_15,5_16,5_17,5_18,5_19 FROM temp_clients WHERE ";

				if(intIsFirst == 1) {
					strSql1 += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql1 += "userid";
				}
				strSql1 += " = " + intId;

				MySqlDataReader objDr1 = db1.select(strSql1);

				if(objDr1.Read()) {
					for(int i = 0;i < 3;i++) {
						arrListBox[i].SelectedValue = objDr1["5_" + (i+1)].ToString();
					}
					for(int i = 0;i < 7;i++) {
						arrTextBox[i].Text = objDr1["5_" + (i+4)].ToString();
					}

					for(int i = 0;i < 9;i++) {
						string strValue = objDr1["5_" + (i+11)].ToString();
						
						if(strValue != "0" && strValue != "0,1" && strValue != "0,2" && strValue != "0,3" && strValue != "0,4" && strValue != "0,5" && strValue != "0,6" && strValue != "0,7" && strValue != "0,8" && strValue != "0,9" && strValue != "1,0" && strValue != "1,1" && strValue != "1,2" && strValue != "1,3" && strValue != "1,4" && strValue != "1,5") {
							arrTextBox[i+7].Text = strValue;
						}
						else {
							try 
							{
								arrListBox[i+3].SelectedValue = strValue;
							}
							catch(Exception e) 
							{
							}
						}
					}
				}
				db1.objDataReader.Close();
				db1 = null;
			}

		}
		private void drawAddPageStep6() { //Henter trin 6 af tilføjelsesskemaet
			int[] arrQ = new int[24];
			int intCounter = 0;
			
			for(int i = 0;i < 72;i++) {
				arrTb[i] = new TextBox();
			}

			for(int i = 0;i < 24;i++) {
				arrListBox[i] = new ListBox();
			}

			for(int i = 0;i < 4;i++) {
				arrRb[i] = new RadioButton();
			}

			Literal js = new Literal();
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function popup(strname){\n";
			js.Text += "window.open('popups/21_test_popup.aspx?id='+strname,'Instruction','width=400,height=500,resizeable=no,toolbars=no,scrollbars=yes');\n";
			js.Text += "}\n";

			js.Text += "function disableObject(intRbId,intTbId){\n";
			js.Text += "var textBox1 = document.getElementById('_"+Files.strCtl+"_textbox_'+intTbId);\n";
			js.Text += "var textBox2 = document.getElementById('_"+Files.strCtl+"_textbox_'+(intTbId+1));\n";
			js.Text += "var textBox3 = document.getElementById('_"+Files.strCtl+"_textbox_'+(intTbId+2));\n";
			js.Text += "var textBox4 = document.getElementById('_"+Files.strCtl+"_textbox_'+(intTbId+3));\n";
			js.Text += "if(intRbId == 0 || intRbId == 2){\n";
			js.Text += "textBox1.disabled = false;\n";
			js.Text += "textBox2.disabled = false;\n";
			js.Text += "textBox3.value = '';\n";
			js.Text += "textBox4.value = '';\n";
			js.Text += "textBox3.disabled = true;\n";
			js.Text += "textBox4.disabled = true;\n";
			js.Text += "}else{\n";
			js.Text += "textBox1.disabled = true;\n";
			js.Text += "textBox2.disabled = true;\n";
			js.Text += "textBox1.value = '';\n";
			js.Text += "textBox2.value = '';\n";
			js.Text += "textBox3.disabled = false;\n";
			js.Text += "textBox4.disabled = false;\n";
			js.Text += "}\n";
			js.Text += "}\n";

			js.Text += "function disableOnLoad(){\n";
			js.Text += "var radiobutton1 = document.getElementById('_"+Files.strCtl+"_radiobutton_0');\n";
			js.Text += "var radiobutton2 = document.getElementById('_"+Files.strCtl+"_radiobutton_1');\n";
			js.Text += "var radiobutton3 = document.getElementById('_"+Files.strCtl+"_radiobutton_2');\n";
			js.Text += "var radiobutton4 = document.getElementById('_"+Files.strCtl+"_radiobutton_3');\n";
			js.Text += "var textBox1 = document.getElementById('_"+Files.strCtl+"_textbox_38');\n";
			js.Text += "var textBox2 = document.getElementById('_"+Files.strCtl+"_textbox_39');\n";
			js.Text += "var textBox3 = document.getElementById('_"+Files.strCtl+"_textbox_40');\n";
			js.Text += "var textBox4 = document.getElementById('_"+Files.strCtl+"_textbox_41');\n";
			js.Text += "var textBox5 = document.getElementById('_"+Files.strCtl+"_textbox_63');\n";
			js.Text += "var textBox6 = document.getElementById('_"+Files.strCtl+"_textbox_64');\n";
			js.Text += "var textBox7 = document.getElementById('_"+Files.strCtl+"_textbox_65');\n";
			js.Text += "var textBox8 = document.getElementById('_"+Files.strCtl+"_textbox_66');\n";
			js.Text += "if(radiobutton1.checked){\n";
			js.Text += "textBox3.disabled = true;\n";
			js.Text += "textBox4.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "if(radiobutton2.checked){\n";
			js.Text += "textBox1.disabled = true;\n";
			js.Text += "textBox2.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "if(radiobutton3.checked){\n";
			js.Text += "textBox7.disabled = true;\n";
			js.Text += "textBox8.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "if(radiobutton4.checked){\n";
			js.Text += "textBox5.disabled = true;\n";
			js.Text += "textBox6.disabled = true;\n";
			js.Text += "}\n";
			js.Text += "}\n";

			Page_body.Attributes["onload"] = "disableOnLoad();";

			Database db = new Database();
			string strSql = "SELECT id,title,choosetext,submittext,instruction FROM questionaire WHERE pageid = " + IntSubmenuId + " AND step = 5;";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				arrInfos = objDr["instruction"].ToString().Split(Convert.ToChar("^"));
				intQuestionaireId = Convert.ToInt32(objDr["id"]);
				strTitle = objDr["title"].ToString();
				strChooseText = objDr["choosetext"].ToString();
				strSubmitText = objDr["submittext"].ToString();
			}

			db.objDataReader.Close();
			db = null;

			js.Text += "function checkTextboxAll(textbox){\n";
			js.Text += "if(isNaN(textbox.value) == true && textbox.value.indexOf(',') < 1){\n";
			js.Text += "if(textbox.value.indexOf('-') == -1 && textbox.value.indexOf('x') == -1){\n";
			js.Text += "textbox.value = '';\n";
			js.Text += "window.alert('"+arrInfos[14].ToString()+"');\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";

			js.Text += "function checkTextbox(textbox){\n";
			js.Text += "if(isNaN(textbox.value) == true && textbox.value.indexOf(',') < 1){\n";
			js.Text += "textbox.value = '';\n";
			js.Text += "window.alert('"+arrInfos[15].ToString()+"');\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			Head_ph.Controls.Add(js);

			db = new Database();
			strSql = "SELECT questions.id FROM questions INNER JOIN questionaire_question ON questions.id = questionid WHERE questionaireid = " + intQuestionaireId + " ORDER BY priority;";
			objDr = db.select(strSql);

			while(objDr.Read()) {
				arrQ[intCounter] = Convert.ToInt32(objDr["id"]);
				intCounter++;
			}

			db.objDataReader.Close();
			db = null;

			HtmlTable objHt = new HtmlTable();
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.ID = "data_table";
			objHt.Border = 0;
			objHt.Style.Add("width","475px");
			objHt.Style.Add("border-collapse","collapse");		
			objHt.Attributes["class"] = "data_table";

			//Række starter her

			HtmlTableRow objHtr = new HtmlTableRow();
			
			HtmlTableCell objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","475px");
			objHtc.ColSpan = 2;
			objHtc.InnerHtml = strTitle;
			objHtc.Attributes["class"] = "data_table_header";
			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her	

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","70px");
			objHtc.Attributes["class"] = "data_table_item";

			HtmlAnchor popup = new HtmlAnchor();
			popup.InnerHtml = "#3:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('3');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","405px;");
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[0].ID = "textbox_0";
			arrTb[0].Style.Add("width","40px");
			arrTb[0].MaxLength = 15;
			//arrTb[0].Attributes["onkeyup"] = "checkTextboxAll(this)";
            arrTb[0].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[0]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[0].ID = "listbox_0";
			arrListBox[0].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[0].DataSource = db.select(strSql);
			arrListBox[0].DataTextField = "_option";
			arrListBox[0].DataValueField = "_value";
			arrListBox[0].DataBind();

			db.objDataReader.Close();
			db = null;

			ListItem objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[0].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[0]);
			objHtr.Controls.Add(objHtc);
    		objHt.Controls.Add(objHtr);
			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#13A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('13A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[1].ID = "textbox_1";
			arrTb[1].Style.Add("width","40px");
			arrTb[1].MaxLength = 15;
			//arrTb[1].Attributes["onkeyup"] = "checkTextboxAll(this)";
            arrTb[1].Attributes["onchange"] = "do_check_price_max(this, false, 100)";
            
            objHtc.Controls.Add(arrTb[1]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[1].ID = "listbox_1";
			arrListBox[1].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[1].DataSource = db.select(strSql);
			arrListBox[1].DataTextField = "_option";
			arrListBox[1].DataValueField = "_value";
			arrListBox[1].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[1].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[1]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#4:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('4');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[2].ID = "listbox_2";
			arrListBox[2].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[2].DataSource = db.select(strSql);
			arrListBox[2].DataTextField = "_option";
			arrListBox[2].DataValueField = "_value";
			arrListBox[2].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[2].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[2]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[2].ID = "textbox_2";
			arrTb[2].Style.Add("width","40px");
			arrTb[2].MaxLength = 15;
			//arrTb[2].Attributes["onkeyup"] = "checkTextbox(this)";
            arrTb[2].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[2]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[3].ID = "textbox_3";
			arrTb[3].Style.Add("width","40px");
			//arrTb[3].Attributes["onkeyup"] = "checkTextboxAll(this)";
            arrTb[3].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[3]);

			arrTb[4].ID = "textbox_4";
			arrTb[4].Style.Add("width","40px");
			//arrTb[4].Attributes["onkeyup"] = "checkTextbox(this)";
            arrTb[4].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[4]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[5].ID = "textbox_5";
			arrTb[5].Style.Add("width","40px");
			//arrTb[5].Attributes["onkeyup"] = "checkTextboxAll(this)";
            arrTb[5].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[5]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[3].ID = "listbox_3";
			arrListBox[3].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[3].DataSource = db.select(strSql);
			arrListBox[3].DataTextField = "_option";
			arrListBox[3].DataValueField = "_value";
			arrListBox[3].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[3].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[3]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[6].ID = "textbox_6";
			arrTb[6].Style.Add("width","40px");
			//arrTb[6].Attributes["onkeyup"] = "checkTextbox(this)";
            arrTb[6].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[6]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[7].ID = "textbox_7";
			arrTb[7].Style.Add("width","40px");
            arrTb[7].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[7]);

			arrTb[8].ID = "textbox_8";
			arrTb[8].Style.Add("width","40px");
            arrTb[8].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[8]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[9].ID = "textbox_9";
			arrTb[9].Style.Add("width","40px");
            arrTb[9].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[9]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#5:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('5');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[4].ID = "listbox_4";
			arrListBox[4].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[4].DataSource = db.select(strSql);
			arrListBox[4].DataTextField = "_option";
			arrListBox[4].DataValueField = "_value";
			arrListBox[4].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[4].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[4]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[10].ID = "textbox_10";
			arrTb[10].Style.Add("width","40px");
            arrTb[10].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[10]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[11].ID = "textbox_11";
			arrTb[11].Style.Add("width","40px");
			arrTb[11].Enabled = false;

			objHtc.Controls.Add(arrTb[11]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[12].ID = "textbox_12";
			arrTb[12].Style.Add("width","40px");
			arrTb[12].Enabled = false;

			objHtc.Controls.Add(arrTb[12]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[5].ID = "listbox_5";
			arrListBox[5].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[5].DataSource = db.select(strSql);
			arrListBox[5].DataTextField = "_option";
			arrListBox[5].DataValueField = "_value";
			arrListBox[5].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[5].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[5]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[13].ID = "textbox_13";
			arrTb[13].Style.Add("width","40px");
            arrTb[13].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[13]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[14].ID = "textbox_14";
			arrTb[14].Style.Add("width","40px");
			arrTb[14].Enabled = false;

			objHtc.Controls.Add(arrTb[14]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[15].ID = "textbox_15";
			arrTb[15].Style.Add("width","40px");
			arrTb[15].Enabled = false;

			objHtc.Controls.Add(arrTb[15]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#7:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('7');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[6].ID = "listbox_6";
			arrListBox[6].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[6].DataSource = db.select(strSql);
			arrListBox[6].DataTextField = "_option";
			arrListBox[6].DataValueField = "_value";
			arrListBox[6].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[6].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[6]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[16].ID = "textbox_16";
			arrTb[16].Style.Add("width","40px");
            arrTb[16].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[16]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[17].ID = "textbox_17";
			arrTb[17].Style.Add("width","40px");
            arrTb[17].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[17]);

			arrTb[18].ID = "textbox_18";
			arrTb[18].Style.Add("width","40px");
            arrTb[18].Attributes["onchange"] = "do_check_price_max(this, false, 400)";
			

			objHtc.Controls.Add(arrTb[18]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[19].ID = "textbox_19";
			arrTb[19].Style.Add("width","40px");
            arrTb[19].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[19]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[7].ID = "listbox_7";
			arrListBox[7].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[7].DataSource = db.select(strSql);
			arrListBox[7].DataTextField = "_option";
			arrListBox[7].DataValueField = "_value";
			arrListBox[7].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[7].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[7]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[20].ID = "textbox_20";
			arrTb[20].Style.Add("width","40px");
            arrTb[20].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[20]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[21].ID = "textbox_21";
			arrTb[21].Style.Add("width","40px");
            arrTb[21].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[21]);

			arrTb[22].ID = "textbox_22";
			arrTb[22].Style.Add("width","40px");
            arrTb[22].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[22]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[23].ID = "textbox_23";
			arrTb[23].Style.Add("width","40px");
            arrTb[23].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[23]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#7A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('7A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[8].ID = "listbox_8";
			arrListBox[8].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[8].DataSource = db.select(strSql);
			arrListBox[8].DataTextField = "_option";
			arrListBox[8].DataValueField = "_value";
			arrListBox[8].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[8].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[8]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[24].ID = "textbox_24";
			arrTb[24].Style.Add("width","40px");
            arrTb[24].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[24]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[25].ID = "textbox_25";
			arrTb[25].Style.Add("width","40px");
            arrTb[25].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[25]);

			arrTb[26].ID = "textbox_26";
			arrTb[26].Style.Add("width","40px");
            arrTb[26].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[26]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[27].ID = "textbox_27";
			arrTb[27].Style.Add("width","40px");
            arrTb[27].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[27]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[9].ID = "listbox_9";
			arrListBox[9].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[9].DataSource = db.select(strSql);
			arrListBox[9].DataTextField = "_option";
			arrListBox[9].DataValueField = "_value";
			arrListBox[9].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[9].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[9]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[28].ID = "textbox_28";
			arrTb[28].Style.Add("width","40px");
            arrTb[28].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[28]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[1].ToString() + " "));

			arrTb[29].ID = "textbox_29";
			arrTb[29].Style.Add("width","40px");
            arrTb[29].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[29]);

			arrTb[30].ID = "textbox_30";
			arrTb[30].Style.Add("width","40px");
            arrTb[30].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[30]);

			objHtc.Controls.Add(new LiteralControl(arrInfos[2].ToString() + " " + arrInfos[3].ToString() + " "));

			arrTb[31].ID = "textbox_31";
			arrTb[31].Style.Add("width","40px");
            arrTb[31].Attributes["onchange"] = "do_check_price_max(this, false, 400)";

			objHtc.Controls.Add(arrTb[31]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#8:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('8');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[32].ID = "textbox_32";
			arrTb[32].Style.Add("width","40px");
            arrTb[32].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[32]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[10].ID = "listbox_10";
			arrListBox[10].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[10].DataSource = db.select(strSql);
			arrListBox[10].DataTextField = "_option";
			arrListBox[10].DataValueField = "_value";
			arrListBox[10].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[10].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[10]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#9:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('9');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[33].ID = "textbox_33";
			arrTb[33].Style.Add("width","40px");
            arrTb[33].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[33]);
			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#10:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('10');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[34].ID = "textbox_34";
			arrTb[34].Style.Add("width","40px");
            arrTb[34].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[34]);

			objHtc.Controls.Add(new LiteralControl("/"));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[35].ID = "textbox_35";
			arrTb[35].Style.Add("width","40px");
            arrTb[35].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[35]);

			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[11].ID = "listbox_11";
			arrListBox[11].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[3] + " ORDER BY priority;";

			arrListBox[11].DataSource = db.select(strSql);
			arrListBox[11].DataTextField = "_option";
			arrListBox[11].DataValueField = "_value";
			arrListBox[11].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[11].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[11]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#11:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('11');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[36].ID = "textbox_36";
			arrTb[36].Style.Add("width","40px");
            arrTb[36].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[36]);

			objHtc.Controls.Add(new LiteralControl("/"));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[37].ID = "textbox_37";
			arrTb[37].Style.Add("width","40px");
            arrTb[37].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[37]);

			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[12].ID = "listbox_12";
			arrListBox[12].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[3] + " ORDER BY priority;";

			arrListBox[12].DataSource = db.select(strSql);
			arrListBox[12].DataTextField = "_option";
			arrListBox[12].DataValueField = "_value";
			arrListBox[12].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[12].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[12]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#12:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('12');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrListBox[13].ID = "listbox_13";
			arrListBox[13].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[4] + " ORDER BY priority;";

			arrListBox[13].DataSource = db.select(strSql);
			arrListBox[13].DataTextField = "_option";
			arrListBox[13].DataValueField = "_value";
			arrListBox[13].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[13].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[13]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[6].ToString() + " "));

			arrRb[0].ID = "radiobutton_0";
			arrRb[0].GroupName = "0_1";
			arrRb[0].Attributes["onclick"] = "disableObject(0,38);";
			objHtc.Controls.Add(arrRb[0]);
			
			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[7].ToString() + " "));

			arrTb[38].ID = "textbox_38";
			arrTb[38].Style.Add("width","40px");
            arrTb[38].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[38]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[8].ToString() + " "));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[39].ID = "textbox_39";
			arrTb[39].Style.Add("width","40px");
            arrTb[39].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[39]);

			objHtc.Controls.Add(new LiteralControl(" "));

			objHtc.Controls.Add(new LiteralControl("<br> " + arrInfos[9].ToString() + " "));

			arrRb[1].ID = "radiobutton_1";
			arrRb[1].GroupName = "0_1";
			arrRb[1].Attributes["onclick"] = "disableObject(1,38);";
			objHtc.Controls.Add(arrRb[1]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[7].ToString() + " "));

			arrTb[40].ID = "textbox_40";
			arrTb[40].Style.Add("width","40px");
            arrTb[40].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[40]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[8].ToString() + " "));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[41].ID = "textbox_41";
			arrTb[41].Style.Add("width","40px");
            arrTb[41].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[41]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#13B:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('13B');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[42].ID = "textbox_42";
			arrTb[42].Style.Add("width","40px");
            arrTb[42].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[42]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[14].ID = "listbox_14";
			arrListBox[14].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[14].DataSource = db.select(strSql);
			arrListBox[14].DataTextField = "_option";
			arrListBox[14].DataValueField = "_value";
			arrListBox[14].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[14].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[14]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#14A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('14A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[15].ID = "listbox_15";
			arrListBox[15].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[15].DataSource = db.select(strSql);
			arrListBox[15].DataTextField = "_option";
			arrListBox[15].DataValueField = "_value";
			arrListBox[15].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[15].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[15]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[43].ID = "textbox_43";
			arrTb[43].Style.Add("width","40px");
            arrTb[43].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[43]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[44].ID = "textbox_44";
			arrTb[44].Style.Add("width","40px");
			arrTb[44].Enabled = false;

			objHtc.Controls.Add(arrTb[44]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[45].ID = "textbox_45";
			arrTb[45].Style.Add("width","40px");
			arrTb[45].Enabled = false;

			objHtc.Controls.Add(arrTb[45]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[16].ID = "listbox_16";
			arrListBox[16].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[16].DataSource = db.select(strSql);
			arrListBox[16].DataTextField = "_option";
			arrListBox[16].DataValueField = "_value";
			arrListBox[16].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[16].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[16]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[46].ID = "textbox_46";
			arrTb[46].Style.Add("width","40px");
            arrTb[46].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[46]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[47].ID = "textbox_47";
			arrTb[47].Style.Add("width","30px");
			arrTb[47].MaxLength = 15;
			arrTb[47].Enabled = false;

			objHtc.Controls.Add(arrTb[47]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[48].ID = "textbox_48";
			arrTb[48].Style.Add("width","30px");
			arrTb[48].MaxLength = 15;
			arrTb[48].Enabled = false;

			objHtc.Controls.Add(arrTb[48]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#15A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('15A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[49].ID = "textbox_49";
			arrTb[49].Style.Add("width","40px");
            arrTb[49].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[49]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[17].ID = "listbox_17";
			arrListBox[17].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[17].DataSource = db.select(strSql);
			arrListBox[17].DataTextField = "_option";
			arrListBox[17].DataValueField = "_value";
			arrListBox[17].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[17].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[17]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";
			objHtc.RowSpan = 2;
			objHtc.Style.Add("vertical-align","top");

			popup = new HtmlAnchor();
			popup.InnerHtml = "#14B:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('14B');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[1];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[18].ID = "listbox_18";
			arrListBox[18].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[1] + " ORDER BY priority;";

			arrListBox[18].DataSource = db.select(strSql);
			arrListBox[18].DataTextField = "_option";
			arrListBox[18].DataValueField = "_value";
			arrListBox[18].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[18].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[18]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[50].ID = "textbox_50";
			arrTb[50].Style.Add("width","40px");
            arrTb[50].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[50]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[51].ID = "textbox_51";
			arrTb[51].Style.Add("width","40px");
			arrTb[51].Enabled = false;

			objHtc.Controls.Add(arrTb[51]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[52].ID = "textbox_52";
			arrTb[52].Style.Add("width","40px");
			arrTb[52].Enabled = false;

			objHtc.Controls.Add(arrTb[52]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			db = new Database();
			strSql = "SELECT question FROM questions WHERE id = " + arrQ[2];
			objDr = db.select(strSql);

			if(objDr.Read()) {
				objHtc.Controls.Add(new LiteralControl(objDr["question"].ToString() + " "));
			}

			db.objDataReader.Close();
			db = null;

			arrListBox[19].ID = "listbox_19";
			arrListBox[19].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[2] + " ORDER BY priority;";

			arrListBox[19].DataSource = db.select(strSql);
			arrListBox[19].DataTextField = "_option";
			arrListBox[19].DataValueField = "_value";
			arrListBox[19].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[19].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[19]);

			objHtr.Controls.Add(objHtc);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[0].ToString() + " "));

			arrTb[53].ID = "textbox_53";
			arrTb[53].Style.Add("width","40px");
            arrTb[53].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[53]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[4].ToString() + " "));

			arrTb[54].ID = "textbox_54";
			arrTb[54].Style.Add("width","40px");
			arrTb[54].Enabled = false;

			objHtc.Controls.Add(arrTb[54]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[5].ToString() + " "));

			arrTb[55].ID = "textbox_55";
			arrTb[55].Style.Add("width","40px");
			arrTb[55].Enabled = false;

			objHtc.Controls.Add(arrTb[55]);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#15B:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('15B');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[56].ID = "textbox_56";
			arrTb[56].Style.Add("width","40px");
            arrTb[56].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[56]);
			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[20].ID = "listbox_20";
			arrListBox[20].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[0] + " ORDER BY priority;";

			arrListBox[20].DataSource = db.select(strSql);
			arrListBox[20].DataTextField = "_option";
			arrListBox[20].DataValueField = "_value";
			arrListBox[20].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[20].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[20]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#16A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('16A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[57].ID = "textbox_57";
			arrTb[57].Style.Add("width","40px");
            arrTb[57].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[57]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#16B:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('16B');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[58].ID = "textbox_58";
			arrTb[58].Style.Add("width","40px");
            arrTb[58].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[58]);

			objHtc.Controls.Add(new LiteralControl("/"));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[59].ID = "textbox_59";
			arrTb[59].Style.Add("width","40px");
            arrTb[59].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[59]);

			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[21].ID = "listbox_21";
			arrListBox[21].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[3] + " ORDER BY priority;";

			arrListBox[21].DataSource = db.select(strSql);
			arrListBox[21].DataTextField = "_option";
			arrListBox[21].DataValueField = "_value";
			arrListBox[21].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[21].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[21]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#17A:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('17A');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[60].ID = "textbox_60";
			arrTb[60].Style.Add("width","40px");
            arrTb[60].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[60]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#17B:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('17B');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrTb[61].ID = "textbox_61";
			arrTb[61].Style.Add("width","40px");
            arrTb[61].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[61]);

			objHtc.Controls.Add(new LiteralControl("/"));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[62].ID = "textbox_62";
			arrTb[62].Style.Add("width","40px");
            arrTb[62].Attributes["onchange"] = "do_check_price_max_x(this, false, 100)";

			objHtc.Controls.Add(arrTb[62]);

			objHtc.Controls.Add(new LiteralControl(" "));

			arrListBox[22].ID = "listbox_22";
			arrListBox[22].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[3] + " ORDER BY priority;";

			arrListBox[22].DataSource = db.select(strSql);
			arrListBox[22].DataTextField = "_option";
			arrListBox[22].DataValueField = "_value";
			arrListBox[22].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[22].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[22]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#18:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('18');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			arrListBox[23].ID = "listbox_23";
			arrListBox[23].Rows = 1;
			
			db = new Database();
			strSql = "SELECT _value,_option FROM questionaire_options INNER JOIN question_option ON questionaire_options.id = question_option.optionid AND questionid = " + arrQ[4] + " ORDER BY priority;";

			arrListBox[23].DataSource = db.select(strSql);
			arrListBox[23].DataTextField = "_option";
			arrListBox[23].DataValueField = "_value";
			arrListBox[23].DataBind();

			db.objDataReader.Close();
			db = null;

			objLi = new ListItem();
			objLi.Text = strChooseText;
			objLi.Value = "0";
			objLi.Selected = true;
			arrListBox[23].Items.Insert(0,objLi);

			objHtc.Controls.Add(arrListBox[23]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[6].ToString() + " "));

			arrRb[2].ID = "radiobutton_2";
			arrRb[2].GroupName = "2_3";
			arrRb[2].Attributes["onclick"] = "disableObject(2,63);";
			objHtc.Controls.Add(arrRb[2]);
			
			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[7].ToString() + " "));

			arrTb[63].ID = "textbox_63";
			arrTb[63].Style.Add("width","40px");
            arrTb[63].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[63]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[8].ToString() + " "));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[64].ID = "textbox_64";
			arrTb[64].Style.Add("width","40px");
            arrTb[64].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[64]);

			objHtc.Controls.Add(new LiteralControl(" "));

			objHtc.Controls.Add(new LiteralControl("<br> " + arrInfos[9].ToString() + " "));

			arrRb[3].ID = "radiobutton_3";
			arrRb[3].GroupName = "2_3";
			arrRb[3].Attributes["onclick"] = "disableObject(3,63);";
			objHtc.Controls.Add(arrRb[3]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[7].ToString() + " "));

			arrTb[65].ID = "textbox_65";
			arrTb[65].Style.Add("width","40px");
            arrTb[65].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[65]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[8].ToString() + " "));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[66].ID = "textbox_66";
			arrTb[66].Style.Add("width","40px");
            arrTb[66].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[66]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#19:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('19');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[10].ToString() + " "));

			arrTb[67].ID = "textbox_67";
			arrTb[67].Style.Add("width","40px");
            arrTb[67].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[67]);
			
			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[11].ToString() + " "));

			arrTb[68].ID = "textbox_68";
			arrTb[68].Style.Add("width","40px");
            arrTb[68].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[68]);

			objHtc.Controls.Add(new LiteralControl(" " + arrInfos[12].ToString() + " "));

			objHtc.Attributes["class"] = "data_table_item";

			arrTb[69].ID = "textbox_69";
			arrTb[69].Style.Add("width","40px");
            arrTb[69].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[69]);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#20:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('20');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			objHtc.Controls.Add(new LiteralControl("- "));

			arrTb[70].ID = "textbox_70";
			arrTb[70].Style.Add("width","40px");
            arrTb[70].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[70]);
			
			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			//Række starter her

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			popup = new HtmlAnchor();
			popup.InnerHtml = "#21:";
			popup.HRef = "javascript:void(0);";
			popup.Attributes["onclick"] = "popup('21');";
			objHtc.Controls.Add(popup);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_item";

			objHtc.Controls.Add(new LiteralControl("+ "));

			arrTb[71].ID = "textbox_71";
			arrTb[71].Style.Add("width","40px");
            arrTb[71].Attributes["onchange"] = "do_check_price_max(this, false, 100)";

			objHtc.Controls.Add(arrTb[71]);
			
			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			//Række slutter her

			this.Controls.Add(objHt);

			this.Controls.Add(new LiteralControl("<p>" + arrInfos[13].ToString() + "</p>"));

			objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.ID = "buttons";
			objHt.Border = 0;
			objHt.Style.Add("width","475px");
			objHt.Style.Add("height","20px");

			objHtr = new HtmlTableRow();
			
			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","50px");
			objHtc.Style.Add("height","20px");
			
			back.ID = "back";
			back.Attributes["class"] = "page_admin_btn";
			back.Style.Add("width","50px");
			back.InnerHtml = "<<";
			back.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=5";
			objHtc.Controls.Add(back);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","375px");
			objHtc.Style.Add("text-align","center");
			
			Button submit = new Button();
			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Text = strSubmitText;
			submit.Click += new EventHandler(saveStep6);
			objHtc.Controls.Add(submit);

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","50px");
			objHtc.Style.Add("text-align","right");
			
			forward.ID = "forward";
			forward.Attributes["class"] = "page_admin_btn";
			forward.Style.Add("width","50px");
			forward.InnerHtml = ">>";
			if(intStepSaved < 6) {
				forward.Visible = false;
			}
			forward.HRef = "../../?page=" + IntPageId + "&submenu="+ IntSubmenuId +"&mode=" + strMode + "&id=" + intId + "&step=7";

			objHtc.Controls.Add(forward);

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			this.Controls.Add(objHt);

			if(intStepSaved > 5) {
				db  = new Database();

				strSql = "SELECT 6_1";

				for(int i = 2;i<101;i++) {
					strSql += ", 6_" + i;
				}
				strSql += " FROM temp_clients WHERE ";

				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;

				objDr = db.select(strSql);
			
				if(objDr.Read()) {
					for(int i = 0;i < 72;i++) {
                        //if(Convert.ToDouble(objDr["6_"+(i+1)]) != 99.0) {
                        //    arrTb[i].Text = objDr["6_"+(i+1)].ToString();
                        //}
                        //else {
                        //arrTb[i].Text = objDr["6_" + (i + 1)].ToString();
                        if (objDr["6_" + (i + 1)].ToString() == "-1" || objDr["6_" + (i + 1)].ToString() == "-1.00")
                        {
                            arrTb[i].Text = "";
                        }
                        else {
                            arrTb[i].Text = objDr["6_" + (i + 1)].ToString();
                        }
						}
                    //}

					for(int i = 0;i < 24;i++) {
						arrListBox[i].SelectedValue = objDr["6_"+(i+73)].ToString();
					}

					for(int i = 0;i < 4;i++) { 
						if(Convert.ToInt32(objDr["6_"+(i+97)]) == 1) {
							arrRb[i].Checked = true;
						}
					}
				}

				db.objDataReader.Close();
				db = null;

			}
		}
		private void drawAddPageStep7() {
			Database db = new Database();

			strSql = "SELECT 6_1";

			for(int i = 2;i<101;i++) {
				strSql += ", 6_" + i;
			}
			strSql += " FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			MySqlDataReader objDr = db.select(strSql);

			Hashtable htValue = new Hashtable();

			if(objDr.Read()) {
				for(int i = 1;i < 101;i++) {
					htValue.Add("6_"+i,objDr["6_"+i]);
				}
			}

			db.objDataReader.Close();
			db = null;

			Hashtable htId = new Hashtable();
			
			string strValue = "";


			//#3
			//Over ½ exo = høj
			//½ exo = neutral
			//Under ½ exo eller eso = lav

			//1 = eso, 2 = 0, 3 = exo
			if(Convert.ToDouble(htValue["6_1"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_73"]) == 1) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_73"]) == 2) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_73"]) == 3) {
					if(Convert.ToDouble(htValue["6_1"]) == 0.5) {
						strValue = "N";
					}
					else if(Convert.ToDouble(htValue["6_1"]) > 0.5) {
						strValue = "H";
					}
					else {
						strValue = "L";
					}
				}
			}
			htId.Add("3",strValue);

			strValue = "";

			//#13A
			//Over 6 exo = høj
			//6 exo = neutral
			//Under 6 exo eller eso= lav
			
			//1 = eso, 2 = 0, 3 = exo
			if(Convert.ToDouble(htValue["6_2"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_74"]) == 1) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_74"]) == 2) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_74"]) == 3) {
					if(Convert.ToDouble(htValue["6_2"]) == 6) {
						strValue = "N";
					}
					else if(Convert.ToDouble(htValue["6_2"]) > 6) {
						strValue = "H";
					}
					else {
						strValue = "L";
					}
				}
			}
			htId.Add("13A",strValue);

			strValue = "";

			//#4
			//Mere plus eller mindre minus end # 7 = høj
			//Samme plus som #7 = neutral
			//Mindre plus eller mere minus end #7 = Lav

			//1 = +, 2 = 0, 3 = -

			//H
			if(Convert.ToDouble(htValue["6_3"]) != 99.0 && Convert.ToDouble(htValue["6_17"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_75"]) == 1) {
					if(Convert.ToInt32(htValue["6_79"]) == 1) {
						if(Convert.ToDouble(htValue["6_3"]) > Convert.ToDouble(htValue["6_17"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_3"]) == Convert.ToDouble(htValue["6_17"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_3"]) < Convert.ToDouble(htValue["6_17"])) {
							strValue = "L";
						}
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 2) {
						strValue = "H";
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 3) {
						strValue = "H";
					}
				}
				else if (Convert.ToInt32(htValue["6_75"]) == 2) {
					if(Convert.ToInt32(htValue["6_79"]) == 1) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 2) {
						if(Convert.ToDouble(htValue["6_3"]) > Convert.ToDouble(htValue["6_17"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_3"]) == Convert.ToDouble(htValue["6_17"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_3"]) < Convert.ToDouble(htValue["6_17"])) {
							strValue = "L";
						}
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 3) {
						strValue = "H";
					}
				}
				else if (Convert.ToInt32(htValue["6_75"]) == 3) {
					if(Convert.ToInt32(htValue["6_79"]) == 1) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 2) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_79"]) == 3) {
						if(Convert.ToDouble(htValue["6_3"]) < Convert.ToDouble(htValue["6_17"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_3"]) == Convert.ToDouble(htValue["6_17"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_3"]) > Convert.ToDouble(htValue["6_17"])) {
							strValue = "L";
						}
					}
				}
			}
			htId.Add("4h",strValue);

			strValue = "";

			//V
			if(Convert.ToDouble(htValue["6_7"]) != 99.0 && Convert.ToDouble(htValue["6_21"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_76"]) == 1) {
					if(Convert.ToInt32(htValue["6_80"]) == 1) {
						if(Convert.ToDouble(htValue["6_7"]) > Convert.ToDouble(htValue["6_21"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_7"]) == Convert.ToDouble(htValue["6_21"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_7"]) < Convert.ToDouble(htValue["6_21"])) {
							strValue = "L";
						}
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 2) {
						strValue = "H";
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 3) {
						strValue = "H";
					}
				}
				else if (Convert.ToInt32(htValue["6_76"]) == 2) {
					if(Convert.ToInt32(htValue["6_80"]) == 1) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 2) {
						if(Convert.ToDouble(htValue["6_7"]) > Convert.ToDouble(htValue["6_21"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_7"]) == Convert.ToDouble(htValue["6_21"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_7"]) < Convert.ToDouble(htValue["6_21"])) {
							strValue = "L";
						}
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 3) {
						strValue = "H";
					}
				}
				else if (Convert.ToInt32(htValue["6_76"]) == 3) {
					if(Convert.ToInt32(htValue["6_80"]) == 1) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 2) {
						strValue = "L";
					}
					else if (Convert.ToInt32(htValue["6_80"]) == 3) {
						if(Convert.ToDouble(htValue["6_7"]) < Convert.ToDouble(htValue["6_21"])) {
							strValue = "H";
						}
						else if(Convert.ToDouble(htValue["6_7"]) == Convert.ToDouble(htValue["6_21"])) {
							strValue = "N";
						}
						else if(Convert.ToDouble(htValue["6_7"]) > Convert.ToDouble(htValue["6_21"])) {
							strValue = "L";
						}
					}
				}
			}
			htId.Add("4v",strValue);

			strValue = "";
			
			//#5h
			//Netto er mere plus/mindre minus end #4 = høj
			//Netto er samme som #4 = neutral
			//Netto er mindre plus/mere minus end #4 = Lav
			double dbl4Value;
			
			if(Convert.ToDouble(htValue["6_3"]) != 99.0 && Convert.ToDouble(htValue["6_13"]) != 99.0) {
				dbl4Value = Convert.ToDouble(htValue["6_3"]);
				if(Convert.ToInt32(htValue["6_75"]) == 3) {
					dbl4Value = dbl4Value * -1.00;
				}

				if(Convert.ToDouble(htValue["6_13"]) > dbl4Value) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_13"]) == dbl4Value) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_13"]) < dbl4Value) {
					strValue = "L";
				}
			}
			htId.Add("5h",strValue);
			
			//#5v
			//Netto er mere plus/mindre minus end #4 = høj
			//Netto er samme som #4 = neutral
			//Netto er mindre plus/mere minus end #4 = Lav

			if(Convert.ToDouble(htValue["6_7"]) != 99.0 && Convert.ToDouble(htValue["6_16"]) != 99.0) {
				dbl4Value = 0.00;

				dbl4Value = Convert.ToDouble(htValue["6_7"]);
				if(Convert.ToInt32(htValue["6_76"]) == 3) {
					dbl4Value = dbl4Value * -1.00;
				}

				if(Convert.ToDouble(htValue["6_16"]) > dbl4Value) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_16"]) == dbl4Value) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_16"]) < dbl4Value) {
					strValue = "L";
				}
			}
			htId.Add("5v",strValue);

			strValue = "";

			//#8
			//Over ½ exo = høj
			//½ exo = neutral
			//Under ½ exo eller eso = lav

			//1 = eso, 2 = 0, 3 = exo
			if(Convert.ToDouble(htValue["6_33"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_83"]) == 1) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_83"]) == 2) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_83"]) == 3) {
					if(Convert.ToDouble(htValue["6_33"]) == 0.5) {
						strValue = "N";
					}
					else if(Convert.ToDouble(htValue["6_33"]) > 0.5) {
						strValue = "H";
					}
					else {
						strValue = "L";
					}
				}
			}
			htId.Add("8",strValue);

			strValue = "";

			//#9
			//Hvis der i stedet for et tal står et kryds, er krydset lig det første tal i #10
			//Over 9  = Høj
			//Fra 7-9 = Neutral (begge tal incl)
			//Under 7 = Lav
            strValue = Convert.ToString(htValue["6_34"]);

            if (Convert.ToString(htValue["6_34"]) == "x" || Convert.ToString(htValue["6_34"]) == "X")
            {
                strValue = "N";
            }
            else
            {
                if (Convert.ToDouble(htValue["6_34"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_34"]) > 9.00)
                    {
                        strValue = "H";
                    }
                    else if (Convert.ToDouble(htValue["6_34"]) < 7.00)
                    {
                        strValue = "L";
                    }
                    else
                    {
                        strValue = "N";
                    }
                }
            }
			htId.Add("9",strValue);

			strValue = "";


			//#10
			//Hvis break (første tal) er større end 19 og recovery (andet tal ) er større end ½ af break = Høj
			//Hvis enten break er mindre end 19 eller recovery er mindre end ½ af break = Lav
			//Denne måling skal bedømmes sammen med #11 – se nedenfor
            if (Convert.ToString(htValue["6_35"]) == "x" || Convert.ToString(htValue["6_35"]) == "X" || Convert.ToString(htValue["6_36"]) == "x" || Convert.ToString(htValue["6_36"]) == "X")
            {
                strValue = "L";
            }
            else
            {

                if (Convert.ToDouble(htValue["6_19"]) != 99.0 && Convert.ToDouble(htValue["6_35"]) != 99.0 && Convert.ToDouble(htValue["6_36"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_35"]) > 19.00 && Convert.ToDouble(htValue["6_36"]) > (Convert.ToDouble(htValue["6_35"]) / 2.00))
                    {
                        strValue = "H";
                    }
                    else if (Convert.ToDouble(htValue["6_35"]) < 19.00)
                    {
                        strValue = "L";
                    }
                    else if (Convert.ToDouble(htValue["6_36"]) < (Convert.ToDouble(htValue["6_35"]) / 2.00))
                    {
                        strValue = "L";
                    }
                }
            }
			htId.Add("10",strValue);

			strValue = "";

			//#11
			//Hvis break (første tal) er større end 19 og recovery (andet tal ) er større end ½ af break = Høj
			//Hvis enten break er mindre end 19 eller recovery er mindre end ½ af break = Lav
			//Denne måling skal bedømmes sammen med #11 – se nedenfor
            if (htValue["6_37"].ToString().ToLower() == "x" || htValue["6_35"].ToString().ToLower() == "x" || htValue["6_38"].ToString().ToLower() == "x" || htValue["6_36"].ToString().ToLower() == "x")
            {
            }
            else
            {

                if (Convert.ToDouble(htValue["6_37"]) != 99.0 && Convert.ToDouble(htValue["6_35"]) != 99.0 && Convert.ToDouble(htValue["6_38"]) != 99.0 && Convert.ToDouble(htValue["6_36"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_37"]) > 19.00 && Convert.ToDouble(htValue["6_38"]) > (Convert.ToDouble(htValue["6_37"]) / 2.00))
                    {
                        strValue = "H";

                        if (htId["10"].ToString() == "L")
                        {
                            htId["10"] = "LL";
                        }
                    }
                    else if (Convert.ToDouble(htValue["6_37"]) < 19.00)
                    {
                        if (htId["10"].ToString() == "H")
                        {
                            strValue = "LL";
                        }
                        else
                        {
                            if (Convert.ToDouble(htValue["6_35"]) / 19.00 < Convert.ToDouble(htValue["6_37"]) / 9.00)
                            {
                                htId["10"] = "LL";
                            }
                            else
                            {
                                strValue = "LL";
                            }
                        }
                    }
                    else if (Convert.ToDouble(htValue["6_38"]) < (Convert.ToDouble(htValue["6_37"]) / 2.00))
                    {
                        strValue = "L";

                        if (htId["10"].ToString() == "H")
                        {
                            strValue = "LL";
                        }
                        else
                        {
                            if (Convert.ToDouble(htValue["6_36"]) / 10.00 < Convert.ToDouble(htValue["6_38"]) / 5.00)
                            {
                                htId["10"] = "LL";
                            }
                            else
                            {
                                strValue = "LL";
                            }
                        }
                    }
                }
            }
			htId.Add("11",strValue);

			strValue = "";


			//13B
			//Over 6 exo = høj
			//6 exo = neutral
			//Under 6 exo eller eso= lav
			
			//1 = eso, 2 = 0, 3 = exo
			if(Convert.ToDouble(htValue["6_43"]) != 99.0) {
				if(Convert.ToInt32(htValue["6_87"]) == 1) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_87"]) == 2) {
					strValue = "L";
				}
				else if(Convert.ToInt32(htValue["6_87"]) == 3) {
					if(Convert.ToDouble(htValue["6_43"]) == 6) {
						strValue = "N";
					}
					else if(Convert.ToDouble(htValue["6_43"]) > 6) {
						strValue = "H";
					}
					else {
						strValue = "L";
					}
				}
			}
			htId.Add("13B",strValue);

			strValue = " ";


			//#14Ah
			//Netto er mere plus/mindre minus end #7 = høj
			//Netto er samme som #7 = neutral
			//Netto er mindre plus/mere minus end #7 = Lav
			if(Convert.ToDouble(htValue["6_17"]) != 99.0 && Convert.ToDouble(htValue["6_46"]) != 99.0) {
				dbl4Value = 0.00;

				dbl4Value = Convert.ToDouble(htValue["6_17"]);
				if(Convert.ToInt32(htValue["6_79"]) == 3) {
					dbl4Value = dbl4Value * -1.00;
				}

				if(Convert.ToDouble(htValue["6_46"]) > dbl4Value) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_46"]) == dbl4Value) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_46"]) < dbl4Value) {
					strValue = "L";
				}
			}
			htId.Add("14Ah",strValue);
			
			//#14Av
			//Netto er mere plus/mindre minus end #7 = høj
			//Netto er samme som #7 = neutral
			//Netto er mindre plus/mere minus end #7 = Lav
			if(Convert.ToDouble(htValue["6_21"]) != 99.0 && Convert.ToDouble(htValue["6_49"]) != 99.0) {
				dbl4Value = 0.00;

				dbl4Value = Convert.ToDouble(htValue["6_21"]);
				if(Convert.ToInt32(htValue["6_80"]) == 3) {
					dbl4Value = dbl4Value * -1.00;
				}

				if(Convert.ToDouble(htValue["6_49"]) > dbl4Value) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_49"]) == dbl4Value) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_49"]) < dbl4Value) {
					strValue = "L";
				}
			}
			htId.Add("14Av",strValue);

			strValue = "";

			//!Gammel udregning start!
			//15A
			//Over ½ exo = høj
			//½ exo = neutral
			//Under ½ exo eller eso = lav

			//1 = eso, 2 = 0, 3 = exo
			
			//if(Convert.ToInt32(htValue["6_90"]) == 1)
			//{
			//strValue = "L";
			//}
			//else if(Convert.ToInt32(htValue["6_90"]) == 2)
			//{
			//strValue = "L";
			//}
			//else if(Convert.ToInt32(htValue["6_90"]) == 3)
			//{
			//if(Convert.ToDouble(htValue["6_50"]) == 0.5)
			//{
			//strValue = "N";
			//}
			//else if(Convert.ToDouble(htValue["6_50"]) > 0.5)
			//{
			//strValue = "H";
			//}
			//else
			//{
			//strValue = "L";
			//}
			//}
			//!Gammel udregning slut!

			//15A	
			//Bedømmes i forhold til #14A
			//Hvis #14A er høj bliver #15 A automatisk lav
			//Hvis # 14A er lav bliver #15A automatisk høj
			//Hvis # 14 A er neutral og #15A er:
			//over 6 exo= Både #14A og#15A er høje
			//= 6 exo = #14A og #15A er neutrale
			//under 6 exo eller eso = #14A og #15A er lave
			if(Convert.ToDouble(htValue["6_50"]) != 99.0) {
				if(htId["14Ah"].ToString() == "H") {
					strValue = "L";
				}
				else if(htId["14Ah"].ToString() == "L") {
					strValue = "H";
				}			
				else if(htId["14Ah"].ToString() == "N") {
					if(Convert.ToInt32(htValue["6_90"]) == 3) {
						if(Convert.ToDouble(htValue["6_50"]) > 6.00) {
							strValue = "H";
							htId["14Ah"] = "H";
						}
						else if(Convert.ToDouble(htValue["6_50"]) == 6.00) {
							strValue = "N";
							htId["14Ah"] = "N";
						}
						else {
							strValue = "L";
							htId["14Ah"] = "L";
						}
					}					
					else {
						strValue = "L";
						htId["14Ah"] = "L";
					}
				}
			}
			htId.Add("15A",strValue);

			strValue = "";

			//#16A
			//over 15 = høj
			//= 15 = neutral
			//under 15 = lav
            if (htValue["6_58"].ToString().ToLower() == "x")
            {
                strValue = "L";
            }
            else
            {
                if (Convert.ToDouble(htValue["6_58"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_58"]) > 15.00)
                    {
                        strValue = "H";
                    }
                    else if (Convert.ToDouble(htValue["6_58"]) == 15.00)
                    {
                        strValue = "N";
                    }
                    else if (Convert.ToDouble(htValue["6_58"]) < 15.00)
                    {
                        strValue = "L";
                    }
                }
            }
			htId.Add("16A",strValue);

			strValue = "";


			//#16B
			//Hvis break (første tal) er større end 21 og recovery (andet tal) er større end 2/3 af break = Høj
			//Hvis enten break er mindre end 21 eller recovery er mindre end 2/3 af break = Lav
            if (htValue["6_59"].ToString().ToLower() == "x" || htValue["6_60"].ToString().ToLower() == "x")
            {
                strValue = "L";
            }
            else
            {
                if (Convert.ToDouble(htValue["6_59"]) != 99.0 && Convert.ToDouble(htValue["6_60"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_59"]) > 21.00 && Convert.ToDouble(htValue["6_60"]) > (Convert.ToDouble(htValue["6_59"]) * 2 / 3))
                    {
                        strValue = "H";
                    }
                    else
                    {
                        strValue = "L";
                    }
                }
            }
			htId.Add("16B",strValue);

			strValue = "";


			//#17A
			//over 14 = høj
			//=14 = neutral
			//under 14 = lav
			//Denne måling skal bedømmes sammen med #17B – se nedenfor

            if (htValue["6_61"].ToString().ToLower() == "x")
            {
                strValue = "L";
            }
            else
            {
                if (Convert.ToDouble(htValue["6_61"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_61"]) > 14.00)
                    {
                        strValue = "H";
                    }
                    else if (Convert.ToDouble(htValue["6_61"]) == 14.00)
                    {
                        strValue = "N";
                    }
                    else if (Convert.ToDouble(htValue["6_61"]) < 14.00)
                    {
                        strValue = "L";
                    }
                }
            }
			htId.Add("17A",strValue);

			strValue = "";

			//#17B
			//Hvis break er større end 22 og recovery større end 3/4 af break = høj
			//Hvis enten break er mindre end 22 eller recovery er mindre end3/4 af break = Lav.


			//if(Convert.ToDouble(htValue["6_62"]) > 22.00 && Convert.ToDouble(htValue["6_63"]) > (Convert.ToDouble(htValue["6_62"])*3 / 4))
			//{
			//strValue = "H";
			//}
			//else
			//{
			//strValue = "L";
			//}

			//htId.Add("17B",strValue);

			//strValue = "";


			//#16B/#17B
			//Hvis begge målinger har lavt break beregnes for hver: Målt break delt med normtallet. Laveste brøk = Lowlow.
			//Hvis begge målinger er dømt lave pga. recovery beregnes for hver: Målt recovery delt med normtallet. Laveste brøk = Lowlow.
			//Hvis begge målinger er bedømt høje, er der ingen Lowlow. Hvis den ene er bedømt høj, er den anden automatisk Lowlow.
            if (htValue["6_59"].ToString().ToLower() == "x" || htValue["6_62"].ToString().ToLower() == "x" || htValue["6_63"].ToString().ToLower() == "x" || htValue["6_60"].ToString().ToLower() == "x")
            {
                strValue = "L";
            }
            else
            {
                if (Convert.ToDouble(htValue["6_59"]) != 99.0 && Convert.ToDouble(htValue["6_62"]) != 99.0 && Convert.ToDouble(htValue["6_63"]) != 99.0 && Convert.ToDouble(htValue["6_60"]) != 99.0)
                {
                    if (Convert.ToDouble(htValue["6_62"]) > 22.00 && Convert.ToDouble(htValue["6_63"]) > (Convert.ToDouble(htValue["6_62"]) * 3 / 4))
                    {
                        strValue = "H";

                        if (htId["16B"].ToString() == "L")
                        {
                            htId["16B"] = "LL";
                        }
                    }
                    else if (Convert.ToDouble(htValue["6_62"]) < 22.00)
                    {
                        if (htId["16B"].ToString() == "H")
                        {
                            strValue = "LL";
                        }
                        else
                        {
                            if (Convert.ToDouble(htValue["6_59"]) / 21.00 < Convert.ToDouble(htValue["6_62"]) / 22.00)
                            {
                                htId["16B"] = "LL";
                            }
                            else
                            {
                                strValue = "LL";
                            }
                        }
                    }
                    else if (Convert.ToDouble(htValue["6_63"]) < (Convert.ToDouble(htValue["6_62"]) * 3 / 4))
                    {
                        strValue = "L";

                        if (htId["16B"].ToString() == "H")
                        {
                            strValue = "LL";
                        }
                        else
                        {
                            if (Convert.ToDouble(htValue["6_60"]) / 15.00 < Convert.ToDouble(htValue["6_63"]) / 18.00)
                            {
                                htId["16B"] = "LL";
                            }
                            else
                            {
                                strValue = "LL";
                            }
                        }
                    }
                }
            }
			htId.Add("17B",strValue);

			strValue = "";


			//19
			//Normtallet beregnes ved: 13 – ¼ ganget med klientens alder – fås fra udfyldelse af journalkort, hvor 	kode bliver tastet ind.
			//Over normtal = høj
			//= normtal = neutral
			//Under normtal = lav

			double dblNorm = 0.00;
			if(Convert.ToDouble(htValue["6_70"]) != 99.0) {
				db = new Database();
				if(intIsFirst == 1) {
					strSql = "SELECT birthdate FROM temp_clients WHERE passwordid = " + intId;
				}
				else if(intIsFirst == 0) {
					strSql = "SELECT birthdate FROM user_client WHERE userid = " + intId;
				}

				int intAge = ((Optician)Session["user"]).getAge(Convert.ToDateTime(db.scalar(strSql)));

				dblNorm = 13 - (intAge / 4);

				if(Convert.ToDouble(htValue["6_70"]) > dblNorm) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_70"]) == dblNorm) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_70"]) < dblNorm) {
					strValue = "L";
				}
			}
			htId.Add("19",strValue);

			strValue = "";

			//#20
			//Mere minus end –2.37 = høj
			//= -2.37 = neutral
			//Mindre minus end –2.37 = lav
			if(Convert.ToDouble(htValue["6_71"]) != 99.0) {
				if(Convert.ToDouble(htValue["6_71"]) > -2.37) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_71"]) == -2.37) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_71"]) > -2.37) {
					strValue = "L";
				}
			}
			htId.Add("20",strValue);

			strValue = "";

			//#21
			//Mere plus end +1.87 = høj
			//+1,5 = neutral
			//Mindre plus end +1.87 = lav
			if(Convert.ToDouble(htValue["6_72"]) != 99.0) {
				if(Convert.ToDouble(htValue["6_72"]) > 1.87) {
					strValue = "H";
				}
				else if(Convert.ToDouble(htValue["6_72"]) == 1.87) {
					strValue = "N";
				}
				else if(Convert.ToDouble(htValue["6_72"]) > 1.87) {
					strValue = "L";
				}
			}
			htId.Add("21",strValue);

			strValue = "";

	
			//Vi gemmer alle de dejlige informationer i databasen

			DateTime tmpAddedTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
			int intUserId = 0;

			db = new Database();

			//Henter brugerens informationer fra temp_clients, og gemmer dem i users og user_client.

			if(intIsFirst == 1) {
				strSql = "SELECT temp_clients.opticianid,temp_clients.name,address,zipcode,city,birthdate,phone,fax,email,access_www,password FROM temp_clients ";
				strSql += "INNER JOIN optician_keys ON passwordid = optician_keys.id WHERE passwordid = " + intId;
			
				objDr = db.select(strSql);

				if(objDr.Read()) {
					Database db1 = new Database();
					string strSql1 = "INSERT INTO users(password,addedtime,address,zipcode,city,email,phone,fax,isactive,usertypeid) VALUES('";
					strSql1 += objDr["password"].ToString() + "','" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "','" + objDr["address"].ToString().Replace("'", "''") + "','" + objDr["zipcode"].ToString() + "',";
					strSql1 += "'" + objDr["city"].ToString() + "','" + objDr["email"].ToString() + "','" + objDr["phone"].ToString() + "','" + objDr["fax"].ToString() + "',";
					strSql1 += 1 + ",3);";
					db1.execSql(strSql1);

					strSql1 = "SELECT id FROM users WHERE password = '" + objDr["password"].ToString() + "';";
					intUserId = Convert.ToInt32(db1.scalar(strSql1));
				
					string strFullname = objDr["name"].ToString();

					strSql = "INSERT INTO user_client(userid,opticianid,firstname,lastname,birthdate,enddate,languageid,access_www) VALUES(";
					strSql += intUserId + "," + Convert.ToInt32(objDr["opticianid"]) + ",'" + (((Optician)Session["user"]).getFirstName(strFullname)).Replace("'", "''") + "','";
					strSql += (((Optician)Session["user"]).getLastName(strFullname)).Replace("'", "''") + "','";
					strSql += Convert.ToDateTime(objDr["birthdate"]).ToString("yyyy-MM-dd") + "','";
					strSql += tmpAddedTime.AddMonths(6).ToString("yyyy-MM-dd HH:mm") + "'," + ((Optician)Session["user"]).IntLanguageId + "," + Convert.ToInt32(objDr["access_www"]) + ");";
					db1.execSql(strSql);

					db1 = null;

					if(objDr["email"].ToString() != ""){
						string[] arrMailBody = new string[5];
						string[] arrMailHeader = new string[] {"","Du er nu oprettet som bruger","Du er nu oprettet som bruger","Your user profile has been created","TrainYourEyes.com hat Sie nun ins System aufgenommen"};

						arrMailBody[0] = "";
						arrMailBody[1] = "Kære [navn],\n\nTrainYourEyes.com har hermed oprettet dig i vores system som du kan finde på http://www.trainyoureyes.com.\n\nDu er blevet oprettet med følgende oplysninger:\nKodeord: [kodeord]\nNavn: [navn]\nAdresse: [adresse]\nPost nr. & By: [postnr] [by]\nTelefon: [telefon]\nFax: [fax]\nEmail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så bedes du kontakte din optiker for at få de korrekte oplysninger registreret.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\n[optikernavn]";
						arrMailBody[2] = "!Kære [navn],\n\nTrainYourEyes.com har hermed oprettet dig i vores system som du kan finde på http://www.trainyoureyes.com.\n\nDu er blevet oprettet med følgende oplysninger:\nKodeord: [kodeord]\nNavn: [navn]\nAdresse: [adresse]\nPost nr. & By: [postnr] [by]\nTelefon: [telefon]\nFax: [fax]\nEmail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så bedes du kontakte din optiker for at få de korrekte oplysninger registreret.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\n[optikernavn]";
						arrMailBody[3] = "/Kære [navn],\n\nTrainYourEyes.com har hermed oprettet dig i vores system som du kan finde på http://www.trainyoureyes.com.\n\nDu er blevet oprettet med følgende oplysninger:\nKodeord: [kodeord]\nNavn: [navn]\nAdresse: [adresse]\nPost nr. & By: [postnr] [by]\nTelefon: [telefon]\nFax: [fax]\nEmail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så bedes du kontakte din optiker for at få de korrekte oplysninger registreret.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\n[optikernavn]";
						arrMailBody[4] = "Sehr geehrte Frau / sehr geehrter Herr [navn],\n\nTrainYourEyes.com hat Sie nun ins System aufgenommen. Sie finden es unter http://www.trainyoureyes.com.\n\n Sie wurden mit folgenden Informationen registriert:\nPasswort: [kodeord]\nName: [navn]\nAdresse: [adresse]\nPostleitzahl und Ort: [postnr] [by]\nTelefon: [telefon]\nFax: [fax]\nE-mail: [email]\n\nBitte überprüfen Sie diese Informationen auf ihre Richtigkeit. Falls sie fehlerhaft sein sollten, wenden Sie sich bitte an Ihren Optiker bzw. Augenarzt.\n\nUnter http://www.trainyoureyes.com können Sie Antworten auf die am häufigsten gestellten Fragen (FAQ) finden, mehr über TrainYourEyes erfahren, TrainYourEyes kontaktieren und vieles mehr.\n\nMit besten Grüßen\n\n[optikernavn]";

						MailMessage objMail = new MailMessage();
						objMail.To = objDr["email"].ToString();
						objMail.Subject = arrMailHeader[((Optician)Session["user"]).IntLanguageId];
						objMail.Body = arrMailBody[((Optician)Session["user"]).IntLanguageId].Replace("[navn]",strFullname).Replace("[kodeord]",objDr["password"].ToString()).Replace("[adresse]",objDr["address"].ToString()).Replace("[postnr]",objDr["zipcode"].ToString()).Replace("[by]",objDr["city"].ToString()).Replace("[telefon]",objDr["phone"].ToString()).Replace("[email]",objDr["email"].ToString()).Replace("[fax]",objDr["fax"].ToString()).Replace("[optikernavn]",((Optician)Session["user"]).StrName);
						objMail.From = "noreply@trainyoureyes.com";
						objMail.BodyFormat = MailFormat.Text;
						//SmtpMail.SmtpServer = "128.9.205.5";
                        SmtpMail.SmtpServer = "localhost";

						SmtpMail.Send(objMail);
					
						objMail = null;
					}
				}

				db.objDataReader.Close();
			}
			else {
				intUserId = intId;
			}
			//Her hentes og gemmes anamnesen

			strSql = "SELECT 2_1";

			for(int i = 2;i < 21;i++) {
				strSql += ",2_" + i;
			}

			strSql += " FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				Database db1 = new Database();

				string strSql1 = "INSERT INTO a_anamnese(";

				for(int i = 1;i < 21;i++) {
					strSql1 += "a_" + i + ",";
				}

				strSql1 += "addedtime,clientid,isfirst) VALUES(";

				for(int i = 1;i < 21;i++) {
					if(i > 18) {
						strSql1 += objDr["2_" + i].ToString().Replace(",",".") + ",";
					}
					else {
						strSql1 += objDr["2_" + i] + ",";
					}
				}

				strSql1 += "'" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "'," + intUserId + "," + intIsFirst + ");";
				
				db1.execSql(strSql1);
			
				//Response.Write(strSql1);
			}

			db.objDataReader.Close();

			//Her hentes og gemmes konvergensnærpunkt

			strSql = "SELECT 3_1";

			for(int i = 2;i < 4;i++) {
				strSql += ",3_" + i;
			}

			strSql += " FROM temp_clients WHERE ";
			
			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				Database db1 = new Database();

				string strSql1 = "INSERT INTO a_convergence(";

				for(int i = 1;i < 4;i++) {
					strSql1 += "c_" + i + ",";
				}

				strSql1 += "addedtime,clientid,isfirst) VALUES(";

				for(int i = 1;i < 4;i++) {
					strSql1 += objDr["3_" + i] + ",";
				}

				strSql1 += "'" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "'," + intUserId + "," + intIsFirst + ");";
				
				db1.execSql(strSql1);
			}

			db.objDataReader.Close();

			//Her hentes og gemmes motilitet

			strSql = "SELECT 4_1,4_1_1";

			for(int i = 2;i < 4;i++) {
				strSql += ",4_" + i + ",4_" + i + "_1";
			}

			strSql += " FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				Database db1 = new Database();

				string strSql1 = "INSERT INTO a_motilitet(";

				for(int i = 1;i < 4;i++) {
					strSql1 += "m_" + i + ",m_" + i + "_1,";
				}

				strSql1 += "addedtime,clientid,isfirst) VALUES(";

				for(int i = 1;i < 4;i++) {
					strSql1 += objDr["4_" + i] + ",'" + objDr["4_" + i + "_1"] + "',";
				}

				strSql1 += "'" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "'," + intUserId + "," + intIsFirst + ");";
				
				db1.execSql(strSql1);
			}

			db.objDataReader.Close();

			//Her gemmes 21 (1/2)

			strSql = "SELECT 5_1 ";

			for(int i = 2;i < 20;i++) {
				strSql += ",5_" + i;
			}

			strSql += " FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				Database db1 = new Database();

				string strSql1 = "INSERT INTO a_21(";

				for(int i = 1;i < 4;i++) {
					strSql1 += "1_" + i + ",";
				}

				for(int i = 4;i < 9;i++) {
					strSql1 += "1_" + i + ",";
				}

				for(int i = 9;i < 20;i++) {
					strSql1 += "1_" + i + ",";
				}

				strSql1 += "addedtime,clientid,isfirst) VALUES('";

				for(int i = 1;i < 20;i++) {
					strSql1 += objDr["5_" + i].ToString().Replace(",",".").Replace("'","''") + "','";
				}

				strSql1 += tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "'," + intUserId + "," + intIsFirst + ");";

				db1.execSql(strSql1);

			}

			db.objDataReader.Close();


			//Her hentes og gemmes 21 (2/2)

			strSql = "SELECT 6_1 ";

			for(int i = 2;i < 101;i++) {
				strSql += ",6_" + i;
			}

			strSql += " FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;

			objDr = db.select(strSql);

			if(objDr.Read()) {
				Database db1 = new Database();

				string strSql1 = "UPDATE a_21 SET ";

				for(int i = 1;i < 101;i++) {
					strSql1 += "2_" + i + " = '" + objDr["6_" + i] + "',";
				}
	
				strSql1 += "addedtime = '" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "' WHERE clientid = " + intUserId + " AND isfirst = " + intIsFirst + ";";

				db1.execSql(strSql1);
			}

			db.objDataReader.Close();

			//Her gemmes htValue			

			string[] arrValues = new string[htId.Count];
			string[] arrKeys = new string[htId.Count];

			htId.Values.CopyTo(arrValues,0);
			htId.Keys.CopyTo(arrKeys,0);

			strSql = "UPDATE a_21 SET ";

			for(int i = 0;i <= arrValues.GetUpperBound(0);i++) {
				strSql += "3_" + arrKeys[i].ToString() + " = '" + arrValues[i].ToString() + "',";
			}

			strSql += "addedtime = '" + tmpAddedTime.ToString("yyyy-MM-dd HH:mm") + "' WHERE clientid = " + intUserId + " AND isfirst = " + intIsFirst + ";";

			db.execSql(strSql);

			//Log
			if(intIsFirst == 1) {
				LogAnalysis la = new LogAnalysis(((Optician)Session["user"]).IntUserId,intUserId,intUserId,Request.UserHostAddress.ToString(),"startid","log_teststart");
				la = null;
			}
			else if(intIsFirst == 0) {
				LogAnalysis la = new LogAnalysis(((Optician)Session["user"]).IntUserId,intUserId,intUserId,Request.UserHostAddress.ToString(),"endid","log_testend");
				la = null;
			}
			
			
			
			//Her sletter vi fra temp_clients & optician_keys

			if(intIsFirst == 1) {
				strSql = "DELETE FROM optician_keys WHERE id = " + intId;
				db.execSql(strSql);
			}

			strSql = "DELETE FROM temp_clients WHERE ";

			if(intIsFirst == 1) {
				strSql += "passwordid";
			}
			else if(intIsFirst == 0) {
				strSql += "userid";
			}
			strSql += " = " + intId;
			
			db.execSql(strSql);


			//Træningsprogram

			if(intIsFirst == 1) {
				int lId = ((Optician)Session["user"]).IntLanguageId;

				string[][] arrComments = new string[5][];			
				arrComments[1] = new string[] {"<li>Lav monokulær træning indtil begge øjne er lige gode.</li>","<li>Vær opmærksom på, at migræneanfald kan fremprovokeres ved nogle dele af træningen. Lav derfor træningen i korte perioder af gangen og evt. i samråd med egen læge.</li>","<li>Husk at tjekke balancen!</li>","<li>Husk at tjekke koordinationen!</li>","15 minutters daglig træning i 2-3 måneder.","15 minutters daglig træning i 4-5 måneder.","15 minutters daglig træning i 5-6 måneder."};
				arrComments[2] = new string[] {"<li>!Lav monokulær træning indtil begge øjne er lige gode.</li>","<li>!Vær opmærksom på, at migræneanfald kan fremprovokeres ved nogle dele af træningen. Lav derfor træningen i korte perioder af gangen og evt. i samråd med egen læge.</li>","<li>!Husk at tjekke balancen!</li>","<li>!Husk at tjekke koordinationen!</li>","!15 minutters daglig træning i 2-3 måneder.","!15 minutters daglig træning i 4-5 måneder.","15 minutters daglig træning i 5-6 måneder."};
				arrComments[3] = new string[] {"<li>Perform monocular training until both eyes are equally good.</li>","<li>/Pay attention to the fact that some parts of the training may provoke migraine attacks. Therefore, the training should be performed for short periods each time and maybe in consultation with the GP.</li>","<li>Remember to check the balance!</li>","<li>/ Remember to check the coordination!</li>","15 minutes of daily training for 2-3 months.","15 minutes of daily training for 4-5 months.","15 minutes of daily training for 5-6 months."};
				arrComments[4] = new string[] {"<li>Führen Sie das Training mit jedem Auge einzeln durch bis beide Augen gleich gut sind..</li>","<li>/Beachten Sie, dass einige Elemente des Trainings Migräne-Anfälle auslösen können. Deshalb sollte das Training in kürzeren Inervallen ausgeführt werden, am besten nach Absprache mit Ihrem Arzt.</li>","<li>Vergessen Sie nicht die Balance zu prüfen!</li>","<li>Vergessen Sie nicht die Koordination zu prüfen!</li>","15 Minuten tägliches Training über 2-3 Monate.","15 Minuten tägliches Training über 4-5 Monate.","15 Minuten tägliches Training über 5-6 Monate."};

				string strTime = "";
				string strComment = "<ul>";

				Hashtable htTest = new Hashtable();

				db = new Database();
				strSql = "SELECT id,bbname FROM tests WHERE languageid = " + ((Optician)Session["user"]).IntLanguageId + " ORDER BY priority;";
				objDr = db.select(strSql);

				while(objDr.Read()) {
					htTest.Add(objDr["bbname"].ToString(),objDr["id"].ToString());
				}

				db.objDataReader.Close();

				//Trin 1

				bool defaultSchedule = true;
				bool trickSchedule = false;
				bool accessWWW = false;

				htValue = new Hashtable();

				db = new Database();
				strSql = "SELECT access_www FROM user_client WHERE userid = " + intUserId + ";";
				if(Convert.ToInt32(db.scalar(strSql)) == 1) {
					accessWWW = true;
				}
				db = null;

				Hashtable htSchedule = new Hashtable();

				htValue = new Hashtable();

				db = new Database();
				strSql = "SELECT a_1 ";

				for(int i = 2;i < 21;i++) {
					strSql += ",a_" + i;
				}

				strSql += " FROM a_anamnese WHERE clientid = " + intUserId + " AND isfirst = 1;";
				objDr = db.select(strSql);

				if(objDr.Read()) {
					for(int i = 1;i < 21;i++) {
						if(Convert.ToInt32(objDr["a_"+i]) > 0) {
							defaultSchedule = false;
						}
						
						htValue.Add(i.ToString(),objDr["a_" + i].ToString());
					}
				}

				db.objDataReader.Close();

				//3-D Levels tilføjes
				htSchedule.Add("3-D Level 0",htTest["3-D Level 0"]);
				htSchedule.Add("3-D Level 1",htTest["3-D Level 1"]);
				htSchedule.Add("3-D Level 2",htTest["3-D Level 2"]);
				htSchedule.Add("3-D Level 3",htTest["3-D Level 3"]);
				htSchedule.Add("3-D Level 4",htTest["3-D Level 4"]);
				htSchedule.Add("3-D Level 5",htTest["3-D Level 5"]);
				htSchedule.Add("3-D Level 6",htTest["3-D Level 6"]);
				htSchedule.Add("3-D Level 7",htTest["3-D Level 7"]);

				//Add levels if yes selected in anamnese
				for(int n = 1; n<19; n++) {

					string test = (string) htValue[n.ToString()];

					if((htValue[n.ToString()]).ToString() == "1") {

						if(!(htSchedule.ContainsKey("Level 1A"))) {
							htSchedule.Add("Level 1A",htTest["Level 1A"]);
						}
						if(!(htSchedule.ContainsKey("Level 1B"))) {
							htSchedule.Add("Level 1B",htTest["Level 1B"]);
						}
						if(!(htSchedule.ContainsKey("Level 2"))) {
							htSchedule.Add("Level 2",htTest["Level 2"]);
						}
						if(!(htSchedule.ContainsKey("Level 3"))) {
							htSchedule.Add("Level 3",htTest["Level 3"]);
						}
						if(!(htSchedule.ContainsKey("Level 4"))) {
							htSchedule.Add("Level 4",htTest["Level 4"]);
						}
						if(!(htSchedule.ContainsKey("Level 5"))) {
							htSchedule.Add("Level 5",htTest["Level 5"]);
						}
						if(!(htSchedule.ContainsKey("Level 6"))) {
							htSchedule.Add("Level 6",htTest["Level 6"]);
						}
						if(!(htSchedule.ContainsKey("Level 7"))) {
							htSchedule.Add("Level 7",htTest["Level 7"]);
						}
						if(!(htSchedule.ContainsKey("Level 8"))) {
							htSchedule.Add("Level 8",htTest["Level 8"]);
						}

						break;
					}
				}
			
				if(Convert.ToDouble(htValue["19"]) == 2.00 && Convert.ToDouble(htValue["20"]) == 2.00) { //Skal der dannes trick-træningsprogram?
					trickSchedule = true;
				}
					/*else if(htValue.ContainsValue("0") && trickSchedule == false) //Er der ikke udfyldte felter på trin 1 ?
					{
						defaultSchedule = true;
					}*/
				else {
					if(Convert.ToInt32(htValue["4"]) == 1) { //Ekstralevel A,B, og C.
						htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
						htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
						htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
					}
				
					if(Convert.ToInt32(htValue["7"]) == 1) { //Grundlevel G, Ekstralevel A,B og C.
						htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
			
						if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
							htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
							htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
							htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
						}

					}

					if(Convert.ToInt32(htValue["9"]) == 1) { //Grundlevel G, Ekstralevel A,B,C og D.
						htSchedule.Add("Ekstralevel D",htTest["Ekstralevel D"]);
			
						if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
							htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
							htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
							htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
						}

						if(!(htSchedule.ContainsKey("Grundlevel G"))) {
							htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
						}

						strComment += arrComments[lId][0];
					}

					if(Convert.ToInt32(htValue["10"]) == 1) { //Level 7, 8, Ekstralevel L

						if(!(htSchedule.ContainsKey("Level 7"))) {
							htSchedule.Add("Level 7",htTest["Level 7"]);
						}
						if(!(htSchedule.ContainsKey("Level 8"))) {
							htSchedule.Add("Level 8",htTest["Level 8"]);
						}

						htSchedule.Add("Ekstralevel L",htTest["Ekstralevel L"]);
					}

					if(Convert.ToInt32(htValue["11"]) == 1) { //Ekstralevel A, B, C, D, L, Grundlevel G, K, Level 1A, Alle Levels (uden 1B)
						strComment += arrComments[lId][0];

						if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
							htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
							htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
							htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel D"))) {
							htSchedule.Add("Grundlevel D",htTest["Grundlevel D"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel L"))) {
							htSchedule.Add("Grundlevel L",htTest["Grundlevel L"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel G"))) {
							htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
						}

						if(!(htSchedule.ContainsKey("Grundlevel K"))) {
							htSchedule.Add("Grundlevel K",htTest["Grundlevel K"]);
						}

						if(!(htSchedule.ContainsKey("Level 1A"))) {
							htSchedule.Add("Level 1A",htTest["Level 1A"]);
						}
						if(!(htSchedule.ContainsKey("Level 2"))) {
							htSchedule.Add("Level 2",htTest["Level 2"]);
						}
						if(!(htSchedule.ContainsKey("Level 3"))) {
							htSchedule.Add("Level 3",htTest["Level 3"]);
						}
						if(!(htSchedule.ContainsKey("Level 4"))) {
							htSchedule.Add("Level 4",htTest["Level 4"]);
						}
						if(!(htSchedule.ContainsKey("Level 5"))) {
							htSchedule.Add("Level 5",htTest["Level 5"]);
						}
						if(!(htSchedule.ContainsKey("Level 6"))) {
							htSchedule.Add("Level 6",htTest["Level 6"]);
						}

						if(!(htSchedule.ContainsKey("Level 7"))) {
							htSchedule.Add("Level 7",htTest["Level 7"]);
						}
						if(!(htSchedule.ContainsKey("Level 8"))) {
							htSchedule.Add("Level 8",htTest["Level 8"]);
						}
					}

					if(Convert.ToInt32(htValue["12"]) == 1) { //Kommentar
						strComment += arrComments[lId][1];
					}

					if(Convert.ToInt32(htValue["13"]) == 1) { //Grundlevel A,B,C og D
						htSchedule.Add("Grundlevel A",htTest["Grundlevel A"]);
						htSchedule.Add("Grundlevel B",htTest["Grundlevel B"]);
						htSchedule.Add("Grundlevel C",htTest["Grundlevel C"]);
						if(!(htSchedule.ContainsKey("Grundlevel D"))) {
							htSchedule.Add("Grundlevel D",htTest["Grundlevel D"]);
						}
					}

					if(Convert.ToInt32(htValue["14"]) == 1) { //Grundlevel E,F,H
						htSchedule.Add("Grundlevel E",htTest["Grundlevel E"]);
						htSchedule.Add("Grundlevel F",htTest["Grundlevel F"]);
						htSchedule.Add("Grundlevel H",htTest["Grundlevel H"]);
					}

					if(Convert.ToInt32(htValue["15"]) == 1) { //Grundlevel E,F, Ekstralevel L
						if(!(htSchedule.ContainsKey("Grundlevel E"))) {
							htSchedule.Add("Grundlevel E",htTest["Grundlevel E"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel F"))) {
							htSchedule.Add("Grundlevel F",htTest["Grundlevel F"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel L"))) {
							htSchedule.Add("Ekstralevel L",htTest["Ekstralevel L"]);
						}
					}

					if(Convert.ToInt32(htValue["16"]) == 1) { //Grundlevel A,B,C,D,K samt Ekstralevel D,E,F
						if(!(htSchedule.ContainsKey("Grundlevel A"))) {
							htSchedule.Add("Grundlevel A",htTest["Grundlevel A"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel B"))) {
							htSchedule.Add("Grundlevel B",htTest["Grundlevel B"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel C"))) {
							htSchedule.Add("Grundlevel C",htTest["Grundlevel C"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel D"))) {
							htSchedule.Add("Grundlevel D",htTest["Grundlevel D"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel K"))) {
							htSchedule.Add("Grundlevel K",htTest["Grundlevel K"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel D"))) {
							htSchedule.Add("Ekstralevel D",htTest["Ekstralevel D"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel E"))) {
							htSchedule.Add("Ekstralevel E",htTest["Ekstralevel E"]);
						}

						if(!(htSchedule.ContainsKey("Ekstralevel F"))) {
							htSchedule.Add("Ekstralevel F",htTest["Ekstralevel F"]);
						}
					}

					if(Convert.ToInt32(htValue["17"]) == 1) { //Grundlevel A,B,C,D,E,K, Ekstralevel D,E,F,L
						if(!(htSchedule.ContainsKey("Grundlevel A"))) {
							htSchedule.Add("Grundlevel A",htTest["Grundlevel A"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel B"))) {
							htSchedule.Add("Grundlevel B",htTest["Grundlevel B"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel C"))) {
							htSchedule.Add("Grundlevel C",htTest["Grundlevel C"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel D"))) {
							htSchedule.Add("Grundlevel D",htTest["Grundlevel D"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel E"))) {
							htSchedule.Add("Grundlevel E",htTest["Grundlevel E"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel K"))) {
							htSchedule.Add("Grundlevel K",htTest["Grundlevel K"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel D"))) {
							htSchedule.Add("Ekstralevel D",htTest["Ekstralevel D"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel E"))) {
							htSchedule.Add("Ekstralevel E",htTest["Ekstralevel E"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel F"))) {
							htSchedule.Add("Ekstralevel F",htTest["Ekstralevel F"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel L"))) {
							htSchedule.Add("Ekstralevel L",htTest["Ekstralevel L"]);
						}
					}

					if(Convert.ToInt32(htValue["18"]) == 1) { //Grundlevel E,F,G, Ekstralevel L
						if(!(htSchedule.ContainsKey("Grundlevel E"))) {
							htSchedule.Add("Grundlevel E",htValue["Grundlevel E"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel F"))) {
							htSchedule.Add("Grundlevel F",htValue["Grundlevel F"]);
						}
						if(!(htSchedule.ContainsKey("Grundlevel G"))) {
							htSchedule.Add("Grundlevel G",htValue["Grundlevel G"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel L"))) {
							htSchedule.Add("Ekstralevel L",htTest["Ekstralevel L"]);
						}
					}

					if(Convert.ToDouble(htValue["19"]) > 0.00 && Convert.ToDouble(htValue["20"]) > 0.00) {
						if(Convert.ToDouble(htValue["19"]) >= Convert.ToDouble(htValue["20"])) { //Alle 3-D Levels, Alle Levels
							if(!(htSchedule.ContainsKey("Level 1A"))) {
								htSchedule.Add("Level 1A",htTest["Level 1A"]);
							}
							if(!(htSchedule.ContainsKey("Level 1B"))) {
								htSchedule.Add("Level 1B",htTest["Level 1B"]);
							}
							if(!(htSchedule.ContainsKey("Level 2"))) {
								htSchedule.Add("Level 2",htTest["Level 2"]);
							}
							if(!(htSchedule.ContainsKey("Level 3"))) {
								htSchedule.Add("Level 3",htTest["Level 3"]);
							}
							if(!(htSchedule.ContainsKey("Level 4"))) {
								htSchedule.Add("Level 4",htTest["Level 4"]);
							}
							if(!(htSchedule.ContainsKey("Level 5"))) {
								htSchedule.Add("Level 5",htTest["Level 5"]);
							}
							if(!(htSchedule.ContainsKey("Level 6"))) {
								htSchedule.Add("Level 6",htTest["Level 6"]);
							}
							if(!(htSchedule.ContainsKey("Level 7"))) {
								htSchedule.Add("Level 7",htTest["Level 7"]);
							}
							if(!(htSchedule.ContainsKey("Level 8"))) {
								htSchedule.Add("Level 8",htTest["Level 8"]);
							}
						}
					}

					if(Convert.ToDouble(htValue["19"]) > 0.00 && Convert.ToDouble(htValue["20"]) > 0.00) {
						if(Convert.ToDouble(htValue["19"]) < Convert.ToDouble(htValue["20"])) { //Alle 3-D Levels, Alle Levels, Ekstralevel A,B,C,D,L samt Grundlevel G
							if(!(htSchedule.ContainsKey("Level 1A"))) {
								htSchedule.Add("Level 1A",htTest["Level 1A"]);
							}
							if(!(htSchedule.ContainsKey("Level 1B"))) {
								htSchedule.Add("Level 1B",htTest["Level 1B"]);
							}
							if(!(htSchedule.ContainsKey("Level 2"))) {
								htSchedule.Add("Level 2",htTest["Level 2"]);
							}
							if(!(htSchedule.ContainsKey("Level 3"))) {
								htSchedule.Add("Level 3",htTest["Level 3"]);
							}
							if(!(htSchedule.ContainsKey("Level 4"))) {
								htSchedule.Add("Level 4",htTest["Level 4"]);
							}
							if(!(htSchedule.ContainsKey("Level 5"))) {
								htSchedule.Add("Level 5",htTest["Level 5"]);
							}
							if(!(htSchedule.ContainsKey("Level 6"))) {
								htSchedule.Add("Level 6",htTest["Level 6"]);
							}
							if(!(htSchedule.ContainsKey("Level 7"))) {
								htSchedule.Add("Level 7",htTest["Level 7"]);
							}
							if(!(htSchedule.ContainsKey("Level 8"))) {
								htSchedule.Add("Level 8",htTest["Level 8"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
								htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
								htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
								htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel D"))) {
								htSchedule.Add("Ekstralevel D",htTest["Ekstralevel D"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel L"))) {
								htSchedule.Add("Ekstralevel L",htTest["Ekstralevel L"]);
							}
							if(!(htSchedule.ContainsKey("Grundlevel G"))) {
								htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
							}
						}
					}

					//Trin 2

					htValue = new Hashtable();

					db = new Database();
					strSql = "SELECT c_1,c_2,c_3 FROM a_convergence WHERE isfirst = 1 AND clientid = " + intUserId + ";";
					objDr = db.select(strSql);

					if(objDr.Read()) {
						if(Convert.ToInt32(objDr["c_1"]) > 0 || Convert.ToInt32(objDr["c_2"]) > 0 || Convert.ToInt32(objDr["c_3"]) > 0) {
							defaultSchedule = false;
						}
						htValue.Add("1",objDr["c_1"].ToString());
						htValue.Add("2",objDr["c_2"].ToString());
						htValue.Add("3",objDr["c_3"].ToString());
					}

					db.objDataReader.Close();
					db = null;

					if(Convert.ToInt32(htValue["1"]) > 1) { //Hvis spg. 1 > 5 cm - Ekstralevel A,B,C
						if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
							htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
							htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
							htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
						}

						if(Convert.ToInt32(htValue["2"]) > 1) { //Grundlevel G, Ekstralevel A,B,C, Kommentar
							if(!(htSchedule.ContainsKey("Grundlevel G"))) {
								htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
								htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
								htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
								htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
							}

							strComment += arrComments[lId][0];
						}
						if(Convert.ToInt32(htValue["3"]) == 1) { //Kommentar 
							strComment += arrComments[lId][2];
						}
					}

					//Trin 3

					htValue = new Hashtable();

					db = new Database();
					strSql = "SELECT m_1,m_2,m_3,m_4 FROM a_motilitet WHERE isfirst = 1 AND clientid = " + intUserId + ";";
					objDr = db.select(strSql);

					if(objDr.Read()) {
						if(Convert.ToInt32(objDr["m_1"]) > 0 || Convert.ToInt32(objDr["m_2"]) > 0 || Convert.ToInt32(objDr["m_3"]) > 0 || Convert.ToInt32(objDr["m_4"]) > 0) {
							defaultSchedule = false;
						}
						htValue.Add("1",objDr["m_1"].ToString());
						htValue.Add("2",objDr["m_2"].ToString());
						htValue.Add("3",objDr["m_3"].ToString());
						htValue.Add("4",objDr["m_4"].ToString());
					}

					db.objDataReader.Close();
					db = null;
					
					if(Convert.ToInt32(htValue["1"]) > 0 && Convert.ToInt32(htValue["2"]) > 0) {
						if(((Convert.ToInt32(htValue["1"]) - Convert.ToInt32(htValue["2"])) > 1) || ((Convert.ToInt32(htValue["1"]) - Convert.ToInt32(htValue["2"])) < -1)) { //Grundlevel G, Ekstralevel A,B,C, Kommentar
							if(!(htSchedule.ContainsKey("Grundlevel G"))) {
								htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
								htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
								htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
								htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
							}

							strComment += arrComments[lId][0];					
						}
					}

					if(Convert.ToInt32(htValue["1"]) > 0 && Convert.ToInt32(htValue["2"]) > 0 && Convert.ToInt32(htValue["3"]) > 0) {
						if((Convert.ToInt32(htValue["1"]) < 4) || (Convert.ToInt32(htValue["2"]) < 4) || (Convert.ToInt32(htValue["3"]) < 4)) { //Grundlevel G, Ekstralevel A,B,C,D,G
							if(!(htSchedule.ContainsKey("Grundlevel G"))) {
								htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel A"))) {
								htSchedule.Add("Ekstralevel A",htTest["Ekstralevel A"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel B"))) {
								htSchedule.Add("Ekstralevel B",htTest["Ekstralevel B"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel C"))) {
								htSchedule.Add("Ekstralevel C",htTest["Ekstralevel C"]);
							}
							if(!(htSchedule.ContainsKey("Ekstralevel D"))) {
								htSchedule.Add("Ekstralevel D",htTest["Ekstralevel D"]);
							}
						}
					}
					if(Convert.ToInt32(htValue["4"]) > 0) {
						if(Convert.ToInt32(htValue["4"]) == 1) { //Grundlevel A,G, Kommentar
							if(!(htSchedule.ContainsKey("Grundlevel A"))) {
								htSchedule.Add("Grundlevel A",htTest["Grundlevel A"]);
							}
							if(!(htSchedule.ContainsKey("Grundlevel G"))) {
								htSchedule.Add("Grundlevel G",htTest["Grundlevel G"]);
							}
						
							strComment += arrComments[lId][3];	
						}
					}

					if(accessWWW == false) { //Hvis klienten ikke har adgang til www
						//Ekstralevel E -> Klappetrampe grafikker samt øvelsesvejledning
						//Ekstralevel F -> Edderkoppeleg (Vi kan ikke finde den?)
						//Ekstralevel D -> Udgår
						//Ekstralevel A -> Optiker print øvelse
						//Level 1A -> Udgår
						//Ekstralevel C -> Optiker print øvelse
						//Ekstralevel B -> Ny øvelsesvejledning
						//3-D Level 0 -> Udgår
						//3-D Level 1 -> Ekstralevel I
						//3-D Level 2 -> -"-
						//3-D Level 3 -> -"-
						//3-D Level 4 -> -"-
						//3-D Level 5 -> -"-
						//3-D Level 6 -> Ekstralevel J
						//3-D Level 7 -> Udgår

						htSchedule.Remove("Ekstralevel D");
						htSchedule.Remove("Level 1A");
						htSchedule.Remove("3-D Level 0");
						htSchedule.Remove("3-D Level 1");
						htSchedule.Remove("3-D Level 2");
						htSchedule.Remove("3-D Level 3");
						htSchedule.Remove("3-D Level 4");
						htSchedule.Remove("3-D Level 5");
						htSchedule.Remove("3-D Level 6");
						htSchedule.Remove("3-D Level 7");

						if(!(htSchedule.ContainsKey("Ekstralevel I"))) {
							htSchedule.Add("Ekstralevel I",htTest["Ekstralevel I"]);
						}
						if(!(htSchedule.ContainsKey("Ekstralevel J"))) {
							htSchedule.Add("Ekstralevel J",htTest["Ekstralevel J"]);
						}
					}

				}

				strComment += "</ul>";

				//Standard eller trick program
				if(defaultSchedule) {
					htSchedule.Clear();

					if(empty == null) {
						htSchedule.Add("3-D Level 0",htTest["3-D Level 0"]);
						htSchedule.Add("3-D Level 1",htTest["3-D Level 1"]);
						htSchedule.Add("3-D Level 2",htTest["3-D Level 2"]);
						htSchedule.Add("3-D Level 3",htTest["3-D Level 3"]);
						htSchedule.Add("3-D Level 4",htTest["3-D Level 4"]);
						htSchedule.Add("3-D Level 5",htTest["3-D Level 5"]);
						htSchedule.Add("3-D Level 6",htTest["3-D Level 6"]);
						htSchedule.Add("3-D Level 7",htTest["3-D Level 7"]);

						htSchedule.Add("Level 1A",htTest["Level 1A"]);
						htSchedule.Add("Level 1B",htTest["Level 1B"]);
						htSchedule.Add("Level 2",htTest["Level 2"]);
						htSchedule.Add("Level 3",htTest["Level 3"]);
						htSchedule.Add("Level 4",htTest["Level 4"]);
						htSchedule.Add("Level 5",htTest["Level 5"]);
						htSchedule.Add("Level 6",htTest["Level 6"]);
						htSchedule.Add("Level 7",htTest["Level 7"]);
						htSchedule.Add("Level 8",htTest["Level 8"]);
					}
				}
				if(trickSchedule) {
					htSchedule.Clear();

					htSchedule.Add("3-D Level 0",htTest["3-D Level 0"]);
					htSchedule.Add("3-D Level 1",htTest["3-D Level 1"]);
					htSchedule.Add("3-D Level 2",htTest["3-D Level 2"]);
					htSchedule.Add("3-D Level 3",htTest["3-D Level 3"]);
					htSchedule.Add("3-D Level 4",htTest["3-D Level 4"]);
					htSchedule.Add("3-D Level 5",htTest["3-D Level 5"]);
					htSchedule.Add("3-D Level 6",htTest["3-D Level 6"]);
					htSchedule.Add("3-D Level 7",htTest["3-D Level 7"]);
				}

				//Tidsforbrug
				if(htSchedule.Count < 17) {
					strTime = arrComments[lId][4].ToString();
				}
				else if(htSchedule.Count > 30) {
					strTime = arrComments[lId][6].ToString();
				}
				else {
					strTime = arrComments[lId][5].ToString();
				}


				//Her gemmes træningsprogrammet i db'en
				for ( int xx = 0; xx <=1 ; xx++) {

					int intScheduleId = 0;
					int intLogId = 0;
					string[] arrStrValues = new string[Convert.ToInt32(htSchedule.Count)];
					string[] arrStrKeys = new string[Convert.ToInt32(htSchedule.Count)];

					htSchedule.Values.CopyTo(arrStrValues,0);
					htSchedule.Keys.CopyTo(arrStrKeys,0);
			
					db = new Database();
					strSql = "INSERT INTO test_schedule (addedtime,clientid,isactive,comments,guide) VALUES('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',";
					strSql += intUserId + ",1,'" + strComment + "','" + strTime + "');";
				
					//Log
					strSql += "INSERT INTO log_test_schedule (addedtime,clientid,comments,guide) VALUES('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',";
					strSql += intUserId + ",'" + strComment + "','" + strTime + "');";
					db.execSql(strSql);
			
					strSql = "SELECT id FROM test_schedule WHERE clientid = " + intUserId + " order by id desc;";
					intScheduleId = Convert.ToInt32(db.scalar(strSql));

					strSql = "SELECT id FROM log_test_schedule WHERE clientid = " + intUserId + " order by id desc;";
					intLogId = Convert.ToInt32(db.scalar(strSql));

					for(int i = 0;i <= arrStrValues.GetUpperBound(0);i++) {
						strSql = "INSERT INTO test_schedule_tests (scheduleid,testid,islocked) VALUES(" + intScheduleId + "," + Convert.ToInt32(arrStrValues[i]) + ",";
				
						if(arrStrKeys[i].ToString().Substring(0,3) == "3-D" && arrStrKeys[i].ToString() != "3-D Level 0") {
							strSql += "0";
						}
						else {
							strSql += "1";
						}

						strSql += ");";

						strSql += "INSERT INTO log_test_schedule_tests (scheduleid,testid,islocked) VALUES(" + intLogId + "," + Convert.ToInt32(arrStrValues[i]) + ",";
				
						if(arrStrKeys[i].ToString().Substring(0,3) == "3-D" && arrStrKeys[i].ToString() != "3-D Level 0") {
							strSql += "0";
						}
						else {
							strSql += "1";
						}

						strSql += ");";
				
						db.execSql(strSql);
					}
				}
				db = null;

				Literal js = new Literal();
				js.Text = "<script type='text/javascript'>\n";
				js.Text += "function handleLB(objCB,intId){\n";
				js.Text += "if(objCB.checked == true){\n";
				js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).disabled = false;\n";
				js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_'+intId).style.color = 'black';\n";
				js.Text += "}else{\n";
				js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).disabled = true;\n";
				js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).selectedIndex = '0';\n";
				js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_'+intId).style.color = '#666666';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/no_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
				js.Text += "}\n";
				js.Text += "}\n";

				js.Text += "function changeLock(intId){\n";
				js.Text += "if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '1'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/no_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
				js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '0'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/program_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
				js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '-1'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/optician_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
				js.Text += "}\n";
				js.Text += "}\n";
				js.Text += "</script>";

				Head_ph.Controls.Add(js);

				//this.Controls.Add(new Schedule().getOpticianSchedule(intUserId,true,IntPageId));
				string myMode = "details";
				if (strMode == "jump2program" ) {
					myMode = "schedule";
				}
				switch(((User)Session["user"]).IntLanguageId) {
					case 1: //dk
						Response.Redirect("?page=" + IntPageId + "&submenu=105&mode="+myMode+"&id=" + intUserId);
						break;
					case 2: //n
						Response.Redirect("?page=" + IntPageId + "&submenu=109&mode="+myMode+"&id=" + intUserId);
						break;
					case 3:
						Response.Redirect("?page=" + IntPageId + "&submenu=113&mode="+myMode+"&id=" + intUserId);
						break;
					case 4:
						Response.Redirect("?page=" + IntPageId + "&submenu=1180&mode="+myMode+"&id=" + intUserId);
						break;
				}
			}
			else {
				string myMode = "details";
				if (strMode == "jump2program" ) {
					myMode = "schedule";
				}
				switch(((User)Session["user"]).IntLanguageId){
					case 1: //dk
						Response.Redirect("?page=" + IntPageId + "&submenu=105&mode="+myMode+"&id=" + intUserId);
						break;
					case 2: //n
						Response.Redirect("?page=" + IntPageId + "&submenu=109&mode="+myMode+"&id=" + intUserId);
						break;
					case 3:
						Response.Redirect("?page=" + IntPageId + "&submenu=113&mode="+myMode+"&id=" + intUserId);
						break;
				}
			}
		}

		private void email_exist_val_servervalidate(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			strSql = "SELECT COUNT(*) AS found FROM users WHERE email = '" + email.Text + "';";

			Database db = new Database();

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				args.IsValid = (Convert.ToInt32(objDr["found"]) == 0);
			}

			db.objDataReader.Close();
			db = null;
		}
		private void cu_3(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[3].SelectedValue == "" && arrTextBox[7].Text == "" || wys.IsNumeric(arrTextBox[7].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_4(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[4].SelectedValue == "" && arrTextBox[8].Text == "" || wys.IsNumeric(arrTextBox[8].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_5(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[5].SelectedValue == "" && arrTextBox[9].Text == "" || wys.IsNumeric(arrTextBox[9].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_6(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[6].SelectedValue == "" && arrTextBox[10].Text == "" || wys.IsNumeric(arrTextBox[10].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_7(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[7].SelectedValue == "" && arrTextBox[11].Text == "" || wys.IsNumeric(arrTextBox[11].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_8(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[8].SelectedValue == "" && arrTextBox[12].Text == "" || wys.IsNumeric(arrTextBox[12].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_9(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[9].SelectedValue == "" && arrTextBox[13].Text == "" || wys.IsNumeric(arrTextBox[13].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_10(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[10].SelectedValue == "" && arrTextBox[14].Text == "" || wys.IsNumeric(arrTextBox[14].Text) == false ) {
				args.IsValid = false;
			}
		}
		private void cu_11(object source, ServerValidateEventArgs args) { //Kontrollerer om der eksisterer en klient med den indtastede email
			Wysiwyg wys = new Wysiwyg();

			if(arrListBox[11].SelectedValue == "" && arrTextBox[15].Text == "" || wys.IsNumeric(arrTextBox[15].Text) == false ) {
				args.IsValid = false;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {
		}
		#endregion

		private void saveStep1(object sender, EventArgs e) { //Gemmer info fra trin 1
			if(Page.IsValid) {
				Wysiwyg wys = new Wysiwyg();

				int intAccess = 0;
				string birthdateToDb = Convert.ToDateTime(birthdate.Text).Year + "/" + Convert.ToDateTime(birthdate.Text).Month + "/" + Convert.ToDateTime(birthdate.Text).Day;

				if(access_www.Checked == true) {
					intAccess = 1;
				}

				if(intStepSaved == 0) {
					strSql = "INSERT INTO temp_clients (passwordid,name,birthdate,address,zipcode,city,phone,fax,email,access_www,opticianid,lastedit,step) VALUES(";
					strSql += intId + ",'" + wys.ToDb(1,name.Text) + "','" + birthdateToDb + "','" + wys.ToDb(1,address.Text) + "','" + wys.ToDb(1,zipcode.Text) + "','" + wys.ToDb(1,city.Text) + "'"; 
					strSql += ",'" + wys.ToDb(1,phone.Text) + "','" + wys.ToDb(1,fax.Text) + "','" + wys.ToDb(1,email.Text) + "'," + intAccess + "," + ((Optician)Session["user"]).IntUserId;
					strSql += ",CURRENT_TIMESTAMP(),1);";
				}
				else {
					strSql = "UPDATE temp_clients SET name = '" + wys.ToDb(1,name.Text) + "',birthdate = '" + birthdateToDb + "',address = '" + wys.ToDb(1,address.Text) + "',zipcode = '" + wys.ToDb(1,zipcode.Text) + "',city = '" + wys.ToDb(1,city.Text) + "'";
					strSql += ",phone = '" + wys.ToDb(1,phone.Text) + "',fax = '" + wys.ToDb(1,fax.Text) + "',email = '" + wys.ToDb(1,email.Text) + "',access_www = " + intAccess + ",lastedit = CURRENT_TIMESTAMP() WHERE passwordid = " + intId;
				}

				Database db = new Database();

				db.execSql(strSql);

				db = null;
				wys = null;

				forward.Visible = true;
			}
		}

		private void saveStep2(object sender, EventArgs e) { //Gemmer info fra trin 2
			if(Page.IsValid) {
				Database db = new Database();

				if(intIsFirst == 1) {
					strSql = "UPDATE temp_clients SET ";

					for(int x = 0;x < 20;x++) {
						strSql += "2_" + (x+1) + " = " + arrListBox[x].SelectedValue.Replace(",",".") + ", ";
					}

					if(intStepSaved == 1) {
						strSql += "step = 2, ";
					}

					strSql += "lastedit = CURRENT_TIMESTAMP() WHERE passwordid = " + intId;
				}
				else if(intIsFirst == 0) {
					if(intStepSaved > 1) {
						strSql = "UPDATE temp_clients SET ";

						for(int x = 0;x < 20;x++) {
							strSql += "2_" + (x+1) + " = " + arrListBox[x].SelectedValue.Replace(",",".") + ", ";
						}

						strSql += "lastedit = CURRENT_TIMESTAMP() WHERE userid = " + intId;
					}
					else {
						strSql = "INSERT INTO temp_clients (";

						for(int x = 0;x < 20;x++) {
							strSql += "2_" + (x+1) + ", ";
						}

						strSql += "lastedit,userid,step) VALUES(";

						for(int x = 0;x < 20;x++) {
							strSql += arrListBox[x].SelectedValue.Replace(",",".") + ", ";
						}

						strSql += "CURRENT_TIMESTAMP()," + intId + ",2);";
					}
				}
				
				db.execSql(strSql);
				db = null;

				forward.Visible = true;
			}
		}

		private void saveStep3(object sender, EventArgs e) { //Gemmer info fra trin 3
			if(Page.IsValid) {
				Database db = new Database();

				strSql = "UPDATE temp_clients SET ";

				for(int x = 0;x < 3;x++) {
					strSql += "3_" + (x+1) + " = " + arrListBox[x].SelectedValue + ", ";
				}

				if(intStepSaved == 2) {
					strSql += "step = 3, ";
				}

				strSql += "lastedit = CURRENT_TIMESTAMP() WHERE ";
				
				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;

				db.execSql(strSql);

				db = null;

				forward.Visible = true;
			}
		}

		private void saveStep4(object sender, EventArgs e) { //Gemmer info fra trin 4
			if(Page.IsValid) {
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				strSql = "UPDATE temp_clients SET ";

				for(int x = 0;x < 4;x++) {
					strSql += "4_" + (x+1) + " = " + arrListBox[x].SelectedValue + ",4_" + (x+1) + "_1 = '" + wys.ToDb(2,arrTextBox[x].Text) + "', ";
				}

				if(intStepSaved == 3) {
					strSql += "step = 4, ";
				}

				strSql += "lastedit = CURRENT_TIMESTAMP() WHERE ";

				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;

				db.execSql(strSql);

				db = null;
				wys = null;

				forward.Visible = true;
			}
		}

		private void saveStep5(object sender, EventArgs e) { //Gemmer info fra trin 5
			if(Page.IsValid) {
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				strSql = "UPDATE temp_clients SET ";

				for(int i = 1;i < 4;i++) {
					strSql += "5_" + i + " = " + Convert.ToInt32(arrListBox[i - 1].SelectedValue) + ", ";
				}

				int intCounter = 0;

				for(int i = 4;i < 11;i++) {
					strSql += "5_" + i + " = ";
					if(i < 9) {
						strSql += "'" + arrTextBox[intCounter].Text.ToString() + "'";
					}
					else {
						if(arrTextBox[intCounter].Text != "") {
							strSql += Convert.ToDouble(arrTextBox[intCounter].Text).ToString().Replace(",",".");
						}
						else {
							strSql += 0;
						}
					}
					strSql += ", ";
					intCounter++;
				}

				intCounter = 3;

				for(int i = 11;i < 20;i++) {
					strSql += "5_" + i + " = '";
					if(arrTextBox[intCounter+4].Text == "") {
						strSql += arrListBox[intCounter].SelectedValue.Replace("'","''");
					}
					else {
						strSql += arrTextBox[intCounter+4].Text.Replace("'","''");
					}
					strSql += "', ";
					intCounter++;
				}

				if(intStepSaved == 4) {
					strSql += "step = 5, ";
				}

				strSql += "lastedit = CURRENT_TIMESTAMP() WHERE ";

				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;

				db.execSql(strSql);
				
				db = null;
				wys = null;

				forward.Visible = true;
			}
		}

		private void saveStep6(object sender, EventArgs e) {
			if(Page.IsValid) {
				double tmpValue = 0;

				//Udregning af LAG til #5h
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 8. (Hvis #15A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = eso, 2 = 0, 3 = exo

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 8.00;
					if(tmpValue > 2.00) {
						tmpValue = 2.00;
					}
				}
				else {
					tmpValue = 0.00;
				}

				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text.Replace(",",".")) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[11].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0;

				//Udregning af Netto til #5h
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				if(Convert.ToInt32(arrListBox[4].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[10].Text) - Convert.ToDouble(arrTb[11].Text);
				}
				if(Convert.ToInt32(arrListBox[4].SelectedValue) == 3) {
					if(arrTb[10].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[10].Text)*-1.00 - Convert.ToDouble(arrTb[11].Text);
					}
				}
					
				arrTb[12].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				//Udregning af LAG til #5v
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 8. (Hvis #15A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = eso, 2 = 0, 3 = exo

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 8.00;
					if(tmpValue > 2.00) {
						tmpValue = 2.00;
					}
				}
				else {
					tmpValue = 0.00;
				}

				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[14].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Udregning af Netto til #5v
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				if(Convert.ToInt32(arrListBox[5].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[13].Text) - Convert.ToDouble(arrTb[14].Text);
				}
				if(Convert.ToInt32(arrListBox[5].SelectedValue) == 3) {
					if(arrTb[13].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[13].Text)*-1.00 - Convert.ToDouble(arrTb[14].Text);
					}
				}
					
				arrTb[15].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Udregning af LAG til #14Ah
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 6. (Hvis #15 A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2,5.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 6.00;
					if(tmpValue > 2.50) {
						tmpValue = 2.50;
					}
				}
				else {
					tmpValue = 0.00;
				}

				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[44].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0;

				//Udregning af Netto til #14Ah
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[15].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[43].Text) - Convert.ToDouble(arrTb[44].Text);
				}
				if(Convert.ToInt32(arrListBox[15].SelectedValue) == 3) {
					if(arrTb[43].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[43].Text)*-1.00 - Convert.ToDouble(arrTb[44].Text);
					}
				}
					
				arrTb[45].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Udregning af LAG til #14Av
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 6. (Hvis #15 A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2,5.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 6.00;
					if(tmpValue > 2.50) {
						tmpValue = 2.50;
					}
				}
				else {
					tmpValue = 0.00;
				}
				
				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[47].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0;

				//Udregning af Netto til #14Av
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[16].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[46].Text) - Convert.ToDouble(arrTb[47].Text);
				}
				if(Convert.ToInt32(arrListBox[16].SelectedValue) == 3) {
					if(arrTb[46].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[46].Text)*-1.00 - Convert.ToDouble(arrTb[47].Text);
					}
				}
					
				arrTb[48].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Udregning af LAG til #14Bh
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 6. (Hvis #15 A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2,5.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 6.00;
					if(tmpValue > 2.50) {
						tmpValue = 2.50;
					}
				}
				else {
					tmpValue = 0.00;
				}
				
				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[51].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0;

				//Udregning af Netto til #14Bh
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[18].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[50].Text) - Convert.ToDouble(arrTb[51].Text);
				}
				if(Convert.ToInt32(arrListBox[18].SelectedValue) == 3) {
					if(arrTb[50].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[50].Text)*-1.00 - Convert.ToDouble(arrTb[51].Text);
					}
				}
					
				arrTb[52].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Udregning af LAG til #14Bv
				//LAG: = værdien fra # 15A, hvis denne er exo delt med 6. (Hvis #15 A er 0 eller eso er LAG = 0)
				//Denne værdi kan aldrig overstige 2,5.
				//Hvis #19 Begge er mindre end 5 skal der beregnes et modificeret LAG: 	LAG-værdien ganget med #19 og delt med 5
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[17].SelectedValue) == 3) {
					tmpValue = Convert.ToDouble(arrTb[49].Text) / 6.00;
					if(tmpValue > 2.50) {
						tmpValue = 2.50;
					}
				}
				else {
					tmpValue = 0.00;
				}
				if(arrTb[69].Text != ""){
					if(Convert.ToDouble(arrTb[69].Text) < 5.00) {
						tmpValue = (tmpValue*Convert.ToDouble(arrTb[69].Text)) / 5.00;
					}
				}
				arrTb[54].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0;

				//Udregning af Netto til #14Bv
				//NETTO: Beregnes ved at trække LAG, der altid er et plustal, fra den målte værdi = bruttoværdien.
				//Hvis bruttoværdien er plus gøres dette plus mindre og bliver evt. til et minus tal.
				//Hvis Bruttoværdien er et minustal bliver NETTO endnu mere minus.
				
				//1 = +, 2 = 0, 3 = -

				if(Convert.ToInt32(arrListBox[19].SelectedValue) == 1) {
					tmpValue = Convert.ToDouble(arrTb[53].Text) - Convert.ToDouble(arrTb[54].Text);
				}
				if(Convert.ToInt32(arrListBox[19].SelectedValue) == 3) {
					if(arrTb[53].Text != ""){
						tmpValue = Convert.ToDouble(arrTb[53].Text)*-1.00 - Convert.ToDouble(arrTb[54].Text);
					}
				}
					
				arrTb[55].Text = decimal.Round(Convert.ToDecimal(tmpValue),2).ToString();

				tmpValue = 0.00;

				//Er der kryds i #9?
				if(arrTb[33].Text.ToString().ToLower() == "x") {
					arrTb[33].Text = arrTb[34].Text.ToString();
				}

				//Er der kryds i #16A
				if(arrTb[57].Text.ToString().ToLower() == "x") {
					arrTb[57].Text = arrTb[58].Text.ToString();
				}

				//Er der kryds i #17A
				if(arrTb[60].Text.ToString().ToLower() == "x") {
					arrTb[60].Text = arrTb[61].Text.ToString();
				}

				Database db = new Database();
				string strSql = "UPDATE temp_clients SET ";

				for(int n = 0;n < 72;n++) {
					strSql += "6_" + (n+1) + " = ";
                    if ( (n > 32 && n < 38) || (n > 56 && n < 63) ) { strSql += "'"; }

					if(arrTb[n].Text != "") {
                        strSql += arrTb[n].Text.Replace(",", ".");
					}
					else {
						strSql += "-1.00";
					}
                    if((n > 32 && n < 38) || (n > 56 && n < 63)) { strSql += "'"; }

					strSql += ", ";
				}

				for(int n = 0;n < 24;n++) {
					strSql += "6_" + (n+73) + " = " + Convert.ToInt32(arrListBox[n].SelectedValue) + ", ";
				}

				for(int n = 0;n < 4;n++) {
					strSql += "6_" + (n+97) + " = ";

					if(arrRb[n].Checked) {
						strSql += 1;	
					}
					else {
						strSql += 0;
					}

					strSql += ", ";
				}

				if(intStepSaved == 5) {
					strSql += "step = 6, ";
				}

				strSql += "lastedit = CURRENT_TIMESTAMP() WHERE ";

				if(intIsFirst == 1) {
					strSql += "passwordid";
				}
				else if(intIsFirst == 0) {
					strSql += "userid";
				}
				strSql += " = " + intId;

				db.execSql(strSql);
							
				db = null;

				forward.Visible = true;
			}
		}

		private void saveControl(object sender, EventArgs e) {
			Database db = new Database();
			string strSql = "INSERT INTO a_control (";

			for(int i = 1;i < 8;i++) {
				strSql += "a_" + i + ",";
			}

			strSql += "a_4_1,a_5_1,a_6_1,a_7_1,a_8,a_9,addedtime,clientid) VALUES(";

			for(int i = 0;i < 7;i++) {
				strSql += Convert.ToInt32(arrListBox[i].SelectedValue) + ",";
			}

			strSql += "'" + arrTextBox[0].Text.ToString() + "','" + arrTextBox[1].Text.ToString() + "','" + arrTextBox[2].Text.ToString() + "','" + arrTextBox[3].Text.ToString() + "','";
			strSql += arrTextBox[4].Text.ToString() + "','" + arrTextBox[5].Text.ToString() + "',CURRENT_TIMESTAMP()," + intId + ");";

			db.execSql(strSql);
			
			strSql = "LOCK TABLE a_control WRITE;";
			db.execSql(strSql);

			strSql = "SELECT id FROM a_control WHERE clientid = " + intId + " ORDER BY id DESC LIMIT 0,1;";
			int intControlId = Convert.ToInt32(db.scalar(strSql));

			strSql = "UNLOCK TABLES;";
			db.execSql(strSql);

			db = null;

			LogAnalysis la = new LogAnalysis(((Optician)Session["user"]).IntUserId,intId,intControlId,Request.UserHostAddress.ToString(),"controlid","log_testcontrol");
			la = null;

			Response.Redirect("?page=" + IntPageId + "&submenu=" + strRef + "&mode=details&id=" + intId);
		}
	}
}