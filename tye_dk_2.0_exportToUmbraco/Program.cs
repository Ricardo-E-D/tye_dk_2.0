using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UmbConsole;
using Umbraco.Core;
using umbraco.cms.businesslogic.web;
using umbraco;

namespace tye_dk_2._0_exportToUmbraco {
	class Program {
		static void w(string q) {
			Console.WriteLine(q);
		}

		static void Main(string[] args) {

			//E:\Development\clients\tye\tye_dk_2.0\App_Data\importdata
			
			if (1 == 1) { 

				//Initialize the application
				var application = new ConsoleApplicationBase();
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



				XmlDocument doc = new XmlDocument();
				doc.Load(@"E:\Development\clients\tye\tye_dk_2.0\App_Data\importdata\news.xml");

				XmlNode top = doc.ChildNodes[3];

				int counter = 0;
				
				foreach (XmlNode node in doc.SelectNodes("tye/news")) {

					DateTime dt = DateTime.Now;
					DateTime.TryParse(node.SelectSingleNode("addedtime").InnerText, out dt);

					string title = node.SelectSingleNode("header").InnerText;
					string body = node.SelectSingleNode("body").InnerText;
					int intLangID = 0;

					if (!int.TryParse(node.SelectSingleNode("languageid").InnerText, out intLangID)) {
						w("error parsing language id(!)");
						continue;
					}

					w(dt.ToString());
					w(title);
					return;

					var doctype = contentTypeService.GetContentType("NewsItem"); //DocumentType.GetByAlias("Textpage");
					//umbraco.BusinessLogic.User author = umbraco.BusinessLogic.User.GetUser(0);
					
					//var fuckmw = contentService.GetById(1269);

					// delete german articles
					//foreach (var child in contentService.GetChildren(1271)) {
					//   try {
					//   } catch (Exception) { }
					//}
					//return;

					int parentid = 0;
					switch(intLangID) {
						case 1: // DK
							parentid = 1269;
							break;

						case 2: // NO
							parentid = 1270;
							break;

						case 3: // UK
							parentid = 1272;
							break;

						case 4: // DE
							parentid = 1271;
							break;
					}

					
					////Get the type you would like to use by its alias and the user who should be the creator of the document 
					//DocumentType docType = DocumentType.GetByAlias("NewsItem");
					//umbraco.BusinessLogic.User author = umbraco.BusinessLogic.User.GetUser(0);

					//Document umbdoc = Document.MakeNew("My new document", docType, author, 1052);

					////// Get the properties you wish to modify by it's alias and set their value
					//umbdoc.getProperty("bodyText").Value = "<p>Your body text</p>";
					//umbdoc.getProperty("articleDate").Value = DateTime.Now;

					//////after creating the document, prepare it for publishing 

					//umbdoc.Publish(author);

					////Tell umbraco to publish the document
					//umbraco.library.UpdateDocumentCache(umbdoc.Id);


					//var currentProduct = uQuery.GetNode(1052);

					counter++;
					
					var content = contentService.CreateContent(title, parentid, "NewsItem", 0);
					content.Properties["newsActive"].Value = true;
					content.Properties["newsDate"].Value = dt;
					content.Properties["newsText"].Value = body;
					try {
						contentService.Save(content);
					} catch(Exception ex) {
					
					}

					//if(counter > 2)
					//   return;
				}
			}

			//Console.ReadKey();
		}
	}
}
