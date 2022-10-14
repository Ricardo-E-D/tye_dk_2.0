using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using tye.Data;
using monosolutions.Utils;
using monosolutions.Controls;
using System.Web.Hosting;

public partial class equipmentShop : PageBase {

	int EquipmentID = 0;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician}
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}
	
	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		if (int.TryParse(VC.RqValue("EquipmentID"), out EquipmentID) && EquipmentID > 0)
			populateVariants();
		else
			populateList();

		var cart = new ShoppingCart();
		litShoppingCart.Text = cart.GetCartTotalItems() + " " + DicValue("cartItem").ToLower() + ": " + cart.GetCartTotalPrice(CurrentLanguage).ToString();

		if (VC.RqValue("action") == "showcart") {
			ElnkShowCart_Click(lnkShowCart, new EventArgs());
		}
	}

	private string getName(Equipment quip) {
		var info = quip.Infos.FirstOrDefault(n => n.LanguageID == CurrentLanguage.ID);
		if (info != null)
			return info.Name;
		else
			return "";
	}
	private string getDescription(Equipment quip) {
		var info = quip.Infos.FirstOrDefault(n => n.LanguageID == CurrentLanguage.ID);
		if (info != null)
			return info.Description;
		else
			return "";
	}
	private double getItemPrice(EquipmentItem quip) {
		var info = quip.Infos.FirstOrDefault(n => n.LanguageID == CurrentLanguage.ID);
		if (info != null)
			return info.Price;
		else
			return 0.0;
	}
	private string getItemDescription(EquipmentItem quip) {
		var info = quip.Infos.FirstOrDefault(n => n.LanguageID == CurrentLanguage.ID);
		if (info != null)
			return info.Description;
		else
			return "";
	}
	
	private void populateList() {
		using (var ipa = statics.GetApi()) {
			var items = 
			ipa.EquipmentGetCollection()
				.Where(n => n.Active)
				.Select(n => new {
					ID = n.ID,
					Name = getName(n),
					Description = getDescription(n)
				})
				.Where(n => !String.IsNullOrEmpty(n.Name));
			repItems.DataSource = items.OrderBy(n => n.Name);
			repItems.DataBind();
		}
	}

	private void populateVariants() {
		plhItems.Visible = false;
		plhVariants.Visible = true;
		using (var ipa = statics.GetApi()) {
			var info = ipa.EquipmentGetSingle(EquipmentID).Infos.FirstOrDefault(n => n.LanguageID == CurrentLanguage.ID);
			if (info != null) {
				litEquipmentName.Text = info.Name;
			}
			repVariants.DataSource = ipa.EquipmentItemGetCollection(EquipmentID)
				.Where(n => n.Active).Select(n => new {
				ID = n.ID,
				Price = getItemPrice(n),
				Description = getItemDescription(n)
			});
			repVariants.DataBind();
		}
	}

	protected void ElnkAddToCart_Click(object sender, EventArgs e) {
		int VariantID = 0;

		int.TryParse(((LinkButton)sender).CommandArgument, out VariantID);

		using (var ipa = statics.GetApi()) {
			var item = ipa.EquipmentItemGetSingle(VariantID);
			if (item != null) {
				var ntb = (NumericTextBox)(((WebControl)sender).Parent.FindControl("ntbQuantity"));
				int iQuantity = 1;
				if(ntb == null || !int.TryParse(ntb.Text, out iQuantity))
					iQuantity = 1;

				var sc = new ShoppingCart();
				sc.AddEquipment(item, iQuantity);
			}
		}

		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void ElnkShowCart_Click(object sender, EventArgs e) {
		plhCart.Visible = true;
		plhItems.Visible = false;
		plhVariants.Visible = false;

		var cart = new ShoppingCart();
		repCart.DataSource = cart.DescribeCart(CurrentLanguage);
		repCart.DataBind();

		litCartTotal.Text = DicValue("cartTotal") + " " + cart.GetCartTotalPrice(CurrentLanguage);

		if (cart.DescribeCart(CurrentLanguage).Count == 0) {
			litCartTotal.Text = "<strong>" + DicValue("cartIsEmpty") + "</strong>";
			repCart.Visible = false;
			lnkCompletePurchase.Visible = false;
			lnkClearShoppingCart.Visible = false;
			TransLit6.Visible = false;
			tbOrderNote.Visible = false;
		}
		//Response.Redirect(VC.QueryStringStripNoTrail(""));
	}
	
	protected void ElnkClearShoppingCart_Click(object sender, EventArgs e) {
		(new ShoppingCart()).Clear();
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void ElnkOneLessMore_Click(object sender, EventArgs e) {

		var cart = new ShoppingCart();
		int equipmentid = 0;

		if (!int.TryParse(((LinkButton)sender).CommandArgument, out equipmentid) || equipmentid < 1)
			return;

		if(((LinkButton)sender).ID == "lnkOneLess")
			cart.OneLess(equipmentid);
		else
			cart.OneMore(equipmentid);

		Response.Redirect(VC.QueryStringStrip("action") + "action=showcart");
	}

	protected void ElnkDelCartItem_Click(object sender, EventArgs e) {
		var cart = new ShoppingCart();
		int equipmentid = 0;

		if (!int.TryParse(((LinkButton)sender).CommandArgument, out equipmentid) || equipmentid < 1)
			return;

		cart.RemoveEquipment(equipmentid);

		Response.Redirect(VC.QueryStringStrip("action") + "action=showcart");
	}


	private string getValue(object o, string PropertyName) {
		return o.GetType().GetProperty(PropertyName).GetValue(o, null).ToString();
	}

	protected void ElnkCompletePurchase_Click(object sender, EventArgs e) {
		var cart = new ShoppingCart();
		string mailtemplate = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/mailtemplate/default.htm"));

		string contents = "<table>";
		contents += ("<thead><tr><th>" + DicValue("description") + "</th>" +
		"<th>" + DicValue("price") + "</th><th>" + DicValue("quantity") + "</th>" +
		"<th>" + DicValue("subTotal") + "</th></tr></thead>");

		contents += "<tbody>";
		foreach (var item in cart.DescribeCart(CurrentLanguage)) {
			contents += "<tr><td>" + getValue(item, "EquipmentName") + "<br /><div style=\"font-size:90%;color:#696969;\">" + getValue(item, "EquipmentItemDescription") + "</div></td>" +
			"<td>" + getValue(item, "EquipmentPrice") + "</td><td>" + getValue(item, "Quantity") + "</td><td>" + getValue(item, "SubTotal") + "</td></tr>";
		}
		
		contents += "<tr><td colspan=\"5\"><br /><br />" + DicValue("cartTotal") + " " + cart.GetCartTotalPrice(CurrentLanguage) + "</td></tr>";
		contents += "</tbody></table>";

		if (!String.IsNullOrEmpty(tbOrderNote.Text))
			contents += "<br /><br /><strong>" + DicValue("remarks") + "</strong><br />" + HttpUtility.HtmlEncode(tbOrderNote.Text);

		string mailToOptician = mailtemplate
			.Replace("{heading}", DicValue("equipment") + " - " + DicValue("order"))
			.Replace("{content}", contents);

		
		statics.SendEmail(CurrentUser.Email, DicValue("equipment") + " - " + DicValue("order"), mailToOptician);
		

		// maria - Danish
		contents = CurrentUser.FullName + "<br />"
			+ CurrentUser.Address + "<br />"
			+ CurrentUser.PostalCode + "<br />"
			+ CurrentUser.City + "<br />"
			+ (String.IsNullOrEmpty(CurrentUser.State) ? "" : CurrentUser.State + "<br />")
			+ CurrentUser.Country.Name + "<br />"
			+ CurrentUser.Email + "<br />"
			+ CurrentUser.Phone + "<br />"
			+ CurrentUser.MobilePhone + "<br /><br /><br />"
			;
		contents += "<table>";
		contents += ("<thead><tr><th>" + Dictionary.GetValueByID("description", 1) + "</th>" +
		"<th>" + Dictionary.GetValueByID("price", 1) + "</th><th>" + Dictionary.GetValueByID("quantity", 1) + "</th>" +
		"<th>" + Dictionary.GetValueByID("subTotal", 1) + "</th></tr></thead>");

		contents += "<tbody>";
		foreach (var item in cart.DescribeCart(new Language() { ID = 1, Name = "Danish" })) {
			contents += "<tr><td>" + getValue(item, "EquipmentName") + "<br /><div style=\"font-size:90%;color:#696969;\">" + getValue(item, "EquipmentItemDescription") + "</div></td>" +
			"<td>" + getValue(item, "EquipmentPrice") + "</td><td>" + getValue(item, "Quantity") + "</td><td>" + getValue(item, "SubTotal") + "</td></tr>";
		}

		contents += "<tr><td colspan=\"5\"><br /><br />" + Dictionary.GetValueByID("cartTotal", 1) + " " + cart.GetCartTotalPrice(CurrentLanguage) + " (" + CurrentUser.Language.Name + ")</td></tr>";
		contents += "</tbody></table>";

		if (!String.IsNullOrEmpty(tbOrderNote.Text))
			contents += "<br /><br /><strong>" + Dictionary.GetValueByID("remarks", 1) + "</strong><br />" + HttpUtility.HtmlEncode(tbOrderNote.Text);

		string mailToMaria = mailtemplate
			.Replace("{heading}", Dictionary.GetValueByID("equipment", 1) + " - " + Dictionary.GetValueByID("order", 1))
			.Replace("{content}", contents);

		// todo: change recipient
		//statics.SendEmail("maria@trainyoureyes.com",
		//   Dictionary.GetValueByID("equipment", 1) + " - " + Dictionary.GetValueByID("order", 1),
		//   mailToMaria);

		statics.SendEmail("maria@trainyoureyes.com",
			Dictionary.GetValueByID("equipment", 1) + " - " + Dictionary.GetValueByID("order", 1), 
			mailToMaria);


		// also send mail to...others

		switch (CurrentUser.CountryID) { 
			case 2: // Norway
				statics.SendEmail("norge@trainyoureyes.com", DicValue("equipment") + " - " + DicValue("order"), mailToOptician);
				break;
			case 3: // Germany
				statics.SendEmail("deutschland@trainyoureyes.com", DicValue("equipment") + " - " + DicValue("order"), mailToOptician);
				break;
		}

		cart.Clear();
		Response.Redirect("equipmentShop.aspx");
	}

}