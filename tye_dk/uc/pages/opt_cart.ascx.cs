namespace tye.uc.pages
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Mail;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;

	public partial class opt_cart : uc_pages
	{
		Hashtable htTb = new Hashtable();
		string[][] arrInfos = new string[5][];
		int lId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strMode = Request.QueryString["mode"];
			int intId = Convert.ToInt32(Request.QueryString["id"]);

			lId = ((tye.Menu)Session["menu"]).IntLanguageId;

			Cart cart = new Cart();
			cart.sessionExists();
			cart = null;
			
			arrInfos[1] = new string[] {"Produkt","Antal","Stk. pris","I alt","Total","Opdater","Bestil","Ingen produkter i kurven.","Din ordre er nu afsendt og du vil snarest modtage en bekræftelse fra TrainYourEyes.com."};
			arrInfos[2] = new string[] {"Produkt","Antal","Stk. pris","I alt","Total","Opdater","Bestil","Ingen produkter i kurven.","Din ordre er nu afsendt og du vil snarest modtage en bekræftelse fra TrainYourEyes.com."};
			arrInfos[3] = new string[] {"Product","Amount","á","Subtotal","Total","Update","Order","No products in the cart.","Your order has been submitted and you will recieve a confirmation from TrainYourEyes.com as soon as possible."};
			arrInfos[4] = new string[] {"Produkt","Betrag","á","Subtotal","Total","Update","Bestellung","Keine Produkte im Einkaufswagen.","Ihre Bestellung wurde aufgenommen. Sie erhalten so schnell wie möglich eine Bestätigung via e-mail von TrainYourEyes.com."};

			switch(strMode)
			{
				case "add":
					add(intId);
					//this.Controls.Add(list());
					break;
				case "confirmed":
					if(Session["noerror"] != null){
						this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
						Session["noerror"] = null;
					}
					break;
				default:
					this.Controls.Add(list());
					break;
			}
		}

		public HtmlTable list(){
			double dblTotal = 0.00;
			double dblSubtotal = 0.00;

			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Style.Add("width","480px");
			htList.Style.Add("border-collapse","collapse");
			htList.Attributes["class"] = "data_table";

            HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","240px");
			tcList.Controls.Add(new LiteralControl(arrInfos[lId][0]));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","60px");
			tcList.Controls.Add(new LiteralControl(arrInfos[lId][1]));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("text-align","right");
			tcList.Style.Add("width","85px");
			tcList.Controls.Add(new LiteralControl(arrInfos[lId][2]));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","95px");
			tcList.Style.Add("text-align","right");
			tcList.Controls.Add(new LiteralControl(arrInfos[lId][3]));

			trList.Controls.Add(tcList);
			htList.Controls.Add(trList);
			
			// display cart contents
			if(((Cart)Session["cart"]).htItems.Count > 0){
				int[] arrItemsK = new int[((Cart)Session["cart"]).htItems.Count];
				int[] arrItemsV = new int[((Cart)Session["cart"]).htItems.Count];
				((Cart)Session["cart"]).htItems.Keys.CopyTo(arrItemsK,0);
				((Cart)Session["cart"]).htItems.Values.CopyTo(arrItemsV,0);

				htTb = new Hashtable();

				for(int i = 0;i <= arrItemsK.GetUpperBound(0);i++){
					htTb.Add(arrItemsK[i],new TextBox());
					((TextBox)htTb[arrItemsK[i]]).Style.Add("width","33px");
					((TextBox)htTb[arrItemsK[i]]).MaxLength = 4;
				}

				string fName = "eName_" + Shared.LanguageAbbr(lId);
				string eDesc = "eiDescription_" + Shared.LanguageAbbr(lId);
				string ePrice = "eiPrice_" + Shared.LanguageAbbr(lId);

				for(int i = 0;i <= arrItemsK.GetUpperBound(0);i++){
					Database db = new Database();
					//string strSql = "SELECT equipment_items.id,name,price FROM equipment_items INNER JOIN equipment ON equipment.id = equipmentid WHERE equipment_items.id ="+ arrItemsK[i];
					string strSql = "SELECT " + fName + ", " + eDesc + ", " + ePrice + " FROM newEquipment" +
									" INNER JOIN newEquipmentItem ON eiEquipment = eID " +
									" WHERE eiID = " + arrItemsK[i];
					MySqlDataReader objDr = db.select(strSql);

					if(objDr.Read()){
						dblSubtotal = 0.00;

						trList = new HtmlTableRow();

						// name and description
						tcList = new HtmlTableCell();
						tcList.Attributes["class"] = "data_table_item";
						tcList.InnerHtml = objDr.GetString(objDr.GetOrdinal(fName)) + "<br /><span style=\"font-size:10px;color:#444;\">" + objDr.GetString(objDr.GetOrdinal(eDesc)) + "</span>"; //objDr["name"].ToString();
						trList.Controls.Add(tcList);

						tcList = new HtmlTableCell();
						tcList.Attributes["class"] = "data_table_item";
						((TextBox)htTb[arrItemsK[i]]).Text = arrItemsV[i].ToString();
						tcList.Controls.Add(((TextBox)htTb[arrItemsK[i]]));
						trList.Controls.Add(tcList);

						tcList = new HtmlTableCell();
						tcList.Style.Add("text-align","right");
						tcList.Attributes["class"] = "data_table_item";
						//tcList.InnerHtml = ((Cart)Session["cart"]).checkDecimals(Convert.ToDouble(objDr["price"]));
						tcList.InnerHtml = ((Cart)Session["cart"]).checkDecimals(objDr.GetDouble(objDr.GetOrdinal(ePrice)));
						trList.Controls.Add(tcList);

						tcList = new HtmlTableCell();
						tcList.Attributes["class"] = "data_table_item";
						tcList.Style.Add("text-align","right");
						//dblSubtotal = Convert.ToDouble(objDr["price"]) * Convert.ToDouble(((TextBox)htTb[arrItemsK[i]]).Text);
						dblSubtotal = objDr.GetDouble(objDr.GetOrdinal(ePrice)) * Convert.ToDouble(((TextBox)htTb[arrItemsK[i]]).Text);
						dblTotal += dblSubtotal;
                        tcList.InnerHtml = ((Cart)Session["cart"]).checkDecimals(dblSubtotal);
						trList.Controls.Add(tcList);
						
						htList.Controls.Add(trList);						
					}
					db.objDataReader.Close();
					db = null;
				}

				trList = new HtmlTableRow();

				tcList = new HtmlTableCell();
				tcList.ColSpan = 3;
				tcList.Style.Add("text-align","right");
				tcList.Attributes["class"] = "data_table_item";
				tcList.InnerHtml = "<span class='bold_text'>" + arrInfos[lId][4] + "</span>";
				trList.Controls.Add(tcList);
				
				tcList = new HtmlTableCell();
				tcList.Style.Add("text-align","right");
				tcList.Attributes["class"] = "data_table_item";
				tcList.InnerHtml = ((Cart)Session["cart"]).checkDecimals(Convert.ToDouble(dblTotal));
				trList.Controls.Add(tcList);

				htList.Controls.Add(trList);

				trList = new HtmlTableRow();

				tcList = new HtmlTableCell();
				tcList.ColSpan = 4;
				tcList.Attributes["class"] = "data_table_item";
					Button btnUpdate = new Button();
					btnUpdate.Text = arrInfos[lId][5].ToString();
					btnUpdate.Style.Add("width","100%");
					btnUpdate.Style.Add("margin-top","15px");
					btnUpdate.Click +=new EventHandler(updateCart);
				tcList.Controls.Add(btnUpdate);
				trList.Controls.Add(tcList);
				
				htList.Controls.Add(trList);

				trList = new HtmlTableRow();

				tcList = new HtmlTableCell();
				tcList.ColSpan = 4;
				tcList.Attributes["class"] = "data_table_item";
					Button btnSubmit = new Button();
					btnSubmit.Text = arrInfos[lId][6].ToString();
					btnSubmit.Style.Add("width","100%");
					btnSubmit.Style.Add("margin-top","15px");
					btnSubmit.Click +=new EventHandler(submitCart);
				tcList.Controls.Add(btnSubmit);
				trList.Controls.Add(tcList);
				
				htList.Controls.Add(trList);
			}
			else{
				trList = new HtmlTableRow();

				tcList = new HtmlTableCell();
				tcList.ColSpan = 4;
				tcList.Attributes["class"] = "data_table_item";
				tcList.InnerHtml = arrInfos[lId][7];

				trList.Controls.Add(tcList);
				htList.Controls.Add(trList);
			}
			return htList;
		}

		public void submit(){

		}

		private void updateCart(object sender, EventArgs e) {
			Hashtable htNewItems = new Hashtable();
			
			int[] arrItemsK = new int[((Cart)Session["cart"]).htItems.Count];
			((Cart)Session["cart"]).htItems.Keys.CopyTo(arrItemsK,0);
			
			for(int i = 0;i <= arrItemsK.GetUpperBound(0);i++){
				htNewItems.Add(arrItemsK[i],Convert.ToInt32(((TextBox)htTb[arrItemsK[i]]).Text));
			}

			((Cart)Session["cart"]).update(htNewItems);

			Response.Redirect("?page="+IntPageId+"&submenu="+IntSubmenuId);
		}

		private void submitCart(object sender, EventArgs e) {
			MailMessage objMail = new MailMessage();

			//objMail.To = "maria@trainyoureyes.com";
			objMail.To = Shared.ShopEmails[((User)Session["user"]).IntLanguageId - 1];
			//objMail.Subject = "Bestilling fra TYE.com";
			objMail.Subject = "Order from TrainYourEyes.com";
			objMail.Body = ((Cart)Session["cart"]).getMailList();
			objMail.From = "noreply@trainyoureyes.com";
			objMail.BodyFormat = MailFormat.Text;
			//SmtpMail.SmtpServer = "websmtp.hardball.nu";
            SmtpMail.SmtpServer = "localhost";
	
			SmtpMail.Send(objMail);
			objMail = null;

			Session["cart"] = null;
			Session["noerror"] = "<div id='noerror'>" + arrInfos[lId][8].ToString() + "</div>";

			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=confirmed");
		}
		public void add(int intId){
			//Findes produktet i hash'en?
			if(((Cart)Session["cart"]).htItems.ContainsKey(intId)){
				((Cart)Session["cart"]).htItems[intId] = Convert.ToInt32(((Cart)Session["cart"]).htItems[intId]) + 1;
			}else{
				((Cart)Session["cart"]).htItems.Add(intId,1);
			}			
			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId);
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
