using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.Program program_get_single(Program data) {
			var element = (Data.Program)OW.WrapObject(typeof(Data.Program), data);
			foreach (var programTest in data.ProgramEyeTests) {
				element.ProgramEyeTests.Add(programeyetest_get_single(programTest));
			}
			return element;
		}

		public void ProgramDeletePermanently(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Programs
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					dc.Programs.DeleteObject(element);
					dc.SaveChanges();
				}
			}
		}

		public Data.Program ProgramGetSingle(int ID) {
			Data.Program data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Programs
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = program_get_single(element);
				}
			}
			return data;
		}

		public Data.Program ProgramGetSingleByUserID(int UserID) {
			Data.Program data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Programs
									where n.ClientUserID == UserID
									select n).FirstOrDefault();

				if (element != null) {
					data = program_get_single(element);
				}
			}
			return data;
		}

		public List<Data.Program> ProgramGetCollection() {
			List<Data.Program> data = new List<Data.Program>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Programs
									 select n);

				foreach (var element in elements)
					data.Add(program_get_single(element));
			}
			return data;
		}

		public Data.Program ProgramSave(Data.Program data) {
			Data.Program returnData = data;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					Program entry = (Program)OW.WrapObject(typeof(Program), data);
					dc.Programs.AddObject(entry);
					dc.SaveChanges();
					returnData.ID = entry.ID;

					foreach (var eyetest in data.ProgramEyeTests) {
                        ProgramEyeTestSave(entry.ID, eyetest.EyeTestID, eyetest.Locked, eyetest.Priority);
					}

				} else {  // existing
					var entry = (from n in dc.Programs
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry = (Program)OW.WrapExistingObject(entry, data);
						dc.SaveChanges();

						List<int> lstIDs = entry.ProgramEyeTests.Select(n => n.EyeTestID).ToList();
						List<int> lstCurrentIDs = data.ProgramEyeTests.Select(n => n.EyeTestID).ToList();

						// delete removed eye tests
                        //foreach (var intDeleteEyeTestID in lstIDs.Except(lstCurrentIDs)) {
                        //    ProgramEyeTestDeletePermanently(entry.ID, intDeleteEyeTestID);
                        //}
						
						// add newly added eye tests
                        //foreach (var intNewID in lstCurrentIDs.Except(lstIDs)) {
                        //    var newEyeTest = data.ProgramEyeTests.Where(n => n.EyeTestID == intNewID).First();
                        //    ProgramEyeTestSave(data.ID, newEyeTest.EyeTestID, newEyeTest.Locked, newEyeTest.Priority, newEyeTest.LockedByOptician);
                        //}

						// add update existing
						foreach (var intNewID in lstCurrentIDs.Where(n => lstIDs.Contains(n))) {
							var newEyeTest = data.ProgramEyeTests.Where(n => n.EyeTestID == intNewID).First();
							ProgramEyeTestSave(data.ID, newEyeTest.EyeTestID, newEyeTest.Locked, newEyeTest.Priority, newEyeTest.LockedByOptician);
						}
					}
				}
			}
			return returnData;
		}

	}
}