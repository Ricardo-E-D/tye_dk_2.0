// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using tye.Data;

/// <summary>
/// Summary description for PageBase
/// </summary>
public class PageBase : System.Web.UI.Page
{
	public PageBase()
	{
		
	}
	
	
	protected override void OnLoadComplete(EventArgs e) {
		//string jsDic = "";
	}
	
	public void AddJavascript(string javascript) {
		this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "$(function() { " + javascript + " }); ", true);
	}

	public tye.Data.Language CurrentLanguage {
		get {
			return (Language)this.Page.Master.GetType().GetProperty("CurrentLanguage").GetValue(this.Page.Master, new object[0]);
		}
		set {
			this.Page.Master.GetType().GetProperty("CurrentLanguage").SetValue(this.Page.Master, value, new object[0]);
		}
	}

	public tye.Data.User CurrentBaseUser {
		get {
			return (User)this.Page.Master.GetType().GetProperty("CurrentBaseUser").GetValue(this.Page.Master, new object[0]);
		}
	}

	public tye.Data.User CurrentUser {
		get {
			return (User)this.Page.Master.GetType().GetProperty("CurrentUser").GetValue(this.Page.Master, new object[0]);
		}
		set {
			this.Page.Master.GetType().GetProperty("CurrentUser").SetValue(this.Page.Master, value, new object[0]);
		}
	}

	public bool Impersonating {
		get {
			return (bool)this.Page.Master.GetType().GetProperty("Impersonating").GetValue(this.Page.Master, new object[0]);
		}
		set {
			this.Page.Master.GetType().GetProperty("Impersonating").SetValue(this.Page.Master, value, new object[0]);
		}
	}

	public tye.Data.Dictionary Dictionary {
		get {
			return (Dictionary)this.Page.Master.GetType().GetProperty("Dictionary").GetValue(this.Page.Master, new object[0]);
		}
	}

	public bool CurrentUserIsAdmin() { 
		var cu = CurrentBaseUser;
		return (cu.Type == tye.Data.User.UserType.SBA || cu.Type == tye.Data.User.UserType.Administrator);
	}
	
	/// <summary>
	/// Short-hand equivalent of "Dictionary.GetValue([Key], CurrentLanguage)"
	/// </summary>
	/// <param name="Key"></param>
	/// <returns></returns>
	public string DicValue(string Key) {
		return this.Page.Master.GetType().GetMethod("DicValue").Invoke(this.Page.Master, new object[] { Key }).ToString();
	}

	public void DictionaryReload() {
		this.Page.Master.GetType().GetMethod("DictionaryReload").Invoke(this.Page.Master, new object[0]);
	}

	public string SessionDataValueGet(SessionDataKeys Key) {
		return (string)this.Page.Master.GetType().GetMethod("SessionDataValueGet").Invoke(this.Page.Master, new object[] { Key });
	}

	public void SessionDataValueSet(SessionDataKeys Key, string Value) {
		this.Page.Master.GetType().GetMethod("SessionDataValueSet").Invoke(this.Page.Master, new object[] { Key, Value });
	}

}