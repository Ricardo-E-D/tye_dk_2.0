	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using System.Collections;

namespace tye {
	
	public class Cart : System.Web.UI.Page {
		public Hashtable htItems = new Hashtable();
		//public Hashtable htTb = new Hashtable();
        
		public Cart() {
			//sessionExists();
		}

		public void sessionExists(){
			//Findes cart session'en?
			if(Session["cart"] == null){
				Session["cart"] = new Cart();
			}
		}

		public void update(Hashtable htNewItems){
			int[] arrNewItemsK = new int[htNewItems.Count];
			int[] arrNewItemsV = new int[htNewItems.Count];
			htNewItems.Keys.CopyTo(arrNewItemsK,0);
			htNewItems.Values.CopyTo(arrNewItemsV,0);

			for(int i = 0;i <= arrNewItemsK.GetUpperBound(0);i++){
				if(arrNewItemsV[i] == 0){
					((Cart)Session["cart"]).htItems.Remove(arrNewItemsK[i]);
				}else{
					((Cart)Session["cart"]).htItems[arrNewItemsK[i]] = arrNewItemsV[i];
				}
			}

			arrNewItemsK = null;
			arrNewItemsV = null;
			htNewItems = null;
		}
		
		public string checkDecimals(double dblValue){
			string strValue = "";
			
			string[] arrCurr = new string[] {"","DKK","NKK","£","EURO"};

			strValue = arrCurr[((Menu)Session["menu"]).IntLanguageId];
			strValue += " " + dblValue.ToString("c").Replace("kr ","");
			return strValue;
		}
		
		public string getMailList(){
			//string strTemp = "Hej Maria,\n\nDette er en ordre fra TYE.com.\n\nOrdren blev afgivet: "+DateTime.Now.ToString("dd-MM-yyyy HH:mm") +"\n\nOrdren blev afgivet af:\n";
			//strTemp += ((Optician)Session["user"]).StrName +"\n"+((Optician)Session["user"]).StrAddress +"\n"+((Optician)Session["user"]).StrZipCode +" "+((Optician)Session["user"]).StrCity +"\n";
			//strTemp += ((Optician)Session["user"]).StrPhone +"\n"+((Optician)Session["user"]).StrEmail+"\n\n";
			//strTemp += "Følgende varer er bestilt:\n\n";

			string strTemp = "Hi\n\nThis is an order placed through TrainYourEyes.com.\n\nThe order was placed at : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + "\n\nThe order was placed by:\n";
			strTemp += ((Optician)Session["user"]).StrName + "\n" + ((Optician)Session["user"]).StrAddress + "\n" + ((Optician)Session["user"]).StrZipCode + " " + ((Optician)Session["user"]).StrCity + "\n";
			strTemp += ((Optician)Session["user"]).StrPhone + "\n" + ((Optician)Session["user"]).StrEmail + "\n\n";
			strTemp += "The order contains the following items:\n\n";

			int[] arrItemsK = new int[((Cart)Session["cart"]).htItems.Count];
			int[] arrItemsV = new int[((Cart)Session["cart"]).htItems.Count];
			((Cart)Session["cart"]).htItems.Keys.CopyTo(arrItemsK,0);
			((Cart)Session["cart"]).htItems.Values.CopyTo(arrItemsV,0);

			int lId = ((tye.Menu)Session["menu"]).IntLanguageId;
			string fName = "eName_" + Shared.LanguageAbbr(lId);
			string eDesc = "eiDescription_" + Shared.LanguageAbbr(lId);
			string ePrice = "eiPrice_" + Shared.LanguageAbbr(lId);

			double dblTotal = 0.00;

			for(int i = 0;i <= arrItemsK.GetUpperBound(0);i++){
				Database db = new Database();
				//string strSql = "SELECT equipment_items.id,name,price FROM equipment_items INNER JOIN equipment ON equipment.id = equipmentid WHERE equipment_items.id ="+ arrItemsK[i];
				string strSql = "SELECT " + fName + ", " + eDesc + ", " + ePrice + " FROM newEquipment" +
						" INNER JOIN newEquipmentItem ON eiEquipment = eID " +
						" WHERE eiID = " + arrItemsK[i];
				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read()){
					double dblSubtotal = objDr.GetDouble(objDr.GetOrdinal(ePrice)) * Convert.ToDouble(arrItemsV[i]); //Convert.ToDouble(objDr["price"]) * Convert.ToDouble(arrItemsV[i]);
					dblTotal += dblSubtotal;
					//strTemp += arrItemsV[i].ToString() + " pcs. " + objDr["name"].ToString() + " (á " + checkDecimals(Convert.ToDouble(objDr["price"])) + ") subtotal " + checkDecimals(dblSubtotal) + "\n";
					strTemp += arrItemsV[i].ToString() + " pcs. " + objDr.GetString(objDr.GetOrdinal(fName)) + " (á " + checkDecimals(objDr.GetDouble(objDr.GetOrdinal(ePrice))) + ") subtotal " + checkDecimals(dblSubtotal) + "\n";
					strTemp += "(" + Shared.StripHtmlTags(objDr.GetString(objDr.GetOrdinal(eDesc)).Replace("<br />", "\n")) + ")\n";
				}
				db.objDataReader.Close();
                db.dbDispose();
				db = null;
			}

			strTemp += "\nTotal: " + checkDecimals(dblTotal);
			
			strTemp += "\n\nThis is an automatically generated email. You cannot reply to the email.";

			strTemp = strTemp.Replace("&oslash;", "ø").Replace("&Oslash", "Ø");
			strTemp = strTemp.Replace("&aring;", "å").Replace("&Aring", "Å");
			strTemp = strTemp.Replace("&aelig;", "æ").Replace("&Aelig", "Æ");

			return strTemp;
		}
	}
}
