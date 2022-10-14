using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;
using monosolutions.Utils;

namespace tye.API {
	public partial class API {

		private Data.EyeTest eyetest_get_single(EyeTest data) {
			var element = (Data.EyeTest)OW.WrapObject(typeof(Data.EyeTest), data);

			foreach (var eyetestInfo in data.EyeTestInfos) {
				element.EyeTestInfos.Add(eyetestinfo_get_single(eyetestInfo));
			}

			return element;
		}

		public Data.EyeTest EyeTestDelete(int ID) {
			Data.EyeTest data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.EyeTests
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					dc.EyeTests.DeleteObject(element);
					dc.SaveChanges();
				}
			}
			return data;
		}

		public Data.EyeTest EyeTestGetSingle(int ID) {
			Data.EyeTest data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.EyeTests
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = eyetest_get_single(element);
				}
			}
			return data;
		}

		public Data.EyeTest EyeTestGetSingle(string OldBbName) {
			Data.EyeTest data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.EyeTests
									where n.OldBbName == OldBbName
									select n).FirstOrDefault();

				if (element != null) {
					data = eyetest_get_single(element);
				}
			}
			return data;
		}

		public List<Data.EyeTest> EyeTestGetCollection() {
			List<Data.EyeTest> data = new List<Data.EyeTest>();
			if (CacheHandler.ItemExpired("ApiEyeTestGetCollection", 60)) {
				using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
					var elements = (from n in dc.EyeTests
                                        where n.OpticianID == null
										 orderby n.Priority
										 select n);

					foreach (var element in elements) {
						data.Add(eyetest_get_single(element));
					}
				}
			} else {
				data = (List<Data.EyeTest>)CacheHandler.GetItem("ApiEyeTestGetCollection");
			}
			return data;
		}

		public List<Data.EyeTest> EyeTestGetCollection(List<int> IDs) {
			List<Data.EyeTest> data = new List<Data.EyeTest>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.EyeTests
									 where IDs.Contains(n.ID)
									 orderby n.Priority
									 select n);

				foreach (var element in elements) {
					data.Add(eyetest_get_single(element));
				}
			}
			return data;
		}

        public List<Data.EyeTest> EyeTestGetCollection(int OpticianID)
        {
            List<Data.EyeTest> data = new List<Data.EyeTest>();
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var elements = (from n in dc.EyeTests
                                where n.OpticianID == OpticianID
                                orderby n.Priority
                                select n);

                foreach (var element in elements)
                {
                    data.Add(eyetest_get_single(element));
                }
            }
            return data;
        }

		public Data.EyeTest EyeTestSave(Data.EyeTest data) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					EyeTest entry = (EyeTest)OW.WrapObject(typeof(EyeTest), data);
					dc.EyeTests.AddObject(entry);
					dc.SaveChanges();
                    return eyetest_get_single(entry);
				} else {  // existing
					var entry = (from n in dc.EyeTests
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry = (EyeTest)OW.WrapExistingObject(entry, data);
						dc.SaveChanges();
                        return eyetest_get_single(entry);
					}
				}
			}
            return null;
		}

		public Data.EyeTest EyeTestGetFromProgramEyeTestID(int ClientID) {
			Data.EyeTest et = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var test = (from n in dc.ProgramEyeTests
								where n.ID == ClientID
								select n.EyeTest).FirstOrDefault();
				if (test != null)
					et = eyetest_get_single(test);
			}
			return et;
		}
	}
}