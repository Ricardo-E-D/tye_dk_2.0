// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _default : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
		 if (CurrentUser.Type == tye.Data.User.UserType.SBA) {
			 Response.Redirect("opticians.aspx");
		 }
		 else if (CurrentUser.Type == tye.Data.User.UserType.Administrator) {
			 Response.Redirect("opticians.aspx");
		 }
		 else if (CurrentUser.Type == tye.Data.User.UserType.Optician) {
			 Response.Redirect("clients.aspx");
		 }
		 else if (CurrentUser.Type == tye.Data.User.UserType.Client) {
			 Response.Redirect("clientProgram.aspx");
		 }
		
		 
    }
}