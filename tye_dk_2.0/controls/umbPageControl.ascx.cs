using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using UmbConsole;
using Umbraco.Core;

public partial class controls_umbPageControl : UserControl {
	
	public int UmbracoPageID { get; set; }

	protected void Page_Load(object sender, EventArgs e) {
		string key = "umbService" + UmbracoPageID;

		if (CacheHandler.ItemExpired(key, 30)) {
			string s = getContents();
			CacheHandler.AddItem(key, s, 30);
		}

		litContent.Text = (CacheHandler.GetItem(key).ToString());
	}

	private string getContents() {
		//Initialize the application
		using (var application = new ConsoleApplicationBase()) {
			application.Start(application, new EventArgs());
			Console.WriteLine("Application Started");

			var context = ApplicationContext.Current;
			Console.WriteLine("ApplicationContext is available: " + (context != null).ToString());
			//Write status for DatabaseContext
			var databaseContext = context.DatabaseContext;
			Console.WriteLine("DatabaseContext is available: " + (databaseContext != null).ToString());
			//Write status for Database object
			var database = databaseContext.Database;
			Console.WriteLine("Database is available: " + (database != null).ToString());
			Console.WriteLine("--------------------");

			//Get the ServiceContext and the two services we are going to use
			var serviceContext = context.Services;
			var contentService = serviceContext.ContentService;
			var contentTypeService = serviceContext.ContentTypeService;

			var servicepage = contentService.GetById(UmbracoPageID);
			if (servicepage != null) {
				string q = servicepage.Properties["pageBodyText"].Value.ToString();
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(q);

				var links = doc.DocumentNode.SelectNodes("//a");
				foreach (var link in links) {
					if(!link.HasAttributes)
						continue;

					HtmlAgilityPack.HtmlAttribute att = link.Attributes["href"];


					if (att != null && att.Value.StartsWith("upload") || att.Value.StartsWith("/upload")) {
						att.Value = "http://tye.dk" + (att.Value.StartsWith("/") ? "" : "/") + att.Value;
					} else if (att != null && att.Value.StartsWith("media") || att.Value.StartsWith("/media")) {
						att.Value = "http://tye.dk" + (att.Value.StartsWith("/") ? "" : "/") + att.Value;
					}
				}

				var imgs = doc.DocumentNode.SelectNodes("//img");
				foreach (var img in imgs) {
					if (!img.HasAttributes)
						continue;

					HtmlAgilityPack.HtmlAttribute att = img.Attributes["src"];
					if (att != null && att.Value.StartsWith("gfx")) {
						att.Value = "http://tye.dk/" + att.Value;
					}
				}
				return doc.DocumentNode.OuterHtml;
			} else {
				return string.Empty;
			}
		}
	}
}