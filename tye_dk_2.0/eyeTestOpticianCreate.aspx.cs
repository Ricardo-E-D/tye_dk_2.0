using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;

public partial class eyeTestOpticianCreate : PageBase
{
	List<Language> langs = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Optician }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

    /// admin/eyeTestInfo.aspx?EyeTestID=814&LangID=2
    /// admin/eyeTestInfo.aspx?EyeTestID=837&LangID=1&EyeTestInfoID=787
	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
	}

    protected void eBtnNameSubmit_Click(object sender, EventArgs e)
    {
        using (var ipa = statics.GetApi())
        {
            var langs = ipa.LanguageGetCollection();
            EyeTest eyeTest = null;

            if (string.IsNullOrEmpty(tbEyeTestName.Text)) {
                tbEyeTestName.Text = "MUST ENTER NAME!";
                return;
            }

            if (tbEyeTestName.Text.Length > 50)
            {
                tbEyeTestName.Text = tbEyeTestName.Text.Substring(0, 49);
            }

            if (eyeTest == null)
            {
                int max = ipa.EyeTestGetCollection().OrderByDescending(m => m.ID).First().ID + 1;
                var newTest = ipa.EyeTestSave(new EyeTest()
                {
                    HighscoreApplicable = false,
                    ID = 0,
                    InternalName = "optTest_" + CurrentUser.ID + "_" + max,
                    OldBbName = "optTest_" + CurrentUser.ID + "_" + max,
                    Name = tbEyeTestName.Text,
                    OpticianID = CurrentUser.ID,
                    Priority = 900,
                    ScoreRequired = 0,
                    ScreenTest = false
                });

                
                foreach (var lang in langs) {
                    ipa.EyeTestInfoSave(new EyeTestInfo()
                    {
                        EyeTestID = newTest.ID,
                        InfoType = "Name",
                        InfoText = tbEyeTestName.Text,
                        LanguageID = lang.ID,
                        ID = 0
                    });
                }
                Response.Redirect("eyeTestOptician.aspx?EyeTestID=" + newTest.ID);
            }
        }
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}
	
}