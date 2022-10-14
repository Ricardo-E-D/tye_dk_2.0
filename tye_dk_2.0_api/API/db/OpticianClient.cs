using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		/// <summary>
		/// In KeyValuePair, first = OpticianID and second = ClientID
		/// </summary>
		/// <param name="OpticianID"></param>
		/// <returns></returns>
		public List<KeyValuePair<int, int>> OpticianClientGetCollectionByOptician(int OpticianID) {
			List<KeyValuePair<int, int>> data = new List<KeyValuePair<int,int>>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.OpticianClients
									where n.OpticianUserID == OpticianID
									select n);

				foreach(var rel in element)
					data.Add(new KeyValuePair<int,int>(rel.OpticianUserID, rel.ClientUserID));
			}
			return data;
		}
		
		/// <summary>
		/// Gets an Optician User instance from ClientID
		/// </summary>
		/// <param name="OpticianID"></param>
		/// <returns></returns>
		public Data.User OpticianClientGetOptician(int ClientID) {
			Data.User optician = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.OpticianClients
									where n.ClientUserID == ClientID
									select n).FirstOrDefault();

				if (element != null)
					optician = UserGetSingle(element.OpticianUserID);

				return optician;
			}
		}

		public void OpticianClientAdd(int OpticianID, int ClientID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var exists = (from n in dc.OpticianClients
								  where n.OpticianUserID == OpticianID && n.ClientUserID == ClientID
								  select n).FirstOrDefault();
				if (exists == null) { 
					var oc = new db.OpticianClient() { ID = 0, OpticianUserID = OpticianID, ClientUserID = ClientID };
					dc.OpticianClients.AddObject(oc);
					dc.SaveChanges();
				}
			}
		}

		public void OpticianClientDeleteByID(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.OpticianClients
									where n.ID == ID
									select n).FirstOrDefault();
				if (element != null) {
					dc.DeleteObject(element);
					dc.SaveChanges();
				}
			}
		}

		public void OpticianClientDeleteByClientID(int ClientID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.OpticianClients
									where n.ClientUserID == ClientID
									select n).FirstOrDefault();
				if (element != null) {
					dc.DeleteObject(element);
					dc.SaveChanges();
				}
			}
		}

		/// <summary>
		/// Deletes every optician/client relation !!!!
		/// </summary>
		/// <param name="ID"></param>
		public void OpticianClientDeleteByOpticianID(int OpticianID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.OpticianClients
									where n.OpticianUserID == OpticianID
									select n);
				foreach(var element in elements) {
					dc.DeleteObject(element);
				}
				dc.SaveChanges();
			}
		}
	}
}