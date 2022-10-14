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

	public partial class admin_opt_admin : uc_pages
	{
		private string strMode;
		private int intId;

		private TextBox name = new TextBox();
		private ListBox languageId = new ListBox();
		private ListBox chainId = new ListBox();
		private TextBox optician = new TextBox();
		private TextBox password = new TextBox();
		private TextBox address = new TextBox();
		private TextBox zipcode = new TextBox();
		private TextBox city = new TextBox();
		private TextBox newChain = new TextBox();
		private TextBox region = new TextBox();
		private Button submit = new Button();
		private TextBox email = new TextBox();
		private TextBox phone = new TextBox();
		private HtmlInputHidden region_id = new HtmlInputHidden();
		protected ListBox lbLanguage = new ListBox();

		private Admin currentUser = null;
		private Translation trans = null;

		protected void Page_Load(object sender, System.EventArgs e) {
			currentUser = (Admin)Session["user"];
			trans = new Translation(Server.MapPath("uc\\translation.xml"), this.GetType().BaseType.ToString(), Translation.DbLangs[currentUser.IntLanguageId - 1].ToString());

			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);

			if (IntSubmenuId == 91 || IntSubmenuId == 1223 || IntSubmenuId == 1224 || IntSubmenuId == 1225) {
				switch (strMode) {
					case "delete":
						deleteOptician();
						break;
					case "edit":
						editOptician();
						break;
					default:
						drawArchivePage();
						break;
				}
			} else {
				drawAddPage();
			}
		}

		private void deleteOptician()
		{
			// mital
			Optician O = null;

			Database db = new Database();
			string strSql = "SELECT users.id,user_optician.name,optician_code.languageid,chainid,optician,password,address,zipcode,city,email,phone,regionid,map_region.name AS regionname FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id";
			strSql += " INNER JOIN language ON optician_code.languageid = language.id INNER JOIN map_region ON regionid = map_region.id WHERE users.id = " + intId + ";";
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read()) {
				O = new Optician();
				O.StrName = objDr["name"].ToString();
				O.StrEmail = objDr["email"].ToString();
				O.StrCity = objDr["city"].ToString();
				O.StrPassword = objDr["password"].ToString();
				O.StrAddress = objDr["address"].ToString();
				O.StrZipCode = objDr["zipcode"].ToString();
				O.StrPhone = objDr["phone"].ToString();
			}

			db.objDataReader.Close();
			db = null;
			//mital
			
			db = new Database();
			strSql = "UPDATE users SET isactive = 0 WHERE id = "+ intId;
			db.execSql(strSql);

			db = null;

			try { // notify Maria
				if (currentUser.IsDistributor && O != null) {
					Email em = new Email();
					em.SenderEmail = "noreply@trainyoureyes.com";
					em.RecipientEmail = Shared.MariaMail;
					em.Subject = "Distributøraktivitet - optiker slettet";
					string strBodyText = "Distributør " + ((tye.Admin)Session["user"]).StrName +
										 " har netop slettet flg. optiker:\n\n" +
										 O.StrName + '\n' +
										 O.StrAddress + '\n' +
										 O.StrZipCode + " " + O.StrCity + '\n' +
										 O.StrPhone + '\n' +
										 O.StrEmail + '\n' +
										 O.StrPassword; 
					em.Body = strBodyText;
					em.Send();
				}
			}
			catch (Exception) { }

			Response.Redirect("?page="+IntPageId+"&submenu="+IntSubmenuId);
		}

		private void drawArchivePage()
		{
			int intLanguageid = Convert.ToInt32(Request.QueryString["language"]);
				
			if(intLanguageid == 0){
				intLanguageid = (currentUser.IsDistributor ? currentUser.IntLanguageId : 1);
			}

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function Delete(intid){\n";
			js.Text += "var confirmDelete = window.confirm('" + trans.GetGeneral("confirm_sureDeleteOptician") + "');\n";
			js.Text += "if(confirmDelete){\n";
			js.Text += "location.href = '?page="+IntPageId+"&submenu="+IntSubmenuId+"&mode=delete&id='+intid;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>";
			
			Head_ph.Controls.Add(js);
			Database db = null;
			string strSql = "";
			if(!currentUser.IsDistributor) {
				this.Controls.Add(new LiteralControl("Sprog: "));

				lbLanguage.ID = "lbLanguage";
				lbLanguage.Rows = 1;
				lbLanguage.Attributes["onchange"] = "location.href='?page="+IntPageId+"&submenu="+IntSubmenuId+"&language='+this.value";
				
				db = new Database();
				strSql = "SELECT id,name FROM language" + (currentUser.IsDistributor ? " WHERE  id = " + currentUser.IntLanguageId : "") + " ORDER BY name";
				lbLanguage.DataSource = db.select(strSql);
				lbLanguage.DataTextField = "name";
				lbLanguage.DataValueField = "id";
				lbLanguage.DataBind();
				
				db.objDataReader.Close();
				db = null;

				this.Controls.Add(lbLanguage);
				this.Controls.Add(new LiteralControl("<br /><br />"));
			}

			db = new Database();
			strSql = "SELECT users.id,user_optician.name,CONCAT(tyeid,'-',optician_chain.name,'-',optician) AS optid FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id";
			strSql += " INNER JOIN language ON optician_code.languageid = language.id INNER JOIN optician_chain ON chainid = optician_chain.id WHERE optician_code.languageid = "+intLanguageid+" AND usertypeid = 2 AND users.isactive = 1 ORDER BY user_optician.name";
            			
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

			objBc.DataField = "name";
			objBc.HeaderText = trans.GetGeneral("name");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 250;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "optid";
			objBc.HeaderText = trans.GetGeneral("opticianid");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 130;
			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id={0}";
			objHlc.HeaderText = trans.GetGeneral("edit");
			objHlc.Text = trans.GetGeneral("edit");
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);
		
			objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "javascript:Delete({0});";
			objHlc.HeaderText = trans.GetGeneral("delete");
			objHlc.Text = trans.GetGeneral("delete");
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			if(!(db.objDataReader.HasRows)){
				this.Controls.Add(new LiteralControl(trans.GetString("noOpticiansFound")));
			}else{
				this.Controls.Add(objDg);
			}

			db.objDataReader.Close();
			db = null;

			lbLanguage.SelectedValue = intLanguageid.ToString();

		}

		private void drawAddPage()
		{
			if (Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl(trans.GetGeneral("name") + ": * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();

			name_val.ControlToValidate = "name";
			name_val.ErrorMessage = trans.GetGeneral("requiredField"); //"Dette felt skal udfyldes.";
			name_val.ID = "name_val";
			name_val.Display = ValidatorDisplay.Dynamic;	

			this.Controls.Add(name_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 65;
			name.MaxLength = 255;
			name.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(name);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			Database db = new Database();

			this.Controls.Add(new LiteralControl(trans.GetGeneral("opticianid") + ": * "));

			RequiredFieldValidator optician_req = new RequiredFieldValidator();

			optician_req.ControlToValidate = "optician";
			optician_req.ErrorMessage = trans.GetString("4chrnumer");
			optician_req.ID = "optician_req";
			optician_req.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(optician_req);

			RegularExpressionValidator optician_val = new RegularExpressionValidator();
			optician_val.ID = "optician_val";
			optician_val.ErrorMessage = trans.GetString("4chrnumer");
			optician_val.ControlToValidate = "optician";
			optician_val.Display = ValidatorDisplay.Dynamic;
			optician_val.ValidationExpression = "\\d{4}";
			
			this.Controls.Add(optician_val);

			CustomValidator optician_exist_val = new CustomValidator();
			optician_exist_val.ID = "optician_exist_val";
			optician_exist_val.ErrorMessage = trans.GetString("opticianIdAlreadyExists");
			optician_exist_val.ControlToValidate = "optician";
			optician_exist_val.Display = ValidatorDisplay.Dynamic;
			optician_exist_val.ServerValidate += new ServerValidateEventHandler(optician_exist_val_servervalidate);

			this.Controls.Add(optician_exist_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			string strSql = "SELECT id,tyeid FROM language WHERE isactive = 1 ORDER BY id;";

			languageId.DataSource = db.select(strSql);

			languageId.ID = "languageid";

			languageId.DataTextField = "tyeid";
			languageId.DataValueField = "id";

			languageId.Rows = 1;

			languageId.Attributes["onchange"] = "popdown();";

			languageId.DataBind();

			this.Controls.Add(languageId);

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(new LiteralControl(" "));

			db = new Database();

			strSql = "SELECT id,name FROM optician_chain ORDER BY id;";

			chainId.DataSource = db.select(strSql);

			chainId.DataTextField = "name";
			chainId.DataValueField = "id";

			chainId.Rows = 1;

			chainId.DataBind();

			this.Controls.Add(chainId);

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(new LiteralControl(" "));

			optician.ID = "optician";
			optician.Width = 4;
			optician.Attributes["style"] = "width:35px;";
			optician.MaxLength = 4;

			this.Controls.Add(optician);

			this.Controls.Add(new LiteralControl(" "));

			password.ID = "password";
			password.Width = 6;
			password.Attributes["style"] = "width:55px;background:#CCCCCC;";
			password.MaxLength = 6;
			password.ReadOnly = false;
			password.Text = (new Admin()).generatePassword();

			this.Controls.Add(password);
			
			this.Controls.Add(new LiteralControl(" "));
			
			newChain.MaxLength = 3;
			newChain.Style.Add("width","25px");
			
			this.Controls.Add(newChain);

			this.Controls.Add(new LiteralControl(" "));

			Button btnChain = new Button();
			btnChain.Text = trans.GetGeneral("createNewChain");
			btnChain.Click +=new EventHandler(addNewChain);
			btnChain.CausesValidation = false;

			this.Controls.Add(btnChain);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl("Region: * "));

			RequiredFieldValidator region_val = new RequiredFieldValidator();

			region_val.ControlToValidate = "region";
			region_val.ErrorMessage = trans.GetGeneral("requiredField");
			region_val.ID = "region_val";
			region_val.Display = ValidatorDisplay.Dynamic;
            region_val.IsValid = true;

			//this.Controls.Add(region_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			region.ID = "region";
			region.Width = 65;
			region.Attributes["style"] = "width:200px;";
			region.ReadOnly = true;
            			
			this.Controls.Add(region);

			region_id.ID = "regionid";
			
			this.Controls.Add(region_id);

			this.Controls.Add(new LiteralControl(" "));

			HtmlAnchor chooseregion = new HtmlAnchor();

			chooseregion.InnerHtml = trans.GetGeneral("choose") + " region";
			chooseregion.Attributes["onclick"] = "popup(document.getElementById('" + languageId.ClientID + "').options[document.getElementById('" + languageId.ClientID + "').selectedIndex].value);";
			chooseregion.HRef = "../../#";
			chooseregion.Attributes["class"] = "page_admin_btn";

			this.Controls.Add(chooseregion);

			this.Controls.Add(new LiteralControl("<br/><br/>"));
	
			this.Controls.Add(new LiteralControl(trans.GetGeneral("address") + ": * "));

			RequiredFieldValidator address_val = new RequiredFieldValidator();

			address_val.ControlToValidate = "address";
			address_val.ErrorMessage = trans.GetGeneral("requiredField");
			address_val.ID = "address_val";
			address_val.Display = ValidatorDisplay.Dynamic;	

			this.Controls.Add(address_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			address.ID = "address";
			address.Width = 65;
			address.MaxLength = 255;
			address.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(address);			
	
			this.Controls.Add(new LiteralControl("<br/><br/>"));
	
			this.Controls.Add(new LiteralControl(trans.GetGeneral("postal") + ": * "));

			RequiredFieldValidator zipcode_val = new RequiredFieldValidator();

			zipcode_val.ControlToValidate = "zipcode";
			zipcode_val.ErrorMessage = trans.GetGeneral("requiredField");
			zipcode_val.ID = "zipcode_val";
			zipcode_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(zipcode_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			zipcode.ID = "zipcode";
			zipcode.Width = 10;
			zipcode.MaxLength = 10;
			zipcode.Attributes["style"] = "width:50px;";
		
			this.Controls.Add(zipcode);
	
			this.Controls.Add(new LiteralControl("<br/><br/> "));

			this.Controls.Add(new LiteralControl(trans.GetGeneral("city") + ": * "));

			RequiredFieldValidator city_val = new RequiredFieldValidator();

			city_val.ControlToValidate = "city";
			city_val.ErrorMessage = trans.GetGeneral("requiredField");
			city_val.ID = "city_val";
			city_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(city_val);

			this.Controls.Add(new LiteralControl("<br/> "));

			city.ID = "city";
			city.Width = 65;
			city.MaxLength = 255;
			city.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(city);	

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl("Email: * "));

			RequiredFieldValidator email_val = new RequiredFieldValidator();

			email_val.ControlToValidate = "email";
			email_val.ErrorMessage = trans.GetGeneral("error_validEmail");
			email_val.ID = "email_val";
			email_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(email_val);

			RegularExpressionValidator email_reg = new RegularExpressionValidator();

			email_reg.ControlToValidate = "email";
			email_reg.ErrorMessage = trans.GetGeneral("error_validEmail");
			email_reg.ID = "email_reg";
			email_reg.Display = ValidatorDisplay.Dynamic;				
			email_reg.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

			this.Controls.Add(email_reg);

			this.Controls.Add(new LiteralControl("<br/> "));

			email.ID = "email";
			email.Width = 65;
			email.MaxLength = 255;
			email.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(email);			

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl(trans.GetGeneral("phone") + ": * "));

			RequiredFieldValidator phone_val = new RequiredFieldValidator();

			phone_val.ControlToValidate = "phone";
			phone_val.ErrorMessage = trans.GetGeneral("requiredField");
			phone_val.ID = "phone_val";
			phone_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(phone_val);

			this.Controls.Add(new LiteralControl("<br/> "));

			phone.ID = "phone";
			phone.Width = 65;
			phone.MaxLength = 255;
			phone.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(phone);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			submit.ID = "submit";
			submit.Width = 65;
			submit.Attributes["style"] = "width:200px;";
			submit.Text = trans.GetGeneral("saveOptician");
			submit.Click += new EventHandler(saveOptician);

			this.Controls.Add(submit);

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "var mywin;\n";	
			js.Text += "function popup(language)\n";
			js.Text += "{\n";
			js.Text += "mywin = window.open('popups/List.aspx?mode=region&language=' + language,'Regions','width=400,height=400,toolbars=no,resizable=no');\n";
			js.Text += "}\n";
			js.Text += "function popdown()\n";
			js.Text += "{\n";
			js.Text += "if(typeof(mywin) == 'undefined'){\n";
			js.Text += "mywin = window.open('popups/List.aspx','Regions','width=1,height=1,toolbars=no,resizable=no');\n";
			js.Text += "}\n";
			js.Text += "mywin.close();\n";
			js.Text += "document.getElementById('" + region_id.ClientID + "').value = '';\n";
			js.Text += "document.getElementById('" + region.ClientID + "').value = '';\n";	
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);
		}

		private void editOptician(){
			if (Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl("<div id='noerror'>" + Session["noerror"].ToString() + "</div>"));

				Session["noerror"] = null;
			}
			this.Controls.Add(new LiteralControl(trans.GetGeneral("name") + ": * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();

			name_val.ControlToValidate = "name";
			name_val.ErrorMessage = trans.GetGeneral("requiredField");
			name_val.ID = "name_val";
			name_val.Display = ValidatorDisplay.Dynamic;	

			this.Controls.Add(name_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 65;
			name.MaxLength = 255;
			name.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(name);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			Database db = new Database();

			this.Controls.Add(new LiteralControl(trans.GetGeneral("opticianid") + ": * "));

			RequiredFieldValidator optician_req = new RequiredFieldValidator();

			optician_req.ControlToValidate = "optician";
			optician_req.ErrorMessage = trans.GetString("4chrnumer");
			optician_req.ID = "optician_req";
			optician_req.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(optician_req);

			RegularExpressionValidator optician_val = new RegularExpressionValidator();
			optician_val.ID = "optician_val";
			optician_val.ErrorMessage = trans.GetString("4chrnumer");
			optician_val.ControlToValidate = "optician";
			optician_val.Display = ValidatorDisplay.Dynamic;
			optician_val.ValidationExpression = "\\d{4}";
			
			this.Controls.Add(optician_val);

			RequiredFieldValidator password_req = new RequiredFieldValidator();

			password_req.ControlToValidate = "password";
			password_req.ErrorMessage = trans.GetString("6chrpassword");
			password_req.ID = "password_req";
			password_req.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(password_req);

			CustomValidator cv = new CustomValidator();
			cv.ControlToValidate = "password";
			cv.ErrorMessage = trans.GetString("passwordAlreadyInUse");
			cv.Display = ValidatorDisplay.Dynamic;
			cv.ServerValidate +=new ServerValidateEventHandler(password_check);

			this.Controls.Add(cv);

			this.Controls.Add(new LiteralControl("<br/>"));

			string strSql = "SELECT id,tyeid FROM language WHERE isactive = 1 ORDER BY id;";

			languageId.DataSource = db.select(strSql);

			languageId.ID = "languageid";

			languageId.DataTextField = "tyeid";
			languageId.DataValueField = "id";

			languageId.Rows = 1;

			languageId.Attributes["onchange"] = "popdown();";

			languageId.DataBind();

			this.Controls.Add(languageId);

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(new LiteralControl(" "));

			db = new Database();

			strSql = "SELECT id,name FROM optician_chain ORDER BY id;";

			chainId.DataSource = db.select(strSql);

			chainId.DataTextField = "name";
			chainId.DataValueField = "id";

			chainId.Rows = 1;

			chainId.DataBind();

			this.Controls.Add(chainId);

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(new LiteralControl(" "));

			optician.ID = "optician";
			optician.Width = 4;
			optician.Attributes["style"] = "width:35px;";
			optician.MaxLength = 4;

			this.Controls.Add(optician);

			this.Controls.Add(new LiteralControl(" "));

			password.ID = "password";
			password.Width = 6;
			password.Attributes["style"] = "width:55px;background:#CCCCCC;";
			password.MaxLength = 6;
			
			this.Controls.Add(password);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl("Region: * "));

			RequiredFieldValidator region_val = new RequiredFieldValidator();

			region_val.ControlToValidate = "region";
			region_val.ErrorMessage = trans.GetGeneral("requiredField");
			region_val.ID = "region_val";
			region_val.Display = ValidatorDisplay.Dynamic;	

			this.Controls.Add(region_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			region.ID = "region";
			region.Width = 65;
			region.Attributes["style"] = "width:200px;";
			region.ReadOnly = true;
            			
			this.Controls.Add(region);

			region_id.ID = "regionid";
			
			this.Controls.Add(region_id);

			this.Controls.Add(new LiteralControl(" "));

			HtmlAnchor chooseregion = new HtmlAnchor();

			chooseregion.InnerHtml = trans.GetGeneral("choose") + " region";
			chooseregion.Attributes["onclick"] = "popup(document.getElementById('" + languageId.ClientID + "').options[document.getElementById('" + languageId.ClientID + "').selectedIndex].value);";
			chooseregion.HRef = "../../#";
			chooseregion.Attributes["class"] = "page_admin_btn";

			this.Controls.Add(chooseregion);

			this.Controls.Add(new LiteralControl("<br/><br/>"));
	
			this.Controls.Add(new LiteralControl(trans.GetGeneral("address") + ": * "));

			RequiredFieldValidator address_val = new RequiredFieldValidator();

			address_val.ControlToValidate = "address";
			address_val.ErrorMessage = trans.GetGeneral("requiredField");
			address_val.ID = "address_val";
			address_val.Display = ValidatorDisplay.Dynamic;	

			this.Controls.Add(address_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			address.ID = "address";
			address.Width = 65;
			address.MaxLength = 255;
			address.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(address);			
	
			this.Controls.Add(new LiteralControl("<br/><br/>"));
	
			this.Controls.Add(new LiteralControl(trans.GetGeneral("postal") + ": * "));

			RequiredFieldValidator zipcode_val = new RequiredFieldValidator();

			zipcode_val.ControlToValidate = "zipcode";
			zipcode_val.ErrorMessage = trans.GetGeneral("requiredField");
			zipcode_val.ID = "zipcode_val";
			zipcode_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(zipcode_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			zipcode.ID = "zipcode";
			zipcode.Width = 10;
			zipcode.MaxLength = 10;
			zipcode.Attributes["style"] = "width:50px;";
		
			this.Controls.Add(zipcode);
	
			this.Controls.Add(new LiteralControl("<br/><br/> "));

			this.Controls.Add(new LiteralControl(trans.GetGeneral("city") + ": * "));

			RequiredFieldValidator city_val = new RequiredFieldValidator();

			city_val.ControlToValidate = "city";
			city_val.ErrorMessage = trans.GetGeneral("requiredField");
			city_val.ID = "city_val";
			city_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(city_val);

			this.Controls.Add(new LiteralControl("<br/> "));

			city.ID = "city";
			city.Width = 65;
			city.MaxLength = 255;
			city.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(city);	

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl("Email: * "));

			RequiredFieldValidator email_val = new RequiredFieldValidator();

			email_val.ControlToValidate = "email";
			email_val.ErrorMessage = trans.GetGeneral("error_validEmail");
			email_val.ID = "email_val";
			email_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(email_val);

			RegularExpressionValidator email_reg = new RegularExpressionValidator();

			email_reg.ControlToValidate = "email";
			email_reg.ErrorMessage = trans.GetGeneral("error_validEmail");
			email_reg.ID = "email_reg";
			email_reg.Display = ValidatorDisplay.Dynamic;				
			email_reg.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

			this.Controls.Add(email_reg);

			this.Controls.Add(new LiteralControl("<br/> "));

			email.ID = "email";
			email.Width = 65;
			email.MaxLength = 255;
			email.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(email);			

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			this.Controls.Add(new LiteralControl(trans.GetGeneral("phone") + ": * "));

			RequiredFieldValidator phone_val = new RequiredFieldValidator();

			phone_val.ControlToValidate = "phone";
			phone_val.ErrorMessage = trans.GetGeneral("requiredField");
			phone_val.ID = "phone_val";
			phone_val.Display = ValidatorDisplay.Dynamic;				

			this.Controls.Add(phone_val);

			this.Controls.Add(new LiteralControl("<br/> "));

			phone.ID = "phone";
			phone.Width = 65;
			phone.MaxLength = 255;
			phone.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(phone);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			submit.ID = "submit";
			submit.Width = 65;
			submit.Attributes["style"] = "width:200px;";
			submit.Text = trans.GetGeneral("update") + " " + trans.GetGeneral("optician");
			submit.Click +=new EventHandler(updateOptician);

			this.Controls.Add(submit);

			db = new Database();
			strSql = "SELECT users.id,user_optician.name,optician_code.languageid,chainid,optician,password,address,zipcode,city,email,phone,regionid,map_region.name AS regionname FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id";
			strSql += " INNER JOIN language ON optician_code.languageid = language.id INNER JOIN map_region ON regionid = map_region.id WHERE users.id = "+ intId +";";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				name.Text = objDr["name"].ToString();
				languageId.SelectedValue = objDr["languageid"].ToString();
				chainId.SelectedValue = objDr["chainid"].ToString();
                optician.Text = objDr["optician"].ToString();
				password.Text = objDr["password"].ToString();
				region_id.Value = objDr["regionid"].ToString();
				region.Text = objDr["regionname"].ToString();
				address.Text = objDr["address"].ToString();
				zipcode.Text = objDr["zipcode"].ToString();
				city.Text = objDr["city"].ToString();
				email.Text = objDr["email"].ToString();
				phone.Text = objDr["phone"].ToString();
			}

			db.objDataReader.Close();
			db = null;

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "var mywin;\n";	
			js.Text += "function popup(language)\n";
			js.Text += "{\n";
			js.Text += "mywin = window.open('popups/List.aspx?mode=region&language=' + language,'Regions','width=400,height=400,toolbars=no,resizable=no');\n";
			js.Text += "}\n";
			js.Text += "function popdown()\n";
			js.Text += "{\n";
			js.Text += "if(typeof(mywin) == 'undefined'){\n";
			js.Text += "mywin = window.open('popups/List.aspx','Regions','width=1,height=1,toolbars=no,resizable=no');\n";
			js.Text += "}\n";
			js.Text += "mywin.close();\n";
			js.Text += "document.getElementById('" + region_id.ClientID + "').value = '';\n";
			js.Text += "document.getElementById('" + region.ClientID + "').value = '';\n";	
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);
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

		private void saveOptician(object sender, EventArgs e)
		{
            if (Page.IsValid && region_id.Value + "x" != "x")
            {
                Optician objO = new Optician();

                objO.IntIsActive = 1;
                objO.IntOpticianChainId = Convert.ToInt32(chainId.SelectedValue);
                objO.IntLanguageId = Convert.ToInt32(languageId.SelectedValue);
                objO.IntRegionId = Convert.ToInt32(region_id.Value);
                objO.IntUserTypeId = 2;
                objO.StrAddress = address.Text.ToString();
                objO.StrCity = city.Text.ToString();
                objO.StrEmail = email.Text.ToString();
                objO.StrOpticianCode = optician.Text.ToString();
                objO.StrName = name.Text.ToString();
                objO.StrPassword = password.Text.ToString();
                objO.StrPhone = phone.Text.ToString();
                objO.StrZipCode = zipcode.Text.ToString();

                objO.Add(objO);


				try { // notify Maria
					if (currentUser.IsDistributor) {
						Email em = new Email();
						em.SenderEmail = "noreply@trainyoureyes.com";
						em.RecipientEmail = Shared.MariaMail;
						em.Subject = "Distributøraktivitet - oprettelse af optiker";
						string strBodyText = "Distributør " + ((tye.Admin)Session["user"]).StrName +
											 " har oprettet flg. optiker:\n\n" + objO.StrName +
											 " (" + objO.StrPassword + ")";
						em.Body = strBodyText;
						em.Send();
					}
				}
				catch (Exception) { }

                Response.Redirect("?page="+IntPageId);
            }
		}

        private void optician_exist_val_servervalidate(object source, ServerValidateEventArgs args)
		{
			string strSql = "SELECT COUNT(*) AS found FROM optician_code WHERE languageid = " + languageId.SelectedValue + " AND chainid = " + chainId.SelectedValue + " AND optician = '" + optician.Text+"'";

			Database db = new Database();

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				args.IsValid = (Convert.ToInt32(objDr["found"]) == 0);
			}

			db.objDataReader.Close();
			db = null;
		}

		private void email_exist_val_servervalidate(object source, ServerValidateEventArgs args)
		{
			string strSql = "SELECT COUNT(*) AS found FROM users WHERE email = '" + email.Text + "';";

			Database db = new Database();

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				args.IsValid = (Convert.ToInt32(objDr["found"]) == 0);
			}

			db.objDataReader.Close();
			db = null;
		}

		private void addNewChain(object sender, EventArgs e) {
			Database db = new Database();
			string strSql = "SELECT COUNT(*) FROM optician_chain WHERE name = '" + newChain.Text + "';";
			int intFound = Convert.ToInt32(db.scalar(strSql));

			if(intFound == 0){
				db = new Database();
				strSql = "INSERT INTO optician_chain (name) VALUES('" + newChain.Text + "');";
				db.execSql(strSql);

				db = null;			
			}

			Response.Redirect("?page=" + IntPageId);
		}

		private void updateOptician(object sender, EventArgs e) {
			if(Page.IsValid){
				Database db = new Database();
				
				int intOpticianCodeId = 0;

				string strSql = "SELECT COUNT(*) AS found FROM optician_code WHERE languageid = "+Convert.ToInt32(languageId.SelectedValue)+" AND chainid = "+Convert.ToInt32(chainId.SelectedValue)+" AND optician = '" + optician.Text + "';";
				int intFound = Convert.ToInt32(db.scalar(strSql));

				if(intFound == 0){
					db = new Database();
					strSql = "INSERT INTO optician_code (languageid,chainid,optician) VALUES("+Convert.ToInt32(languageId.SelectedValue)+","+Convert.ToInt32(chainId.SelectedValue)+",'"+optician.Text+"')";
					db.execSql(strSql);

					db = new Database();
					strSql = "SELECT id FROM optician_code WHERE languageid = "+Convert.ToInt32(languageId.SelectedValue)+" AND chainid = "+Convert.ToInt32(chainId.SelectedValue)+" AND optician = '" + optician.Text + "';";
					intOpticianCodeId = Convert.ToInt32(db.scalar(strSql));

					db = null;
				} else {
					db = new Database();
					strSql = "SELECT id FROM optician_code WHERE languageid = "+Convert.ToInt32(languageId.SelectedValue)+" AND chainid = "+Convert.ToInt32(chainId.SelectedValue)+" AND optician = '" + optician.Text + "';";
					intOpticianCodeId = Convert.ToInt32(db.scalar(strSql));
					
					db = null;
				}

				Wysiwyg wys = new Wysiwyg();

				db = new Database();
				strSql = "UPDATE users SET password = '"+ password.Text +"',address = '"+ wys.ToDb(2,address.Text) +"',zipcode ='"+ wys.ToDb(2,zipcode.Text) +"',city = '"+ wys.ToDb(2,city.Text) +"',email = '"+ email.Text +"',phone = '"+ wys.ToDb(2,phone.Text) +"' WHERE id = " + intId;
				db.execSql(strSql);

				db = new Database();
				strSql = "UPDATE user_optician SET name = '"+ wys.ToDb(2,name.Text) +"',opticiancodeid = " + intOpticianCodeId + ",regionid = " + Convert.ToInt32(region_id.Value) +" WHERE userid = " + intId;
				db.execSql(strSql);

				db = null;

				Session["noerror"] = trans.GetString("opticianUpdated"); //"Optikeren er nu opdateret.";

				try { // notify Maria
					if (currentUser.IsDistributor) {
						Email em = new Email();
						em.SenderEmail = "noreply@trainyoureyes.com";
						em.RecipientEmail = Shared.MariaMail;
						em.Subject = "Distributøraktivitet - redigering af optiker";
						string strBodyText = "Distributør " + ((tye.Admin)Session["user"]).StrName +
											 " har redigeret stamdata for flg. optiker:\n\n" + wys.ToDb(2, name.Text) +
											 " (" + password.Text + ")";
						em.Body = strBodyText;
						em.Send();
					}
				}
				catch (Exception) { }

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id=" + intId);
			}
		}

		private void password_check(object source, ServerValidateEventArgs args)
		{
			Database db = new Database();
			string strSql = "SELECT COUNT(*) FROM optician_keys WHERE password = '" + password.Text + "'";
			bool isNotFound = true;
			if(Convert.ToInt32(db.scalar(strSql)) > 0)
			{
				isNotFound = false;
			}
			strSql = "SELECT COUNT(*) FROM users WHERE password = '" + password.Text + "' AND id <> " + intId;
			if(Convert.ToInt32(db.scalar(strSql)) > 0)
			{
				isNotFound = false;
			}

			db = null;

			args.IsValid = isNotFound;
		}
	}
}
