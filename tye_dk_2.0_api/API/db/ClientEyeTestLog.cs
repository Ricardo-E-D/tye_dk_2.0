using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using tye.db;

namespace tye.API {
	
	public partial class API {

		private Data.ClientEyeTestLog clienteyetestlog_get_single(ClientEyeTestLog data) {
			var r = (Data.ClientEyeTestLog)OW.WrapObject(typeof(Data.ClientEyeTestLog), data);
			r.ProgramEyeTest = programeyetest_get_single(data.ProgramEyeTest);
			return r;
		}

		public Data.ClientEyeTestLog ClientEyeTestLogGetSingle(int ID) {
			Data.ClientEyeTestLog clienteyetestlog = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.ClientEyeTestLogs
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					clienteyetestlog = clienteyetestlog_get_single(data);
				}
			} // using
			return clienteyetestlog;
		}

		public List<Data.ClientEyeTestLog> ClientEyeTestLogGetCollection() {
			List<Data.ClientEyeTestLog> lstclienteyetestlog = new List<Data.ClientEyeTestLog>();
			using(DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.ClientEyeTestLogs
					select n);
				foreach(var data in datas)
					lstclienteyetestlog.Add(clienteyetestlog_get_single(data));
			} // using
			return lstclienteyetestlog;
		}

		public List<Data.ClientEyeTestLog> ClientEyeTestLogGetCollection(int ClientUserID) {
			List<Data.ClientEyeTestLog> lstclienteyetestlog = new List<Data.ClientEyeTestLog>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.ClientEyeTestLogs
								 where n.ProgramEyeTest.Program.ClientUserID == ClientUserID
								 select n);
				foreach (var data in datas)
					lstclienteyetestlog.Add(clienteyetestlog_get_single(data));
			} // using
			return lstclienteyetestlog;
		}

		/// <summary>
		/// Note!!! Parameter passed is EyeTestID....not ProgramEyeTestID
		/// </summary>
		/// <param name="ClientUserID"></param>
		/// <param name="EyeTestID"></param>
		/// <returns></returns>
		public List<Data.ClientEyeTestLog> ClientEyeTestLogGetCollection(int ClientUserID, int EyeTestID) {
			List<Data.ClientEyeTestLog> lstclienteyetestlog = new List<Data.ClientEyeTestLog>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.ClientEyeTestLogs
								 where n.ProgramEyeTest.Program.ClientUserID == ClientUserID
									&& n.ProgramEyeTest.EyeTestID == EyeTestID
								 select n);
				foreach (var data in datas)
					lstclienteyetestlog.Add(clienteyetestlog_get_single(data));
			} // using
			return lstclienteyetestlog;
		}


		/// <summary>
		/// </summary>
		/// <param name="ClientUserID"></param>
		/// <param name="EyeTestID"></param>
		/// <returns></returns>
		public List<Data.ClientEyeTestLog> ClientEyeTestLogGetCollection(int ClientUserID, DateTime StartDate, DateTime EndDate) {
			List<Data.ClientEyeTestLog> lstclienteyetestlog = new List<Data.ClientEyeTestLog>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.ClientEyeTestLogs
								 where n.ProgramEyeTest.Program.ClientUserID == ClientUserID
									&& n.StartTime >= StartDate && n.EndTime <= EndDate
									orderby n.StartTime descending
								 select n);
				foreach (var data in datas)
					lstclienteyetestlog.Add(clienteyetestlog_get_single(data));
			} // using
			return lstclienteyetestlog;
		}


		public Data.ClientEyeTestLog ClientEyeTestLogGetHighScore(int ClientUserID, int EyeTestID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.ClientEyeTestLogs
								 where n.ProgramEyeTest.Program.ClientUserID == ClientUserID
									&& n.ProgramEyeTest.EyeTestID == EyeTestID
									&& n.HighScore == true 
								 orderby n.StartTime descending
								 select n).FirstOrDefault();
				if (datas != null)
					return clienteyetestlog_get_single(datas);
				else
					return null;
			} // using

		}

		public Data.ClientEyeTestLog ClientEyeTestLogSave(Data.ClientEyeTestLog savedata) {
			Data.ClientEyeTestLog clienteyetestlog = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new ClientEyeTestLog();
					data = (ClientEyeTestLog)OW.WrapExistingObject(data, savedata);
					dc.ClientEyeTestLogs.AddObject(data);
					dc.SaveChanges();
					clienteyetestlog = clienteyetestlog_get_single(data);
				} else { // existing
					var data = (from n in dc.ClientEyeTestLogs
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (ClientEyeTestLog)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return clienteyetestlog;
		}

		public List<Data.ClientEyeTestLog> ClientEyeTestLogSearch(string SearchExpression, List<object> Parameters, string OrderByExpression = "", int SkipCount = 0, int TakeCount = 0) {
			List<Data.ClientEyeTestLog> resultSet = new List<Data.ClientEyeTestLog>();

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.ClientEyeTestLogs
									 select n);

				if (!String.IsNullOrEmpty(OrderByExpression))
					elements = elements.OrderBy(OrderByExpression);

				elements = elements.Where(SearchExpression, Parameters.ToArray());

				if (SkipCount > 0)
					elements = elements.Skip(SkipCount);

				if (TakeCount > 0)
					elements = elements.Take(TakeCount);

				if (elements.Count() > 0)
					resultSet.AddRange(elements.AsEnumerable().Select(n => clienteyetestlog_get_single(n)));
			}
			return resultSet;
		} // method

		public void ClientEyeTestLogUpdateEndTime(int ID, string UpdateToken, DateTime EndTime, string AttribName, string AttribValue, int Score) { 
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				DateTime dtCompare = DateTime.UtcNow.AddHours(-1);
				var log = (from n in dc.ClientEyeTestLogs
							  where n.ProgramEyeTestID == ID 
									&& n.UpdateToken == UpdateToken
									&& n.StartTime > dtCompare // to prevent update old-old records
							  select n).FirstOrDefault();
				if(log != null) {
					log.EndTime = EndTime;
					if (!String.IsNullOrEmpty(AttribName))
						log.AttribName = AttribName;
					if (!String.IsNullOrEmpty(AttribValue))
						log.AttribValue = AttribValue;
					if (Score > 0)
						log.Score = Score;
					dc.SaveChanges();
				}
			}
		}
	}

}