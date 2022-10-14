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
using tye;

namespace tye.ScreenTasks
{
	public partial class ExtralevelA : System.Web.UI.Page {
		private string id;
		private int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try {
				intId = Convert.ToInt32(Request.QueryString["intExerciseIdNo"]);
			}
			catch { }
			if (Session["tests"] == null && intId > -1) {
				Tests T = new Tests();
				T = T.GetTestFromId(intId);
				T.DatStarttime = DateTime.Now;
				Session["tests"] = T;
			}

			if(((Tests)Session["tests"]).DatStarttime.CompareTo(Convert.ToDateTime("01-01-2004 01:01:01")) == 0)
				((Tests)Session["tests"]).DatStarttime = DateTime.Now;
		
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'ExtralevelA.aspx?intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];
			id = "1";
			try  {
				id = Request.QueryString["id"].ToString();
			} 
			catch  { } 
			
			this.GenerateMenu();	
			this.PrintPicture(id);
			
			if(!timeSpent.Value.ToString().Equals("!")) {
				CreateLog();
			}
		}

		private void PrintPicture(string id) {
			this.labyrintPicture.ImageUrl = "Images/ExtralevelA/" + id + ".png";	
			this.labyrintPicture.Visible = true;
		}	

		private void GenerateMenu()
		{
			for(int i = 1; i <= 20; i++) {
				Label lbl = new Label();
				lbl.Text = " | ";
				HyperLink button = new HyperLink();
				button.Text = i.ToString();
				button.NavigateUrl = "ExtralevelA.aspx?id=" + i + "&intExerciseIdNo=" + intId.ToString();
				if (i.ToString() == id)
					button.Style.Add("color", "#888888");
				this.labyrintMenu.Controls.Add(button);
				this.labyrintMenu.Controls.Add(lbl);
			}
		}

		private void CreateLog() {
			if(Session["tests"] == null)
				return;
			Tests T = (Tests)Session["tests"];
			T.BlnCompleted = true;
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.AttValue = this.id;
			T.saveLog();
			Session["tests"] = null;
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
