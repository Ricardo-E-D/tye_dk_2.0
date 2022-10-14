using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using tye;

namespace tye.ScreenTasks
{
	public partial class ExtralevelD : System.Web.UI.Page
	{
		protected Tests test;
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
			exNo.Value = intId.ToString();

			if(((Tests)Session["tests"]).DatStarttime.CompareTo(Convert.ToDateTime("01-01-2004 01:01:01")) == 0)
				((Tests)Session["tests"]).DatStarttime = DateTime.Now;

			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'ExtralevelD.aspx?id=map1a&intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];

			string id = "map1a";
			try {
				id = Request.QueryString["id"];
			}
			catch { }
			try {
				HtmlAnchor ha = (HtmlAnchor)FindControl("lnk" + id);
				if(ha != null) {
					ha.Style.Add("color", "#888888");
				}
			}
			catch {}

			this.mapNum.Value = id;
			if(!score.Value.ToString().Equals("!")) {
				CreateLog();
			}
			GenerateMenu();
		}

		private void GenerateMenu() {
			letterMenu.InnerHtml = "";
			int intCount = 0;
			for (int i = 1; i < 6; i++) {
				for (int j = 1; j < 3; j++) {
					intCount++;
					string strMap = "map" + i.ToString() + (j == 1 ? "a" : "b");
					string strMapQuerystring = (Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "");
					letterMenu.InnerHtml += "<a " + (strMap == strMapQuerystring ? "style=\"text-decoration:underline;color:#999;font-weight:bold;\" " : "") + "runat=\"server\" id=\"lnkmap" + i.ToString();
					letterMenu.InnerHtml += (j == 1 ? "a" : "b") + "\"";
					letterMenu.InnerHtml += "href=\"ExtralevelD.aspx?id=" + strMap +
											"&intExerciseIdNo=" + intId.ToString() +
											"\">" + intCount.ToString() + "</a>";
					if (i < 6)
						letterMenu.InnerHtml += " | ";
				}
			}
		}

		private void CreateLog() {
			if (Session["tests"] == null)
				return;

			System.Collections.Generic.List<Pair> lstMaps = new System.Collections.Generic.List<Pair>();
			lstMaps.Add(new Pair("map1a", "Level 1"));
			lstMaps.Add(new Pair("map1b", "Level 2"));
			lstMaps.Add(new Pair("map2a", "Level 3"));
			lstMaps.Add(new Pair("map2b", "Level 4"));
			lstMaps.Add(new Pair("map3a", "Level 5"));
			lstMaps.Add(new Pair("map3b", "Level 6"));
			lstMaps.Add(new Pair("map4a", "Level 7"));
			lstMaps.Add(new Pair("map4b", "Level 8"));
			lstMaps.Add(new Pair("map5a", "Level 9"));
			lstMaps.Add(new Pair("map5b", "Level 10"));

			Tests T = (Tests)Session["tests"];
			T.BlnCompleted = true;
			T.IntScore = Convert.ToInt32(score.Value);
			T.BlnHighScore = false;
			if (Convert.ToInt32(score.Value) >= T.IntHighScore) {
				T.BlnHighScore = true;
				T.IntHighScore = Convert.ToInt32(score.Value);
			}
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.AttValue = Request.QueryString["id"];

			// override log save value with something that makes sense.
			Pair P = lstMaps.Find(delegate(Pair P1) { return P1.First.ToString() == Request.QueryString["id"]; });
			if (P != null) {
				T.AttValue = P.Second.ToString();
			}

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
