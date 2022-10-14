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

namespace tye.popups
{
	public partial class ActivateKeys : System.Web.UI.Page
	{
		protected string strDate;
		protected int intId;
		protected CheckBox[] cbArr = new CheckBox[99];
		private tye.Admin currentUser = null; 
		private Translation trans = null;
		private string strOpticianForNewlyCreatedCodes = "";

		protected void Page_Load(object sender, System.EventArgs e) {
			currentUser = (tye.Admin)Session["user"];
			if(((Login)Session["login"]).BlnSucces == true)
			{
				strDate = Convert.ToDateTime(Request.QueryString["date"].Replace("_", " ")).ToString("yyyy-MM-dd HH:mm:ss");
				
				intId = Convert.ToInt32(Request.QueryString["id"]);
				trans = new Translation(Server.MapPath("..\\uc\\translation.xml"), this.GetType().BaseType.ToString(), Translation.DbLangs[currentUser.IntLanguageId - 1].ToString());
				drawListPage();
			}
		}

		protected void drawListPage()
		{

			mainForm.Controls.Add(new LiteralControl("<div class='page_subheader'>" + trans.GetGeneral("keys") + "</div><br/>"));
            Database db = new Database();
			string strSql = "SELECT COUNT(*) FROM optician_keys WHERE addedtime = '" + strDate + "' AND opticianid = " + intId;
			int intKeys = Convert.ToInt32(db.scalar(strSql));
					
			for(int i = 0;i <= cbArr.GetUpperBound(0);i++)
			{
				cbArr[i] = new CheckBox();
			}

			strSql = "SELECT id,password,isactive FROM optician_keys WHERE addedtime = '" + strDate + "' AND opticianid = " + intId;
			MySqlDataReader objDr = db.select(strSql);

			int intCounter = 0;

			while(objDr.Read())
			{
				cbArr[intCounter].Text = " " + objDr["password"].ToString() + "<br/>";
				mainForm.Controls.Add(cbArr[intCounter]);
				
				if(Convert.ToInt32(objDr["isactive"]) == 1)
				{
					cbArr[intCounter].Checked = true;
				}

				intCounter++;
			}

			//rettet
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			Button submit = new Button();
			submit.Text = trans.GetGeneral("update");
			submit.Style.Add("width","200px");
			submit.Style.Add("margin-top","15px");
			submit.Click +=new EventHandler(updateKeys);

			mainForm.Controls.Add(submit);
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

		private void updateKeys(object sender, EventArgs e)
		{
			string strSql = "";
			int intActive = 0;
			string strCodesConcat = "";

			for(int i = 0;i <= cbArr.GetUpperBound(0);i++) {
				intActive = 0;

				if(cbArr[i].Checked) {
					intActive = 1;
				}

				strSql += "UPDATE optician_keys SET isactive = "+ intActive + " WHERE password = '";
				if(cbArr[i].Text != "") {
					strSql += cbArr[i].Text.Substring(1,6);
					if(intActive == 1)
						strCodesConcat += cbArr[i].Text.Substring(1,6) + '\n';
				}
				
				strSql += "';";
			}

			Database db = new Database();
			db.execSql(strSql);
			
			strSql = "SELECT user_optician.name,address,zipcode,city,email,phone FROM users INNER JOIN user_optician ON users.id = user_optician.userid WHERE users.id = " + intId;
			MySqlDataReader objDr = db.select(strSql);
			
			if (objDr.Read()) {
				strOpticianForNewlyCreatedCodes = objDr["name"].ToString() + "\n" +
												  objDr["address"].ToString() + "\n" +
												  objDr["zipcode"].ToString() + " " +
												  objDr["city"].ToString() + "\n" +
												  objDr["email"].ToString() + "\n" +
												  objDr["phone"].ToString() + "\n";
			}
			db.objDataReader.Close();
			objDr.Close();
			db.dbDispose();
			db = null;

			try {
				if (currentUser.IsDistributor) {
					Email em = new Email();
					em.SenderEmail = "noreply@trainyoureyes.com";
					em.RecipientEmail = Shared.MariaMail;
					em.Subject = "Distributøraktivitet - kodeaktivering";
					string strBodyText = "Distributør " + ((tye.Admin)Session["user"]).StrName + 
										 " har for\n\n" + strOpticianForNewlyCreatedCodes + "\naktiveret flg. kode(r):\n\n" + strCodesConcat;
					em.Body = strBodyText;
					em.Send();
				}
			}
			catch (Exception) { }

			Response.Redirect("ActivateKeys.aspx?date="+strDate+"&id="+intId);
		}
	}
}
