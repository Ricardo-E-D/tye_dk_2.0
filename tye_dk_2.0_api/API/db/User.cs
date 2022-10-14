using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.User user_get_single(User data) {
			Data.User user = (Data.User)OW.WrapObject(typeof(Data.User), data);
			user.Type = (Data.User.UserType)data.UserTypeID;
			user.Country = (Data.Country)OW.WrapObject(typeof(Data.Country), data.Country);
			user.Language = (Data.Language)OW.WrapObject(typeof(Data.Language), data.Language);
			if (!String.IsNullOrEmpty(data.PudData)) {
				user.Pud = Data.Pud.DeSerialize(data.PudData);
			}
			return user;
		}

		public void UserDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Users
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					data.Enabled = false;
					dc.SaveChanges();
				}
			} // using
		}
		
		public void UserDeletePermanently(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Users
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					dc.DeleteObject(data);
					dc.SaveChanges();
				}
			} // using
		}

		public void UserDeleteAllForDataImport() {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Users
								where !n.Email.Contains("monosolutions")
								select n);
				foreach(var d in data) {
					dc.DeleteObject(d);
				}
				dc.SaveChanges();
			} // using
		}

		public List<Data.User> UserGetCollection() {
			List<Data.User> lstusers = new List<Data.User>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Users
								 orderby n.FullName
								 select n); //.ToList();
				foreach (var data in datas) { 
					lstusers.Add(user_get_single(data));
				}
			} // using
			return lstusers;
		}

		public List<Data.User> UserGetCollection(List<int> IDs) {
			List<Data.User> lstusers = new List<Data.User>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Users
								 where IDs.Contains(n.ID)
								 orderby n.FullName
								 select n); //.ToList();
				foreach (var data in datas)
					lstusers.Add(user_get_single(data));
			} // using
			return lstusers;
		}

		public List<Data.User> UserGetCollection(tye.Data.User.UserType Type) {
			List<Data.User> lstusers = new List<Data.User>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Users
								 where n.UserTypeID == (int)Type
								 orderby n.FullName
								 select n); //.ToList();
				foreach (var data in datas)
					lstusers.Add(user_get_single(data));
			} // using
			return lstusers;
		}
		
		public List<Data.User> UserGetCollection(tye.Data.User.UserType Type, int LangaugeID) {
			List<Data.User> lstusers = new List<Data.User>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Users
								 where n.UserTypeID == (int)Type && n.LanguageID == LangaugeID
								 orderby n.FullName
								 select n); //.ToList();
				foreach (var data in datas)
					lstusers.Add(user_get_single(data));
			} // using
			return lstusers;
		}


		public List<Data.User> UserGetCollectionByOptician(int OpticianID) {
			List<Data.User> lstusers = new List<Data.User>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Users
								 where (from m in dc.OpticianClients 
											where m.OpticianUserID == OpticianID 
											select m.ClientUserID)
											.Contains(n.ID)
								 orderby n.FullName
								 select n); //.ToList();
				foreach (var data in datas)
					lstusers.Add(user_get_single(data));
			} // using
			return lstusers;
		}

		public Data.User UserGetSingle(int ID) {
			Data.User user = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Users
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					user = user_get_single(data);
				}
			} // using
			return user;
		}

		public Data.User UserGetSingle(string Email) {
			Data.User user = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Users
								where n.Email == Email
								select n).FirstOrDefault();
				if (data != null) {
					user = user_get_single(data);
				}
			} // using
			return user;
		}

		public Data.User UserSave(Data.User savedata) {
			Data.User user = savedata;
			savedata.FullName = savedata.FirstName
						+ (String.IsNullOrEmpty(savedata.MiddleName) ? "" : " " + savedata.MiddleName)
						+ (String.IsNullOrEmpty(savedata.LastName) ? "" : " " + savedata.LastName);

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new User();
					data = (User)OW.WrapExistingObject(data, savedata);
					data.UserTypeID = (int)savedata.Type;
					//data.CountryID = savedata.Country.ID;
					dc.Users.AddObject(data);
					dc.SaveChanges();
					user.ID = data.ID;
				} else { // existing
					var data = (from n in dc.Users
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (User)OW.WrapExistingObject(data, savedata);
						data.UserTypeID = (int)savedata.Type;
						if (savedata.Pud != null)
							data.PudData = savedata.Pud.Serialize();
						//data.CountryID = savedata.Country.ID;
						dc.SaveChanges();
					}
				}
			} // using
			return user;
		}

		public List<Data.User> UserSearch(string SearchExpression, List<object> Parameters, string OrderByExpression = "", int SkipCount = 0, int TakeCount = 0) {
			List<Data.User> resultSet = new List<Data.User>();

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Users
									 select n);

				if (!String.IsNullOrEmpty(OrderByExpression))
					elements = elements.OrderBy(OrderByExpression);

				elements = elements.Where(SearchExpression, Parameters.ToArray());

				if (SkipCount > 0)
					elements = elements.Skip(SkipCount);

				if (TakeCount > 0)
					elements = elements.Take(TakeCount);

				if (elements.Count() > 0)
					resultSet.AddRange(elements.AsEnumerable().Select(n => user_get_single(n)));
			}
			return resultSet;
		} // method

	}
}
