// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using monosolutions.Utils;
using tye.Data;

/// <summary>
/// Summary description for PageBase
/// </summary>
public class MasterBase : System.Web.UI.MasterPage
{
	public MasterBase()	{
		if (Dictionary == null) {
			using (var ipa = statics.GetApi()) {
				Dictionary = ipa.DictionaryGet();
			}
		}
	}

	Language _currentLanguage = null;
	public Language CurrentLanguage {
		get {
			if (_currentLanguage == null) {
				if (CurrentUser != null && CurrentUser.Language != null)
					_currentLanguage = CurrentUser.Language;

				if (_currentLanguage == null)
					return new Language() { ID = 4, Name = "English" };
				else
					return _currentLanguage;
			} else {
				return _currentLanguage;
			}
		}
		set { _currentLanguage = value; }
	}

	public bool Impersonating {
		get {
			return !String.IsNullOrEmpty(SessionDataValueGet(SessionDataKeys.Impersonating)) 
						&& SessionDataValueGet(SessionDataKeys.Impersonating) == "True";
		}
		set {
			SessionDataValueSet(SessionDataKeys.Impersonating, value.ToString());
		}
	}

	/// <summary>
	/// Current user without any impersonation
	/// </summary>
	public User CurrentBaseUser {
		get {
			User u = null;
			if (Context != null && Context.Session != null) {
				if (Context.Session[SessionKeys.CurrentUser.ToString()] != null)
					u = (User)Context.Session[SessionKeys.CurrentUser.ToString()];
			}
			return u;
		}
	}

	public User CurrentUser {
		get {
			User u = null;
			if (Context != null && Context.Session != null) {
				if (Context.Session[SessionKeys.CurrentUser.ToString()] != null)
					u = (User)Context.Session[SessionKeys.CurrentUser.ToString()];
				
				/*if (u != null) { 
					using (var ipa = statics.GetApi()) {
						u = ipa.UserGetSingle(u.ID);
					}
				}*/

				if (Impersonating && u.ImpersonatingUser != null)
					u = u.ImpersonatingUser;

				// test re-load to update permissions 
				if (u != null && !Impersonating) { 
					if (Context.Session[SessionKeys.CurrentUserReloadTimeStamp.ToString()] == null) {
						Context.Session[SessionKeys.CurrentUserReloadTimeStamp.ToString()] = DateTime.Now;
					} else {
						DateTime dtReload = (DateTime)Context.Session[SessionKeys.CurrentUserReloadTimeStamp.ToString()];
						if (DateTime.Now.Subtract(dtReload).TotalMinutes > 8) {
							using (var ipa = statics.GetApi())
								u = ipa.UserGetSingle(u.ID);
							this.CurrentUser = u;
						}
					}
				}
			}

			if (u == null) {

				//using (var ipa = statics.GetApi())
				//   u = ipa.UserGetSingle(1);
                try
                {
                    Response.Redirect("~/login.aspx?ru=" + HttpUtility.UrlEncode(Context.Request.Url.PathAndQuery));
                }
                catch (Exception) { }
			} else if (u.MustChangePassword && !Impersonating && !Context.Request.Url.ToString().ToLower().Contains("/loginchangepassword.aspx")) {
				Response.Redirect("~/loginChangePassword.aspx?ru=" + HttpUtility.UrlEncode(Context.Request.Url.PathAndQuery));
			}
			return u;
		}
		set {
			if (Context != null && Context.Session != null)
				Context.Session[SessionKeys.CurrentUser.ToString()] = value;
		}
	}

	public void DictionaryReload() {
		CacheHandler.RemoveItem("globalDictionary");
	}

	public Dictionary Dictionary {
		get {
			Dictionary dic = new Dictionary();
			if (CacheHandler.ItemExpired("globalDictionary", 60)) {
				using (var ipa = statics.GetApi()) {
					CacheHandler.AddItem("globalDictionary", (dic = ipa.DictionaryGet()), 60);
					CacheHandler.AddItem("globalDictionaryClientSide", ipa.DictionaryGetClientSide(), 60);
				}
			} else {
				dic = (Dictionary)CacheHandler.GetItem("globalDictionary");
			}
			return (dic == null ? new Dictionary() : dic);
		}
		set {
			CacheHandler.AddItem("globalDictionary", value, 60);
		}
	}
	
	public string DicValue(string Key) {
		if (Dictionary == null)
			DictionaryReload();
		if (Dictionary == null)
			return "";
		else 
		return Dictionary.GetValue(Key, CurrentLanguage);
	}

	public string SessionDataValueGet(SessionDataKeys Key) {
		if(HttpContext.Current == null || HttpContext.Current.Session == null) 
			return String.Empty;
		var session = HttpContext.Current.Session;
		if (session[SessionKeys.SessionDataValues.ToString()] == null) {
			session[SessionKeys.SessionDataValues.ToString()] = new Dictionary<SessionDataKeys, string>();
			return String.Empty;
		} else {
			var dic = (Dictionary<SessionDataKeys, string>)session[SessionKeys.SessionDataValues.ToString()];
			if(dic.ContainsKey(Key))
				return dic[Key];
			else
				return String.Empty;
		}
	}

	public void SessionDataValueSet(SessionDataKeys Key, string Value) {
		if(Session == null) 
			return;
		if(Session[SessionKeys.SessionDataValues.ToString()] == null) {
			Session[SessionKeys.SessionDataValues.ToString()] = new Dictionary<SessionDataKeys, string>();
		}
		var dic = (Dictionary<SessionDataKeys, string>)Session[SessionKeys.SessionDataValues.ToString()];
		if(!dic.ContainsKey(Key))
			dic.Add(Key, Value);
		else
			dic[Key] = Value;
	}

}