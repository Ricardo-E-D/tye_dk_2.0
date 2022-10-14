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

namespace tye.ScreenTasks
{
	public partial class ExtralevelH : System.Web.UI.Page
	{
		int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(Request.QueryString["intExerciseIdNo"].ToString(), out intId);
			if (Session["tests"] == null) {
				Tests T = new Tests();
				if (intId > -1) {
					T = T.GetTestFromId(intId);
					Session["tests"] = T;
				}
			}

			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'ExtralevelH.aspx" + (intId > -1 ? "?intExerciseIdNo=" + intId.ToString() : "") + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];

			int id = 1;
			try {
				id = Convert.ToInt32(Request.QueryString["id"].ToString());
			} catch {
			} 
		
			img.ImageUrl = "Images/ExtralevelH/" + id + ".png";
			this.GenerateMenu(id);
			if(!timeSpent.Value.ToString().Equals("!"))
			{
				((Tests)Session["tests"]).AttValue = id.ToString();
				CreateLog();
			}
			
		}

		private void GenerateMenu(int id)
		{
			for(int i = 1; i <= 2; i++)
			{
				Label lbl = new Label();
				lbl.Text = " | ";
				HyperLink button = new HyperLink();
				button.Text = i.ToString();
				button.NavigateUrl = "ExtralevelH.aspx?id=" + i + "&intExerciseIdNo=" + intId;
				if(id == i)
				{
					button.Enabled = false;
					button.Style["color"] = "grey";
					button.Style["font-size"] = "14px";
				}
				this.letterMenu.Controls.Add(button);
				this.letterMenu.Controls.Add(lbl);
				if(i % 20 == 0)
				{
					Label lbl2 = new Label();
					lbl2.Text = "<br />";
					this.letterMenu.Controls.Add(lbl2);
				}
			}
		}

		public void CreateLog()
		{
			Tests T = (Tests)Session["tests"];
			T.BlnCompleted = true;
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.saveLog();
			Session["tests"] = null; // !!!
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
