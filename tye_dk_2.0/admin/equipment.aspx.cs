using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;
using CKEditor.NET;

public partial class equipment : PageBase {

	int EquipmentID = -1;
	int EquipmentItemID = -1;
	List<Language> langs = new List<Language>();
	bool blnShowItems = false;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Administrator }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}
	
	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		int.TryParse(VC.RqValue("EquipmentID"), out EquipmentID);
		int.TryParse(VC.RqValue("EquipmentItemID"), out EquipmentItemID);
		blnShowItems = (VC.RqValue("view") == "items");

		using (var ipa = statics.GetApi()) {
			if (EquipmentID < 1 && EquipmentItemID < 1 && VC.RqValue("EquipmentID") != "0")
				populateList();
			else {
				if ((EquipmentID > 0 || VC.RqValue("EquipmentID") == "0") && !blnShowItems) {
					populateEditEquipment();
				} else if (EquipmentID > 0 && blnShowItems) {
					populateEditEquipmentItemList();
				}
			}
		}
		
	}

	protected void eDdlLanguage_SelectedIndexChanged(object sender, EventArgs e) { 
	
	}

	private void populateList() {
		using (var ipa = statics.GetApi()) {
			repList.DataSource = ipa.EquipmentGetCollection().OrderBy(n => n.Name);
			repList.DataBind();
		}
	}
	
	private void populateEditEquipmentItemList() {
		plhList.Visible = false;
		plhEquipmentItem.Visible = true;
		plhItemTabs.Controls.Clear();

		using (var ipa = statics.GetApi()) {
			var equipment = ipa.EquipmentGetSingle(EquipmentID);
			if (equipment != null) {
				litEquipmentName2.Text = "<a href=\"equipment.aspx?EquipmentID=" + equipment.ID + "\">" + equipment.Name + "</a>";
			}
			var items = ipa.EquipmentItemGetCollection(EquipmentID);
			
			foreach (var lang in ipa.LanguageGetCollection()) {
				litItemTabs.Text += "<li>" + lang.Name + "</li>";

				Panel pnlText = new Panel() { CssClass = "tabPanel" };
				plhItemTabs.Controls.Add(pnlText);

				foreach (var item in items) {
					TextBox ck = new TextBox() { TextMode = TextBoxMode.MultiLine };
					ck.ID = "t" + lang.ID + item.ID;

					monosolutions.Controls.NumericTextBox tbPrice = new monosolutions.Controls.NumericTextBox();
					tbPrice.ID = "price" + lang.ID + item.ID;
					tbPrice.MaxLength = 50;
					tbPrice.NumberType = monosolutions.Controls.NumericTextBox.NumericType.Decimal;
					tbPrice.MinValue = 0;
					tbPrice.MaxValue = Int32.MaxValue / 2;

					var info = item.Infos.FirstOrDefault(n => n.LanguageID == lang.ID);
					if (info != null) {
						ck.Text = Server.HtmlDecode(info.Description);
						tbPrice.Text = info.Price.ToString();
					}

					Panel pnlItemID = new Panel() { CssClass = "fieldLabel" };
					pnlItemID.Controls.Add(new LiteralControl("<strong>ID " + item.ID + "</strong>"));
					LinkButton lnkDelVariant = new LinkButton();
					lnkDelVariant.ID = "lnkDelVar" + item.ID + lang.ID;
					lnkDelVariant.CommandArgument = item.ID.ToString();
					lnkDelVariant.Text = "Delete variant (all languages)";
					lnkDelVariant.CssClass = "negativesmall";
					lnkDelVariant.Click += new EventHandler(lnkDelVariant_Click);
					lnkDelVariant.OnClientClick = "return confirm(tye.dicValue('confirm_delete'))";
					pnlItemID.Controls.Add(lnkDelVariant);

					pnlText.Controls.Add(pnlItemID);
					pnlText.Controls.Add(new LiteralControl("<div class=\"fieldLabel\">" + DicValue("price") + "</div>"));
					pnlText.Controls.Add(tbPrice);
					pnlText.Controls.Add(new LiteralControl("<div class=\"fieldLabel\">" + DicValue("description") + "</div>"));
					pnlText.Controls.Add(ck);

					pnlText.Controls.Add(new LiteralControl("<br /><br />"));

					
				} // foreach
				plhItemTabs.Controls.Add(pnlText);
			} // foreach
		} // using
	}

	void lnkDelVariant_Click(object sender, EventArgs e) {
		int iTry = 0;
		int.TryParse(((LinkButton)sender).CommandArgument, out iTry);

		using (var ipa = statics.GetApi()) {
			ipa.EquipmentItemDelete(iTry);
		}

		Response.Redirect(VC.QueryStringStripNoTrail(""));

	} // method

	private void populateEditEquipment() {
		plhEditText.Visible = true;
		
		using(var ipa = statics.GetApi()) {

			var quip = ipa.EquipmentGetSingle(EquipmentID);
			if (quip != null) { 
				chkActive.Checked = quip.Active;
				tbInternalName.Text = quip.Name;
			}
			
			foreach (var lang in ipa.LanguageGetCollection()) {
				litTabs.Text += "<li>" + lang.Name + "</li>";

				Panel pnlText = new Panel() {  CssClass="tabPanel"};
				plhTabs.Controls.Add(pnlText);

				CKEditorControl ck = new CKEditorControl();
				ck.ID = "t" + lang.ID;

				TextBox tbName = new TextBox();
				tbName.ID = "name" + lang.ID;
				tbName.MaxLength = 50;

				if (quip != null) {
					var info = quip.Infos.FirstOrDefault(n => n.LanguageID == lang.ID);
					if (info != null) {
						ck.Text = info.Description;
						tbName.Text = info.Name;
					}
				}
				pnlText.Controls.Add(new LiteralControl("<div class=\"fieldLabel\">" + DicValue("name") + "</div>"));
				pnlText.Controls.Add(tbName);

				pnlText.Controls.Add(new LiteralControl("<br /><br /><div class=\"fieldLabel\">" + DicValue("description") + " " + lang.Name + "</div>"));
				pnlText.Controls.Add(ck);

				pnlText.Controls.Add(new LiteralControl("<br /><br />"));

			}
			if (quip != null) {
				litEquipmentName.Text = quip.Name + " - <a href=\"equipment.aspx?EquipmentID=" + quip.ID + "&view=items\">Goto variants</a>";
			} else {
				litEquipmentName.Text = "New...";
			}
		}
	}

	protected void lnkDeleteEquipment_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {

			int id = 0;
			int.TryParse(((LinkButton)sender).CommandArgument, out id);

			ipa.EquipmentDelete(id);
		}
		Response.Redirect("equipment.aspx");
	}

	protected void eLnkEquipmentItemSave_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var items = ipa.EquipmentItemGetCollection(EquipmentID);
			
			foreach (var lang in ipa.LanguageGetCollection()) {
				foreach (var item in items) {
					TextBox ck = (TextBox)plhItemTabs.FindControl("t" + lang.ID + item.ID);
					monosolutions.Controls.NumericTextBox tbPrice = (monosolutions.Controls.NumericTextBox)plhItemTabs.FindControl("price" + lang.ID + item.ID);

					EquipmentItemInfo info = item.Infos.FirstOrDefault(n => n.LanguageID == lang.ID);
					if (info == null)
						info = new EquipmentItemInfo() { EquipmentItemID = item.ID, ID = 0, LanguageID = lang.ID, Price = 0, Description = "" };

					if (ck != null)
						info.Description = ck.Text;

					if (tbPrice != null) {
						double dblTry = 0;
						double.TryParse(tbPrice.Text.Replace(".", ","), out dblTry);
						info.Price = dblTry;
					}

					ipa.EquipmentItemInfoSave(info);
					
				} // foreach
			} // foreach
		} // using

		if(((LinkButton)sender).ID != "lnkSave")
			Response.Redirect("equipment.aspx");
	}

	protected void eLnkEquipmentSave_Click(object sender, EventArgs e) {
		using(var ipa = statics.GetApi()) {

			Equipment quip = new Equipment() { ID = 0, Picture = "" }; 
			
			if(EquipmentID > 0)
				quip = ipa.EquipmentGetSingle(EquipmentID);

			quip.Active = chkActive.Checked;
			quip.Name = tbInternalName.Text;
			quip = ipa.EquipmentSave(quip);

			foreach (var lang in ipa.LanguageGetCollection()) {

				CKEditorControl ck = (CKEditorControl)plhTabs.FindControl("t" + lang.ID);
				TextBox tbName = (TextBox)plhTabs.FindControl("name" + lang.ID);

				var info = quip.Infos.FirstOrDefault(n => n.LanguageID == lang.ID);
				if (info == null) {
					info = new EquipmentInfo() { ID = 0, EquipmentID = quip.ID, LanguageID = lang.ID };
				}
				if(ck != null)
					info.Description = ck.Text;
				if(tbName != null)
					info.Name = tbName.Text;

				ipa.EquipmentInfoSave(info);
			}
		}
		if(((LinkButton)sender).ID == "lnkEquipentSaveAndClose")
			Response.Redirect("equipment.aspx");
	}

	protected void eLnkAddVariant_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var n = new EquipmentItem() { ID = 0, Active = true, EquipmentID = EquipmentID };
			ipa.EquipmentItemSave(n);
		}
		Response.Redirect(VC.QueryStringStrip(""));
	}
	
	
}