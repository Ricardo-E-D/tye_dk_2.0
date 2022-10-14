namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Mail;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;

	public partial class contact : uc_pages
	{
		protected TextBox name = new TextBox();
		protected TextBox address = new TextBox();
		protected TextBox zipcode = new TextBox();
		protected TextBox city = new TextBox();
		protected TextBox phone = new TextBox();
		protected TextBox email = new TextBox();
		protected TextBox topic = new TextBox();
		protected TextBox body = new TextBox();
		protected CheckBox sendcopy = new CheckBox();
		protected CheckBox forfaq = new CheckBox();
		protected string strMode;
		protected string[] arrInfos;
		protected string strKey;
		protected string[] arrChars;
		protected Random r;
		protected int intNextChar;
		protected int intIsLocked;
		protected int intReferenceId;
		protected string strSql_name;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];

			try
			{
				Database db = new Database();

				string strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read())
				{
					arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
				}

				db.objDataReader.Close();
				db = null;			


				drawMailForm();

			}
			catch(NoDataFound ndf)
			{
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}
			

		}

		private void drawMailForm()
		{
			if(Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl(arrInfos[1].ToString() + ": * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();

			name_val.ID = "name_val";
			name_val.ErrorMessage = arrInfos[10].ToString();
			name_val.ControlToValidate = "name";
			name_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(name_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 200;
			name.Style.Add("width","200px");
			
			this.Controls.Add(name);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[2].ToString() + ":<br/>"));

			address.ID = "address";
			address.Width = 200;
			address.Style.Add("width","200px");
			
			this.Controls.Add(address);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[3].ToString() + " & " + arrInfos[4].ToString() + ":<br/>"));

			zipcode.ID = "zipcode";
			zipcode.Width = 8;
			zipcode.Style.Add("width","70px");

			this.Controls.Add(zipcode);

			this.Controls.Add(new LiteralControl(" "));

			city.ID = "city";
			city.Width = 125;
			city.Style.Add("width","125px");

			this.Controls.Add(city);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[5].ToString() + ":<br/>"));

			phone.ID = "phone";
			phone.Width = 200;
			phone.Style.Add("width","200px");

			this.Controls.Add(phone);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[6].ToString() + ": * "));

			RequiredFieldValidator email_val = new RequiredFieldValidator();

			email_val.ControlToValidate = "email";
			email_val.ErrorMessage = arrInfos[11].ToString();
			email_val.ID = "email_val";
			email_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(email_val);

			RegularExpressionValidator email_reg = new RegularExpressionValidator();

			email_reg.ControlToValidate = "email";
			email_reg.ErrorMessage = arrInfos[11].ToString();
			email_reg.ID = "email_reg";
			email_reg.Display = ValidatorDisplay.Dynamic;				
			email_reg.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

			this.Controls.Add(email_reg);

			this.Controls.Add(new LiteralControl("<br/>"));

			email.ID = "email";
			email.Width = 200;
			email.Style.Add("width","200px");

			this.Controls.Add(email);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[7].ToString() + ": * "));

			RequiredFieldValidator topic_val = new RequiredFieldValidator();

			topic_val.ControlToValidate = "topic";
			topic_val.ErrorMessage = arrInfos[10].ToString();
			topic_val.ID = "topic_val";
			topic_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(topic_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			topic.ID = "topic";
			topic.Width = 300;
			topic.Style.Add("width","375px");

			this.Controls.Add(topic);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[8].ToString() + ": * "));

			RequiredFieldValidator body_val = new RequiredFieldValidator();

			body_val.ControlToValidate = "body";
			body_val.ErrorMessage = arrInfos[10].ToString();
			body_val.ID = "body_val";
			body_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(body_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			body.ID = "body";
			body.Width = 300;
			body.Rows = 10;
			body.TextMode = TextBoxMode.MultiLine;
			body.Style.Add("width","375px");
			body.Style.Add("height","150px");

			this.Controls.Add(body);

			this.Controls.Add(new LiteralControl("<br/>"));

			sendcopy.ID = "sendcopy";
			sendcopy.Checked = true;
			sendcopy.Text = " " + arrInfos[18].ToString();
			
			this.Controls.Add(sendcopy);

			this.Controls.Add(new LiteralControl("<br/>"));

			forfaq.ID = "forfaq";
			forfaq.Checked = true;
			forfaq.Text = " " + arrInfos[19].ToString();
			
			this.Controls.Add(forfaq);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = arrInfos[9].ToString();
			submit.Width = 300;
			submit.Style.Add("width","375px");
			submit.Style.Add("margin-top","15px");
			submit.Click += new EventHandler(sendMail);

			this.Controls.Add(submit);
		}

		private void sendMail(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				MailMessage objMail = new MailMessage();
				objMail.To = "maria@trainyoureyes.com";

				if(sendcopy.Checked == true)
				{
					objMail.Cc = email.Text.ToString();
				}

				objMail.Subject = topic.Text.ToString();
				objMail.Body = body.Text.ToString();
				objMail.From = email.Text.ToString();
				objMail.BodyFormat = MailFormat.Text;

				if(forfaq.Checked == true)
				{
					objMail.Body += "\n\n" + arrInfos[19].ToString();
				}	    

				//SmtpMail.SmtpServer = "128.9.205.5";
				SmtpMail.SmtpServer = "localhost";
				
				SmtpMail.Send(objMail);
				
			
				Session["noerror"] = "<div id='noerror'>"+arrInfos[20].ToString()+"</div>";

				Response.Redirect("?page=" + IntPageId);
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		
	}
}
