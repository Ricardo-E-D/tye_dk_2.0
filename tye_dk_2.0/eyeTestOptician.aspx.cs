using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;

public partial class eyeTestOptician : PageBase
{

    int EyeTestID = -1;
    int EyeTestInfoID = -1;

    List<Language> langs = null;

    private void checkPermissions()
    {
        if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Optician }
            .Contains(CurrentUser.Type))
            Response.Redirect("/");
    }

    protected string createEditLinks(int EyeTestID)
    {
        return string.Empty;

        if (langs == null)
        {
            using (var ipa = statics.GetApi())
            {
                langs = ipa.LanguageGetCollection();
            }
        }
        string s = "";
        foreach (var lang in langs)
        {
            s += "<a href=\"eyeTestInfo.aspx?EyeTestID=" + EyeTestID + "&LangID=" + lang.ID + "\">"
                + "<img src=\"/img/flag_" + lang.Name + ".png\" alt=\"\" /></a>";
        }

        return s;
    }

    /// admin/eyeTestInfo.aspx?EyeTestID=814&LangID=2
    /// admin/eyeTestInfo.aspx?EyeTestID=837&LangID=1&EyeTestInfoID=787
    protected void Page_Init(object sender, EventArgs e)
    {
        checkPermissions();
        int.TryParse(VC.RqValue("EyeTestID"), out EyeTestID);
        int.TryParse(VC.RqValue("EyeTestInfoID"), out EyeTestInfoID);

        if (EyeTestInfoID > 0)
            populateTextEdit();
        else
            populateEdit();
    }

    private void populateEdit()
    {
        plhEyeTestInfos.Controls.Clear();

        //litLangLinks.Text = createEditLinks(EyeTestID) + "<br /><br />";

        using (var ipa = statics.GetApi())
        {

            var langs = ipa.LanguageGetCollection().OrderBy(n => n.Name);
            plhList.Visible = false;
            plhTextTypes.Visible = true;

            int langId = 0;
            if (!int.TryParse(VC.RqValue("LangID"), out langId))
                langId = langs.First().ID;

            EyeTest eyeTest = null;
            if (EyeTestID <= 0)
            { // impossible
                Response.Redirect("/eyeTest.aspx");
            }

            if (EyeTestID > 0)
            {
                eyeTest = ipa.EyeTestGetSingle(EyeTestID);
                if (eyeTest == null)
                {
                    Response.Redirect("/eyeTest.aspx");
                }

                if (!eyeTest.OpticianID.HasValue || eyeTest.OpticianID.Value != CurrentUser.ID)
                {
                    Response.Redirect("/eyeTest.aspx");
                }
            }

            plhAddButtons.Visible = (eyeTest != null);

            var infos = ipa.EyeTestInfoGetCollection(EyeTestID, langId);

            lnkAddImportant.Visible = !infos.Any(n => n.InfoType == "Important");
            lnkAddIntro.Visible = !infos.Any(n => n.InfoType == "Intro");
            lnkAddPurpose.Visible = !infos.Any(n => n.InfoType == "Purpose");

            if (infos.Any())
            {
                foreach (var info in infos.OrderBy(n => n.InfoType).ThenBy(n => n.Priority))
                {
                    plhEyeTestInfos.Controls.Add(new LiteralControl("<a href=\"" + VC.QueryStringStrip("EyeTestInfoID") + "EyeTestInfoID=" + info.ID + "\"><i class=\"fa fa-pencil\"></i>&nbsp;" + (info.InfoType == "Step" ? info.Priority.ToString() : info.InfoType) + "</a><br />"));
                    plhEyeTestInfos.Controls.Add(new LiteralControl("<div class=\"eyeTestInfo\">" + info.InfoText + "</div>"));
                }
                var infoHeading = infos.FirstOrDefault(n => n.InfoType == "Name");
                if (infoHeading != null)
                {
                    var eyetest = ipa.EyeTestGetSingle(EyeTestID);
                    //litEyeTestName.Text = infoHeading.InfoText;
                }
            }
        }
    }

    private void populateTextEdit()
    {
        using (var ipa = statics.GetApi())
        {
            plhList.Visible = false;
            plhTextTypes.Visible = false;
            plhEditText.Visible = true;

            var info = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
            var eyetest = ipa.EyeTestGetSingle(EyeTestID);
            if (eyetest.OpticianID != CurrentUser.ID)
            {
                Response.Redirect("/");
            }

            ckEditor.Text = info.InfoText;
            if (info.InfoType == "Name")
            {
                tbName.Visible = true;
                ckEditor.Visible = false;
                tbName.Text = info.InfoText;
            }
            lnkCancelEyeTestInfo.NavigateUrl = "eyeTest.aspx";
            litEditTextName.Text = eyetest.InfoValue("Name", info.LanguageID) + " - " + info.InfoType; // + " - " + ipa.LanguageGetSingle(info.LanguageID).Name;
        }
    }

    protected void eDdlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateEdit();
        using (var ipa = statics.GetApi())
        {
            //CurrentUser.Pud.SetValue(tye.Data.Pud.PudKeys.EyeTestInfoLanguage, ddlLanguage.SelectedValue);
            ipa.UserSave(CurrentUser);
        }
    }

    protected void eLnkAddText_Click(object sender, EventArgs e)
    {
        using (var ipa = statics.GetApi())
        {
            int intLang = 1;
            //int.TryParse(ddlLanguage.SelectedValue, out intLang);
            int.TryParse(VC.RqValue("LangID"), out intLang);
            var langs = ipa.LanguageGetCollection();
            if (intLang == 0)
                intLang = langs.First().ID;

            EyeTest eyetTest = ipa.EyeTestGetSingle(EyeTestID);
            if (eyetTest != null && eyetTest.OpticianID != CurrentUser.ID)
            {
                Response.Redirect("/");
            }
            var infos = ipa.EyeTestInfoGetCollection(EyeTestID);
            string senderid = ((LinkButton)sender).ID;

            int topPriority = (infos.Any(n => n.InfoType == "Step") ? infos.Max(n => n.Priority) + 1 : 1);


            foreach (var lang in langs)
            {
                var text = new tye.Data.EyeTestInfo()
                {
                    InfoType = "Step",
                    Priority = topPriority,
                    LanguageID = lang.ID,
                    ID = 0,
                    EyeTestID = EyeTestID,
                    InfoText = ""
                };

                if (senderid == "lnkAddStep")
                {
                    ipa.EyeTestInfoSave(text);
                }
                else
                {
                    text.Priority = 0;
                    if (senderid.EndsWith("Intro") && !infos.Any(n => n.InfoType == "Intro"))
                    {
                        text.InfoType = "Intro";
                        ipa.EyeTestInfoSave(text);
                    }
                    else if (senderid.EndsWith("Important") && !infos.Any(n => n.InfoType == "Important"))
                    {
                        text.InfoType = "Important";
                        ipa.EyeTestInfoSave(text);
                    }
                    else if (senderid.EndsWith("Purpose") && !infos.Any(n => n.InfoType == "Purpose"))
                    {
                        text.InfoType = "Purpose";
                        ipa.EyeTestInfoSave(text);
                    }
                }
            }
        }
        Response.Redirect(VC.QueryStringStripNoTrail(""));
    }

    protected void elnkDeleteEyeTestInfo_Click(object sender, EventArgs e)
    {
        using (var ipa = statics.GetApi())
        {
            var text = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
            if (text == null)
            {
                Response.Redirect(VC.QueryStringStripNoTrail("EyeTestInfoID"));
            }
            var eyetest = ipa.EyeTestGetSingle(text.EyeTestID);
            if (eyetest != null && eyetest.OpticianID == CurrentUser.ID)
            {
                // must delete text for every language for current eye test
                var deletes = ipa.EyeTestInfoGetCollection(text.EyeTestID);
                foreach (var entity in deletes.Where(m => m.InfoType == text.InfoType))
                {
                    ipa.EyeTestInfoDelete(entity.ID);
                }
            }
        }
        Response.Redirect(VC.QueryStringStripNoTrail("EyeTestInfoID"));
    }

    protected void eLnkSaveEyeTestInfo_Click(object sender, EventArgs e)
    {
        using (var ipa = statics.GetApi())
        {

            plhList.Visible = false;
            plhTextTypes.Visible = false;
            plhEditText.Visible = true;
            var eyetest = ipa.EyeTestGetSingle(EyeTestID);
            if (eyetest.OpticianID != CurrentUser.ID)
            {
                Response.Redirect("/");
            }

            var info = ipa.EyeTestInfoGetSingle(EyeTestInfoID);
            var texts = ipa.EyeTestInfoGetCollection(info.EyeTestID);
            langs = ipa.LanguageGetCollection();

            foreach (var lang in langs)
            {
                var text = texts.FirstOrDefault(m => m.LanguageID == lang.ID && m.InfoType == info.InfoType && m.Priority == info.Priority);
                if (text != null) {
                    if (info.InfoType == "Name")
                    {
                        text.InfoText = tbName.Text;
                    }
                    else
                    {
                        text.InfoText = ckEditor.Text;
                    }
                    ipa.EyeTestInfoSave(text);
                }
            }
        }
        Response.Redirect(VC.QueryStringStripNoTrail("EyeTestInfoID"));
    }

    private void populateList()
    {
        using (var ipa = statics.GetApi())
        {
            repList.DataSource = ipa.EyeTestGetCollection().OrderBy(n => n.Name);
            repList.DataBind();
        }
    }


}