using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;

public partial class eyeTestInfo : PageBase {

	int EyeTestID = -1;
	int EyeTestInfoID = -1;

	List<Language> langs = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Administrator }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected string createEditLinks(int EyeTestID) {
		if (langs == null) {
			using (var ipa = statics.GetApi()) {
				langs = ipa.LanguageGetCollection();
			}
		}
		string s = "";
		foreach (var lang in langs) { 
			s += "<a href=\"eyeTestInfo.aspx?EyeTestID=" + EyeTestID + "&LangID=" + lang.ID + "\">"
				+ "<img src=\"/img/flag_" + lang.Name + ".png\" alt=\"\" /></a>";
		}

		return s;
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		int.TryParse(VC.RqValue("EyeTestID"), out EyeTestID);
		int.TryParse(VC.RqValue("EyeTestInfoID"), out EyeTestInfoID);

		//using (var ipa = statics.GetApi()) { 
		//   if (!IsPostBack) {
		//      ddlLanguage.DataSource = ipa.LanguageGetCollection().OrderBy(n => n.Name);
		//      ddlLanguage.DataTextField = "Name";
		//      ddlLanguage.DataValueField = "ID";
		//      ddlLanguage.DataBind();
		//   }
		//}
		if (EyeTestID < 1) {
			populateList();
		} else {
			if (EyeTestInfoID > 0)
				populateTextEdit();
			else
				populateEdit();
		}
		//if (CurrentUser.Pud.HasValue(tye.Data.Pud.PudKeys.EyeTestInfoLanguage)) {
		//   ddlLanguage.SelectedValue = CurrentUser.Pud.GetValue(tye.Data.Pud.PudKeys.EyeTestInfoLanguage).ToString();
		//};
	}

	private void populateEdit() {
		plhEyeTestInfos.Controls.Clear();

		litLangLinks.Text = createEditLinks(EyeTestID) + "<br /><br />";

		using (var ipa = statics.GetApi()) {
			
			var langs = ipa.LanguageGetCollection().OrderBy(n => n.Name);
			plhList.Visible = false;
			plhTextTypes.Visible = true;

			int langId = 0;
			//if (!int.TryParse(ddlLanguage.SelectedValue, out langId))
			if(!int.TryParse(VC.RqValue("LangID"), out langId))
				langId = langs.First().ID;

			var infos = ipa.EyeTestInfoGetCollection(EyeTestID, langId);

			lnkAddImportant.Visible = !infos.Any(n => n.InfoType == "Important");
			lnkAddIntro.Visible = !infos.Any(n => n.InfoType == "Intro");
			lnkAddPurpose.Visible = !infos.Any(n => n.InfoType == "Purpose");

			foreach (var info in infos.OrderBy(n => n.InfoType).ThenBy(n => n.Priority)) {
					plhEyeTestInfos.Controls.Add(new LiteralControl("<a href=\"" + VC.QueryStringStrip("EyeTestInfoID") + "EyeTestInfoID=" + info.ID + "\">" + (info.InfoType == "Step" ? info.Priority.ToString() : info.InfoType) + "</a><br />"));
				plhEyeTestInfos.Controls.Add(new LiteralControl("<div class=\"eyeTestInfo\">" + info.InfoText + "</div>"));
			}
			var infoHeading = infos.FirstOrDefault(n => n.InfoType == "Name");
			if (infoHeading != null) {
				var eyetest = ipa.EyeTestGetSingle(EyeTestID);
				litEyeTestName.Text = infoHeading.InfoText;
			}
		}
	}

	private void populateTextEdit() {
		using (var ipa = statics.GetApi()) {
			plhList.Visible = false;
			plhTextTypes.Visible = false;
			plhEditText.Visible = true;

			var info = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
			var eyetest = ipa.EyeTestGetSingle(EyeTestID);
			ckEditor.Text = info.InfoText;
			if(info.InfoType == "Name") {
				tbName.Visible = true;
				ckEditor.Visible = false;
				tbName.Text = info.InfoText;
			}
			lnkCancelEyeTestInfo.NavigateUrl = VC.QueryStringStripNoTrail("EyeTestInfoID");
			litEditTextName.Text = eyetest.InfoValue("Name", info.LanguageID) + " - " + ipa.LanguageGetSingle(info.LanguageID).Name;
		}
	}
	
	protected void eDdlLanguage_SelectedIndexChanged(object sender, EventArgs e) {
		populateEdit();
		using (var ipa = statics.GetApi()) {
			//CurrentUser.Pud.SetValue(tye.Data.Pud.PudKeys.EyeTestInfoLanguage, ddlLanguage.SelectedValue);
			ipa.UserSave(CurrentUser);
		}
	}

	protected void eLnkAddText_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			int intLang = 1;
			//int.TryParse(ddlLanguage.SelectedValue, out intLang);
			int.TryParse(VC.RqValue("LangID"), out intLang);

			var infos = ipa.EyeTestInfoGetCollection(EyeTestID, intLang);
			string senderid = ((LinkButton)sender).ID;

			int topPriority = (infos.Any(n => n.InfoType == "Step") ? infos.Max(n => n.Priority) + 1 : 1);

			var text = new tye.Data.EyeTestInfo() { 
				InfoType = "Step", 
				Priority = topPriority, 
				LanguageID = intLang, 
				ID = 0, 
				EyeTestID = EyeTestID,
			InfoText = ""};

			if (senderid == "lnkAddStep") {
				ipa.EyeTestInfoSave(text);
			} else {
				text.Priority = 0;
				if (senderid.EndsWith("Intro") && !infos.Any(n => n.InfoType == "Intro")) {
					text.InfoType = "Intro";
					ipa.EyeTestInfoSave(text);
				} else if (senderid.EndsWith("Important") && !infos.Any(n => n.InfoType == "Important")) {
					text.InfoType = "Important";
					ipa.EyeTestInfoSave(text);
				} else if (senderid.EndsWith("Purpose") && !infos.Any(n => n.InfoType == "Purpose")) {
					text.InfoType = "Purpose";
					ipa.EyeTestInfoSave(text);
				}
			}
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void elnkDeleteEyeTestInfo_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var text = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
			if (text != null)
				ipa.EyeTestInfoDelete(EyeTestInfoID);
		}
		Response.Redirect(VC.QueryStringStripNoTrail("EyeTestInfoID"));
	}

	protected void eLnkSaveEyeTestInfo_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {

			plhList.Visible = false;
			plhTextTypes.Visible = false;
			plhEditText.Visible = true;

			var info = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
			if (info.InfoType == "Name") {
				info.InfoText = tbName.Text;
			} else {
				info.InfoText = ckEditor.Text;
			}
			ipa.EyeTestInfoSave(info);
		}
		Response.Redirect(VC.QueryStringStripNoTrail("EyeTestInfoID"));
	}

	private void populateList() {
		using (var ipa = statics.GetApi()) {
			repList.DataSource = ipa.EyeTestGetCollection().OrderBy(n => n.Name);
			repList.DataBind();
		}
	}

	
}