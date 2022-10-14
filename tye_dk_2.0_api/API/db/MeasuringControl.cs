using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.MeasuringControl measuringControl_get_single(MeasuringControl data) {
			var element = (Data.MeasuringControl)Helpers.Deserialize(Convert.FromBase64String(data.Data));
			element.ID = data.ID;
			element.Created = data.Created;
			element.ClientUserID = data.ClientUserID;
			return element;
		}

		public Data.MeasuringControl MeasuringControlGetSingle(int ID) {
			Data.MeasuringControl data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.MeasuringControls
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = measuringControl_get_single(element);
				}
			}
			return data;
		}

		public List<Data.MeasuringControl> MeasuringControlGetCollection(int ClientUserID) {
			List<Data.MeasuringControl> data = new List<Data.MeasuringControl>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.MeasuringControls
									 where n.ClientUserID == ClientUserID
									 orderby n.Created descending
									 select n);

				foreach (var element in elements) {
					data.Add(measuringControl_get_single(element));
				}
			}
			return data;
		}

		public void MeasuringControlSave(Data.MeasuringControl data) {
			string strPud = "";

			try {
				byte[] bytPud = Helpers.Serialize(data);
				strPud = Convert.ToBase64String(bytPud);
			} catch (Exception) { }

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					//MeasuringControl entry = (MeasuringControl)OW.WrapObject(typeof(MeasuringControl), data);
					MeasuringControl entry = new MeasuringControl() { 
						ID = 0,
						Created = DateTime.UtcNow, 
						ClientUserID = data.ClientUserID,
						IsStart = data.IsStart,
						Data = strPud 
					};
					
					dc.MeasuringControls.AddObject(entry);
					dc.SaveChanges();
				} else {  // existing
					var entry = (from n in dc.MeasuringControls
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry.Data = strPud;
						dc.SaveChanges();
					}
				}
			}

		}
	}
}