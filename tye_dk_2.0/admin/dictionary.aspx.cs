using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using tye.Data;
using monosolutions.Utils;

public partial class admin_dictionary : PageBase {

	List<Language> languages = new List<Language>();
	int EditID = -1;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA, tye.Data.User.UserType.Administrator }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}


	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		Form.DefaultFocus = "inpQuickFind";
		
		
		using (var ipa = statics.GetApi()) {
			languages = ipa.LanguageGetCollection();

			if (int.TryParse(VC.RqValue("ID"), out EditID))
				populateEdit();
			else {
				TableHeaderRow trHead = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };
				trHead.Cells.Add(new TableHeaderCell() { Text = "Key" });
				foreach (var lang in languages) {
					trHead.Cells.Add(new TableHeaderCell() { Text = lang.Name });
				}
				trHead.Cells.Add(new TableHeaderCell() { Text = "ClientSide" });
				tblEntries.Rows.Add(trHead);

				foreach (var entry in ipa.DictionaryGet().Entries) {
					TableRow tr = new TableRow() {  CssClass = "link"};
					tr.Attributes.Add("onclick", "window.location.href='?ID=" + entry.ID + "'");
					tr.Cells.Add(new TableCell() { Text = entry.Key });
					
					foreach (var lang in languages) {
						string txt = entry.GetValue(lang);
						if (string.IsNullOrEmpty(txt))
							txt = "!MISSING!";
						tr.Cells.Add(new TableCell() { Text = txt});	
					}

					tr.Cells.Add(new TableCell() { Text = entry.ClientSide.ToString() });

					tblEntries.Rows.Add(tr);
				}

			}
		}
	}

	private void populateEdit() {
		pnlExistingData.Visible = false;
		pnlAddEntry.Visible = true;
		
		Form.DefaultFocus = tbKey.ClientID;

		lnkDelete.Visible = (EditID > 0) && CurrentUser.Type == tye.Data.User.UserType.SBA;
		lnkNew.Visible = false;

		using (var ipa = statics.GetApi()) {
			var entry = ipa.DictionaryEntryGetSingle(EditID);
			if (entry != null)
				tbKey.Text = entry.Key;

			foreach (var lang in languages) {
				plhAddEntry.Controls.Add(new LiteralControl("<div class=\"fieldLabel\">" + lang.Name + "</div>"));
				TextBox tb = new TextBox() { ID = "tbLang" + lang.ID, Text = (entry != null ? entry.GetValue(lang) : ""), TextMode = TextBoxMode.MultiLine, Rows = 3 };
				plhAddEntry.Controls.Add(tb);
			}
			plhAddEntry.Controls.Add(new CheckBox() { ID = "chkClientSide", Text = "ClientSide", Checked = entry.ClientSide, Visible = (CurrentUser.Type == tye.Data.User.UserType.SBA) });
		}
	}

	protected void eBtnSave_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			DictionaryEntry de = new DictionaryEntry();
			if (EditID == 0) { // new
				if (ipa.DictionaryGet().EntryExists(tbKey.Text.Trim()) || String.IsNullOrEmpty(tbKey.Text.Trim())) {
					AddJavascript(String.Format("PIM.showMessage('" + Dictionary.GetValue("admin_error_dictionarySaveKeyInUse", CurrentLanguage) + "', 'error');", tbKey.Text.Trim())); // todo: replace with dictionary entry
					return;
				}
			} else {
				de = ipa.DictionaryEntryGetSingle(EditID);
			}

			de.Key = tbKey.Text.Trim();

			foreach (var lang in languages) {
				TextBox tb = (TextBox)plhAddEntry.FindControl("tbLang" + lang.ID);
				if (tb != null) {
					de.SetValue(lang, tb.Text.Trim());
				}
			}
			de.ClientSide = ((CheckBox)plhAddEntry.FindControl("chkClientSide")).Checked;
			ipa.DictionaryEntrySave(de);
		}

		DictionaryReload();
		Response.Redirect("dictionary.aspx");
	}

	protected void eLnkDelete_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			ipa.DictionaryEntryDelete(EditID);
		}
		Response.Redirect("dictionary.aspx");
	}

}