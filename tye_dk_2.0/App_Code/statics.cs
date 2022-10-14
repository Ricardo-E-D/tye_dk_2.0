// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System.ComponentModel;
using System.Net.Mail;

/// <summary>
/// Summary description for statics
/// </summary>
public static class statics {

	// valid data types for Attrib Types include: Boolean, DateTime, Number, Picklist, and String
	public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	public static tye.API.API GetApi() {
		string c = "DatabaseEntities";
		try {
			//if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
			//   c += "Local";
		} catch (Exception) { }
		string strConn = "";
		
		strConn = System.Configuration.ConfigurationManager
							.ConnectionStrings[c].ConnectionString;
		return new tye.API.API(strConn);
	}

	/// <summary>
	/// Transforms xml
	/// </summary>
	/// <param name="Xml"></param>
	/// <param name="PathToXslt"></param>
	/// <param name="Parameters">String array of ParameterName, ParameterValue, ParameterName, ParameterValue etc.</param>
	/// <returns></returns>
	public static string TransformXml(string Xml, string PathToXslt, string[] Parameters) {
		using (MemoryStream ms = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml))) {
			string strXsltPath = HttpContext.Current.Server.MapPath(PathToXslt);

			if (Parameters.Length % 2 != 0)
				throw new Exception("Parameters count must be dividable by 2");

			foreach (var p in Parameters) {
				if (p.GetType() != typeof(String))
					throw new Exception("All Parameters must be of type String");
			}

			XslCompiledTransform XSLTransform = new XslCompiledTransform();
			XsltArgumentList argsList = new XsltArgumentList();
			for (int i = 0; i < Parameters.Length; i += 2) {
				argsList.AddParam(Parameters[i], "", Parameters[i + 1]);
			}

			XSLTransform.Load(strXsltPath);

			using (TextWriter htmlWriter = new StringWriter()) {
				using (XmlReader xmlreader = XmlReader.Create(ms)) {
					XSLTransform.Transform(xmlreader, argsList, htmlWriter);
					xmlreader.Close();
				}
				return htmlWriter.ToString();
			}
		}
	} // method

	public static class UISettings {
		/// <summary>
		/// The number of child nodes to load on the editing page of Entity objects
		/// </summary>
		public static int ChildNodesLoadCount = 20;
		public static bool DefaultPermissionIsAllow = statics.App.GetSetting(SettingsKeys.DefaultPermission) == "Allow";
	}

	public static class App {
		public static string Name = "TrainYourEyes.com";
		public static string Version = "2.0";
		public static string GetSetting(SettingsKeys SettingsKey) {
			return System.Configuration.ConfigurationManager.AppSettings[SettingsKey.ToString()];
		}
		public static void SetSetting(SettingsKeys SettingsKey, string Value) {
			//System.Configuration.ConfigurationManager.AppSettings[SettingsKey.ToString()] = Value;
			
			System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			System.Configuration.KeyValueConfigurationElement setting = config.AppSettings.Settings[SettingsKey.ToString()];
			setting.Value =  Value;
			config.Save();
		}
		/// <summary>
		/// Gets the physical root path of the applications
		/// </summary>
		/// <returns></returns>
		public static string RootPath {
			get {
				string rootPath = String.Empty;
				if (HttpContext.Current != null && HttpContext.Current.Server != null) {
					rootPath = HttpContext.Current.Server.MapPath("~/");
				}
				return rootPath;
			}
		}
	}

	public static string SendEmailDefaultTemplate(string To, string Subject, string Heading, string Body) {
		int iTry = 0;

		string mailtemplate = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/mailtemplate/default.htm"));

		MailMessage mail = new MailMessage();
		mail.From = new MailAddress(App.GetSetting(SettingsKeys.MailSenderAddress));
		if (To.Contains(";")) {
			foreach (string recipient in To.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				mail.To.Add(recipient);
		} else
			mail.To.Add(To);

		string bodyhtml = mailtemplate
			.Replace("{heading}", Heading)
			.Replace("{content}", Body);

		mail.Subject = Subject;
		mail.IsBodyHtml = true;
		mail.Body = bodyhtml;

		//mail.Attachments.Add(new Attachment(FileToAttach));

		try {
			SmtpClient smtp = new SmtpClient(App.GetSetting(SettingsKeys.MailServerHost));
			if (int.TryParse(App.GetSetting(SettingsKeys.MailServerPort), out iTry) && iTry > 0)
				smtp.Port = iTry;
			smtp.Timeout = 60 * 1000;
			smtp.Credentials = new System.Net.NetworkCredential(App.GetSetting(SettingsKeys.MailServerUsername), App.GetSetting(SettingsKeys.MailServerPassword));
			smtp.Send(mail);
			mail.Dispose();
			return String.Empty;
		} catch (Exception ex) {
			return ex.ToString();
		}
	}

	public static string SendEmail(string To, string Subject, string BodyText) {
		int iTry = 0;

		MailMessage mail = new MailMessage();
		mail.From = new MailAddress(App.GetSetting(SettingsKeys.MailSenderAddress));

		//mail.To.Add(settings.EmailTo);
		if (To.Contains(";")) {
			foreach (string recipient in To.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				mail.To.Add(recipient);
		} else
			mail.To.Add(To);

		mail.Subject = Subject;
		mail.IsBodyHtml = true;
		mail.Body = BodyText;

		//mail.Attachments.Add(new Attachment(FileToAttach));
	
		try {
			SmtpClient smtp = new SmtpClient(App.GetSetting(SettingsKeys.MailServerHost));
			if (int.TryParse(App.GetSetting(SettingsKeys.MailServerPort), out iTry) && iTry > 0)
				smtp.Port = iTry;
			smtp.Timeout = 60 * 1000;
			smtp.Credentials = new System.Net.NetworkCredential(App.GetSetting(SettingsKeys.MailServerUsername), App.GetSetting(SettingsKeys.MailServerPassword));
			smtp.Send(mail);
			mail.Dispose();
			return String.Empty;
		} catch (Exception ex) {
			return ex.ToString();
		}
		
	}
}

public enum CacheKeys {
	CssEntityTypeIcons,
	SystemJobRunning
}
public enum SettingsKeys { 
	Authentication,
	DatabaseProvider,
	DefaultPermission,
	EncryptionKey,
	MailSenderAddress,
	MailServerHost,
	MailServerPort,
	MailServerUsername,
	MailServerPassword
}
public enum SessionKeys { 
	CurrentUser,
	CurrentUserReloadTimeStamp,
	AdvancedFindExportProgress,
	//Impersonation,
	SessionDataValues,
	AdvancedFind,
	AdvancedFindList,
	ShoppingCart
}
public enum SessionDataKeys {
	QuickSearchEntityType,
	QuickSearchShowAttributes,
	QuickSearchPhrase,
	Impersonating,
	ClientCodeIsValid
}
