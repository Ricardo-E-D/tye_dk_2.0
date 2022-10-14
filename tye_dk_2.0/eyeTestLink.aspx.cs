using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class eyeTestLink : PageBase {

	public int EyeTestID = 0;
	PropertyMapper PM = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician
		}
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
        pnlError.Visible = false;
        if (!int.TryParse("" + Request.QueryString["EyeTestID"], out EyeTestID)) {
            Response.Redirect("eyeTest.aspx");
        }
        List();
        Page.Title = "Links";
        this.Form.DefaultButton = btnNewLinkSubmit.UniqueID;
	}
	
	private void List() {
        

		using (var ipa = statics.GetApi()) {
			var links = ipa.EyeTestLinkGetCollection(CurrentUser.ID, EyeTestID);
            var eyetest = ipa.EyeTestGetSingle(EyeTestID);
            if (eyetest != null) {
                eyeTestName.Text = eyetest.InfoValue("Name", CurrentUser.LanguageID);
            }

			foreach (var Link in links) {
				TableRow row = new TableRow();

				TableCell tdName = new TableCell();
                TableCell tdLinks = new TableCell();
                TableCell tdDelete = new TableCell();
                tdName.Text = Link.LinkUrl;
                tdLinks.Text = Link.LinkName;

                Button btnDelete = new Button();
                btnDelete.ID = "deleteLink" + Link.ID;
                btnDelete.Text = "Delete Link";
                btnDelete.CommandArgument = Link.ID.ToString();
                btnDelete.CssClass = "negativesmall";
                btnDelete.OnClientClick = "return confirm('Sure?');";
                btnDelete.Click += btnDelete_Click;
                tdDelete.Controls.Add(btnDelete);

				row.Cells.Add(tdName);
                row.Cells.Add(tdLinks);
                row.Cells.Add(tdDelete);

				tblLinks.Rows.Add(row);
			}

		}
	}

    void btnDelete_Click(object sender, EventArgs e)
    {
        var that = (Button)sender;
        var arg = that.CommandArgument;
        int linkID = 0;
        int.TryParse("" + that.CommandArgument, out linkID);

        using (var ipa = statics.GetApi()) {
            var link = ipa.EyeTestLinkGetSingle(linkID);
            if (link != null && link.OpticianID == CurrentUser.ID) {
                ipa.EyeTestLinkDelete(linkID);
            }
        }
        Response.Redirect("eyeTestLink.aspx?EyeTestID=" + EyeTestID);
    }

    protected void btnNewLinkSubmit_Click(object sender, EventArgs e)
    {
        
        if (tbNewLinkUrl.Text.Length == 0)
        {
            litError.Text= "Must enter URL";
            pnlError.Visible = true;
            return;
        }

        if (!monosolutions.Utils.RegExp.IsUrl(tbNewLinkUrl.Text)) {
            pnlError.Visible = true;
            litError.Text = "Please enter valid URL";
            return;
        }
        if (tbNewLinkName.Text.Length == 0)
        {
            litError.Text = "Must enter display name";
            pnlError.Visible = true;
            return;
        }
        using (var ipa = statics.GetApi()) {
            ipa.EyeTestLinkSave(new tye.Data.EyeTestLink() { 
                ID = 0,
                EyeTestID = EyeTestID, 
                OpticianID = CurrentUser.ID,
                LinkUrl = tbNewLinkUrl.Text.Trim(),
                LinkName = tbNewLinkName.Text.Trim()
            });
        }
        Response.Redirect("eyeTestLink.aspx?EyeTestID=" + EyeTestID);
        
    }
}