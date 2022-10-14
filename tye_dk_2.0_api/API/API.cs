using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monosolutions.Utils;
using tye.Data;

namespace tye.API {
	public partial class API : IDisposable {

		private string connectionString = String.Empty;
		private ObjectWrapper OW = null;

		public API(string ConnectionString) {
			connectionString = ConnectionString;
			OW = new ObjectWrapper();
		}

		public object SaveObject(object Instance) {
			object objReturn = null;
			string strInstanceName = Instance.GetType().Name;
			if (this.GetType().GetMethod(strInstanceName + "Save") != null) {
				objReturn = this.GetType().GetMethod(Instance.GetType().Name + "Save").Invoke(this, new object[] { Instance });
			} else {
				throw new Exception("Method '" + Instance.GetType().Name + "Save' doesn't exist!");
			}
			return objReturn;
		}

		void IDisposable.Dispose() {
		}

		/// <summary>
		/// Executes a non query command directly against the database
		/// </summary>
		/// <param name="CommandText"></param>
		/// <param name="Parameters"></param>
		public void ExecuteNonQuery(string CommandText) {
			//using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
			//   dc.ExecuteStoreCommand(CommandText, null);
			//}
		}


		
	}

}
