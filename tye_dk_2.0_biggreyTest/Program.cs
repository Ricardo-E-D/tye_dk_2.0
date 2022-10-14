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



				var doctype = contentTypeService.GetContentType("FaqMaster");
				var content = contentService.GetById(1146);

				string q = "";
				Console.Write(content.Name);
				System.Threading.Thread.Sleep(1000);
				//var content = contentService.CreateContent(title, parentid, "NewsItem", 0);
				//content.Properties["newsActive"].Value = true;
				//content.Properties["newsDate"].Value = dt;
				//content.Properties["newsText"].Value = body;
				//try {
				//   contentService.Save(content);
			}

			//Console.ReadKey();
		}
	}
}
