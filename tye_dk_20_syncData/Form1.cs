using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace tye_dk_20_syncData {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Ok to go-go with all tables?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return; 
			sync(true);
		}

		private void sync(bool SyncAllTables) {
			string Src = tbConnSource.Text;
			string Dest = tbConnTarget.Text;
			int BatchSize = 500;
			int NotifyAfter = 500;

			SqlBulkCopy c = new SqlBulkCopy(Dest, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepIdentity);
			SqlConnection srcConn = new SqlConnection(Src);
			srcConn.Open();

			SqlConnection destConn = new SqlConnection(Dest);
			destConn.Open();

			SqlCommand cm = new SqlCommand("select * from sysobjects where type='u' AND name <> 'sysdiagrams'");
			cm.Connection = srcConn;
			cm.CommandType = CommandType.Text;

			DataTable AllTables = new DataTable();
			SqlDataAdapter a = new SqlDataAdapter(cm);
			a.Fill(AllTables);
			SqlDataReader dr;

			int n = 0;
			int tot = AllTables.Rows.Count;

			List<string> tableNames = new List<string>();
			foreach (DataRow r in AllTables.Rows) {
				tableNames.Add(r["name"].ToString());
			}

			if (!SyncAllTables) {
				tableNames.Clear();
				foreach (var item in lbDropTables.CheckedItems) {
					//if (item.Checked) {
						tableNames.Add(item.ToString());
					//}
				}
			}
			//Console.WriteLine("Found {0} Tables to copy.", tot);
			//Console.WriteLine("");

			c.SqlRowsCopied += new SqlRowsCopiedEventHandler(delegate(object sender, SqlRowsCopiedEventArgs e) {
				tbLog.Text = " -- copied " + e.RowsCopied + " rows ..." + Environment.NewLine + tbLog.Text;
				Application.DoEvents();
			});

			foreach (string tableName in tableNames) {
				SqlCommand cmDel = new SqlCommand("delete FROM [" + tableName + "]");
				cmDel.Connection = destConn;
				cmDel.CommandType = CommandType.Text;
				cmDel.ExecuteNonQuery();
				cmDel.Dispose();

				Application.DoEvents();

				tbLog.Text = String.Format("Copying table {0} of {1}: {2}", ++n, tot, tableName) + Environment.NewLine + tbLog.Text;
				cm.CommandText = String.Format("select * from [{0}]", tableName);
				dr = cm.ExecuteReader();

				// <to prevent collation errors>
				SqlDataAdapter dap = new SqlDataAdapter("SELECT  * FROM [" + tableName + "]", srcConn);
				DataSet ds = new DataSet();
				dap.Fill(ds);
				// </to prevent collation errors>

				c.BatchSize = BatchSize;
				c.DestinationTableName = "[" + tableName.ToString() + "]";
				c.NotifyAfter = NotifyAfter;
				c.WriteToServer(ds.Tables[0]);
				dr.Close();

				ds.Dispose();
				dap.Dispose();
			}
			
			srcConn.Close();
			destConn.Close();

			c.Close();

			tbLog.Text = "All done..." + Environment.NewLine + tbLog.Text;

		}

		private void Form1_Load(object sender, EventArgs e) {

		}

		private void btnLoadTables_Click(object sender, EventArgs e) {
			string Dest = tbConnTarget.Text;
			SqlConnection destConn = new SqlConnection(Dest);
			destConn.Open();

			SqlCommand cm = new SqlCommand("select * from sysobjects where type='u' AND name <> 'sysdiagrams' ORDER BY [Name]");
			cm.Connection = destConn;
			cm.CommandType = CommandType.Text;

			DataTable AllTables = new DataTable();
			SqlDataAdapter a = new SqlDataAdapter(cm);
			a.Fill(AllTables);

			lbDropTables.Items.Clear();
			foreach (DataRow r in AllTables.Rows) {
				//lbDropTables.Items.Add(new ListViewItem() { Text = r["Name"].ToString() });
				lbDropTables.Items.Add(r["Name"].ToString());
			}
			cm.Dispose();
			destConn.Close();
			destConn.Dispose();
		}

		private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			for (int i = 0; i < lbDropTables.Items.Count; i++) {
				lbDropTables.SetItemChecked(i, true);
			}
		}

		private void lnkSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			for (int i = 0; i < lbDropTables.Items.Count; i++) {
				lbDropTables.SetItemChecked(i, false);
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Ok to delete all selected tables?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;

			string Dest = tbConnTarget.Text;
			SqlConnection destConn = new SqlConnection(Dest);
			destConn.Open();

			foreach (var item in lbDropTables.CheckedItems) {
				try {
					tbLog.Text = String.Format("Dropping table {0}", item) + Environment.NewLine + tbLog.Text;
					SqlCommand cm = new SqlCommand("DROP TABLE [" + item + "]");
					cm.Connection = destConn;
					cm.CommandType = CommandType.Text;
					cm.ExecuteNonQuery();
					cm.Dispose();
				} catch (Exception ex) {
					tbLog.Text = String.Format("Exception: {0}", ex.Message) + Environment.NewLine + tbLog.Text;
				}
			}

			destConn.Close();
			destConn.Dispose();
			btnLoadTables_Click(btnLoadTables, new EventArgs());
		}

		private void btnGoSelectedTables_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Ok to go-go with selected tables?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;
			sync(false);
		}

		private void btnDeleteAll_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Ok to delete all data from selected tables?", "Confirm", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;

			SqlConnection destConn = new SqlConnection(tbConnTarget.Text);
			destConn.Open();

			List<string> tableNames = new List<string>();
			foreach (var item in lbDropTables.CheckedItems) {
				tableNames.Add(item.ToString());
			}
			
			foreach (string tableName in tableNames) {
				try {
					SqlCommand cmDel = new SqlCommand("delete FROM [" + tableName + "]");
					cmDel.Connection = destConn;
					cmDel.CommandType = CommandType.Text;
					cmDel.ExecuteNonQuery();
					cmDel.Dispose();

					tbLog.Text = String.Format("Deleted data from table {0}", tableName) + Environment.NewLine + tbLog.Text;

					Application.DoEvents();
				} catch (Exception) {
					tbLog.Text = String.Format("Failed to delete data from table {0}", tableName) + Environment.NewLine + tbLog.Text;
				}
			} // foreach

			destConn.Close();
			destConn.Dispose();
		}

		private void btnTruncate_Click(object sender, EventArgs e) {
			if (MessageBox.Show("Ok to truncate all selected tables?", "Confirm", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;

			SqlConnection destConn = new SqlConnection(tbConnTarget.Text);
			destConn.Open();

			List<string> tableNames = new List<string>();
			foreach (var item in lbDropTables.CheckedItems) {
				tableNames.Add(item.ToString());
			}

			foreach (string tableName in tableNames) {
				try {
					SqlCommand cmDel = new SqlCommand("TRUNCATE TABLE [" + tableName + "]");
					cmDel.Connection = destConn;
					cmDel.CommandType = CommandType.Text;
					cmDel.ExecuteNonQuery();
					cmDel.Dispose();

					tbLog.Text = String.Format("Truncated table {0}", tableName) + Environment.NewLine + tbLog.Text;

					Application.DoEvents();
				} catch (Exception ex) {
					tbLog.Text = String.Format("Failed to truncate table {0}", tableName) + ex.ToString() + Environment.NewLine + Environment.NewLine + Environment.NewLine + tbLog.Text;
				}
			} // foreach

			destConn.Close();
			destConn.Dispose();
		}

	
		
	}
}
