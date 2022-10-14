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

	public partial class opt_shop : uc_pages {

		protected void Page_Load(object sender, System.EventArgs e) {
			try{
				Cart cart = new Cart();
				cart.sessionExists();
				cart = null;

				drawList();
			}
			catch(NoDataFound ndf){
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}
		}

		private void drawList() {
			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Attributes["style"] = "width:480px;border-collapse:collapse;";
			htList.Attributes["class"] = "data_table";
			htList.ID = "htList";

			HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			string[] arrAlt = new string[] { "", "Tilføj 1 [item] ([price]) til kurven.", "Tilføj 1 [item] ([price]) til kurven.", "Tilføj 1 [item] ([price]) til kurven.", "Tilføj 1 [item] ([price]) til kurven." };
			int[] arrHref = new int[] { 0, 179, 180, 181, 1198 };///XXXX
			int lId = ((tye.Menu)Session["menu"]).IntLanguageId;

			// ((tye.Menu)Session["menu"]).IntLanguageId
			string fName = "eName_" + Shared.LanguageAbbr(lId);
			string fDesc = "eDescription_" + Shared.LanguageAbbr(lId);

			Database db = new Database();
			string strSql = "SELECT eId, " + fName + ", " + fDesc + " FROM newequipment WHERE eActive = 1 ORDER BY " + fName;
			MySqlDataReader objDr = db.select(strSql);

			if (!(objDr.HasRows)) {
				throw new NoDataFound();
			}

			while (objDr.Read()) {
				trList = new HtmlTableRow();
				tcList = new HtmlTableCell();

				tcList.Style.Add("width", "130px");
				tcList.Style.Add("vertical-align", "top");
				tcList.Attributes["class"] = "data_table_item";
				tcList.Controls.Add(new LiteralControl("<span class='bold_text'>" + objDr.GetString(objDr.GetOrdinal(fName)) + "</span>"));
				trList.Controls.Add(tcList);

				tcList = new HtmlTableCell();
				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("width", "350px");

				if (objDr[fDesc].ToString() != "") {
					tcList.Controls.Add(new LiteralControl(objDr.GetString(objDr.GetOrdinal(fDesc)) + "<br/><br/>"));
				}

				string eDesc = "eiDescription_" + Shared.LanguageAbbr(lId);
				string ePrice = "eiPrice_" + Shared.LanguageAbbr(lId);

				Database db1 = new Database();
				strSql = "SELECT eiId, " + eDesc + ", " + ePrice + " FROM newequipmentitem WHERE eiEquipment = " + objDr["eId"].ToString() + " AND eiActive = 1";
				MySqlDataReader objDr1 = db1.select(strSql);

				while (objDr1.Read()) {
					string strAlt = arrAlt[lId].ToString().Replace("[item]", objDr.GetString(objDr.GetOrdinal(fName)).Replace("[price]", ((Cart)Session["cart"]).checkDecimals(objDr1.GetDouble(objDr1.GetOrdinal(ePrice)))) + "' id='cart_" + objDr1["eiId"].ToString());
					string strTc = "<div style='width:100%;margin-bottom:10px;'><div style='width:245px;float:left;'>";
					strTc += objDr1.GetString(objDr1.GetOrdinal(eDesc)) + "</div>"; // data reader "query" necessary to fix 'System.Byte[]'-bug
					strTc += "<div style='height:100%;text-align:right;'>";
					strTc += ((Cart)Session["cart"]).checkDecimals(objDr1.GetDouble(objDr1.GetOrdinal(ePrice)));
					strTc += "<br/><img onmouseover=\"this.style.cursor='hand'\"";
					strTc += " onclick=\"location.href='?page=" + IntPageId + "&submenu=" + arrHref[lId] + "&mode=add&id=" + objDr1["eiId"] + "'\" src='gfx/cart.gif'";
					strTc += " alt='" + strAlt + "' title='" + strAlt + "'/></div></div>";
					tcList.Controls.Add(new LiteralControl(strTc));
				}

				trList.Controls.Add(tcList);

				db1.objDataReader.Close();
				db1 = null;
				
				htList.Controls.Add(trList);
			}


			db.objDataReader.Close();
			db = null;

			this.Controls.Add(htList);

		}

		private void old_drawList(){
			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Attributes["style"] = "width:480px;border-collapse:collapse;";
			htList.Attributes["class"] = "data_table";
			htList.ID = "htList";

			HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();
			
			string[] arrAlt = new string[] {"","Tilføj 1 [item] ([price]) til kurven.","Tilføj 1 [item] ([price]) til kurven.","Tilføj 1 [item] ([price]) til kurven.","Tilføj 1 [item] ([price]) til kurven."};
			int[] arrHref = new int[] {0,179,180,181,1198};///XXXX
            int lId = ((tye.Menu)Session["menu"]).IntLanguageId;

			Database db = new Database();
            string strSql = "SELECT id,name,description FROM equipment WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " ORDER BY name";
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows)){
				throw new NoDataFound();
			}

			while(objDr.Read()){
				trList = new HtmlTableRow();
				tcList = new HtmlTableCell();

				tcList.Style.Add("width","130px");
				tcList.Style.Add("vertical-align","top");
				tcList.Attributes["class"] = "data_table_item";
				tcList.Controls.Add(new LiteralControl("<span class='bold_text'>"+objDr["name"].ToString()+"</span>"));
				trList.Controls.Add(tcList);

				tcList = new HtmlTableCell();
				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("width","350px");

				if(objDr["description"].ToString() != ""){
					tcList.Controls.Add(new LiteralControl(objDr["description"].ToString()+"<br/><br/>"));
				}

				Database db1 = new Database();
				strSql = "SELECT id,description,price FROM equipment_items WHERE equipmentid = " + Convert.ToInt32(objDr["id"]);
				MySqlDataReader objDr1 = db1.select(strSql);
				
				while(objDr1.Read()){
					string strTc = "<div style='width:100%;margin-bottom:10px;'><div style='width:245px;float:left;'>" +
										objDr1["description"].ToString() + "</div>" +
									"<div style='height:100%;text-align:right;'>" +
										((Cart)Session["cart"]).checkDecimals(Convert.ToDouble(objDr1["price"])) +
										"<br/><img onmouseover=\"this.style.cursor='hand'\"" +
										" onclick=\"location.href='?page=" + IntPageId + "&submenu=" + arrHref[lId] + "&mode=add&id=" + objDr1["id"] + "'\" src='gfx/cart.gif'" +
										" alt='" + arrAlt[lId].ToString().Replace("[item]", objDr["name"].ToString()).Replace("[price]", ((Cart)Session["cart"]).checkDecimals(Convert.ToDouble(objDr1["price"]))) + "' id='cart_" + objDr1["id"].ToString() + "' title='" + arrAlt[lId].ToString().Replace("[item]", objDr["name"].ToString()).Replace("[price]", ((Cart)Session["cart"]).checkDecimals(Convert.ToDouble(objDr1["price"]))) + "'/></div></div>";
					tcList.Controls.Add(new LiteralControl(strTc));
				}

				trList.Controls.Add(tcList);

				db1.objDataReader.Close();
				db1 = null;

				htList.Controls.Add(trList);
			}
			

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(htList);

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