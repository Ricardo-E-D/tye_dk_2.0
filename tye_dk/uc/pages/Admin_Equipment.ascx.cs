namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Configuration;
	using System.Collections;
	using System.IO;
	using System.Web;
	using System.Web.Security;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.WebControls.WebParts;
	using System.Web.UI.HtmlControls;
	using exceptions;
	using System.Text;
	using MySql.Data.MySqlClient;

	public partial class Admin_Equipment : uc_pages
	{
		private string[] flags = { "danish_flag.gif", "norwegian_flag.gif", "english_flag.gif", "german_flag.gif" };
		private int intCurrentLang = 1;
		private string strQuery = "";
		string[] arrLang = { "DK", "NO", "UK", "DE" };

		protected void Page_Init(object sender, EventArgs e)
		{
			for (int i = 0; i < arrLang.Length; i++) {
				HtmlTableCell td = (HtmlTableCell)FindControl("tdDesc_" + arrLang[i]);
				if (td == null)
					throw new ApplicationException("Critical error. No output cell found");
				FredCK.FCKeditorV2.FCKeditor nF = new FredCK.FCKeditorV2.FCKeditor();
				nF = new FredCK.FCKeditorV2.FCKeditor();
				nF.ID = "editor_fck" + arrLang[i];
				nF.BasePath = "FCKeditor/";
				nF.BaseHref = "";
				nF.Width = 400;
				nF.Height = 200;
				nF.SkinPath = "skins/silver/";
				nF.ToolbarSet = "small";
				td.Controls.Add(nF);
			}

			for (int i = 0; i < arrLang.Length; i++)
			{
				HtmlTableCell td = (HtmlTableCell)FindControl("tdEditDesc_" + arrLang[i]);
				if (td == null)
					throw new ApplicationException("Critical error. No output cell found");
				FredCK.FCKeditorV2.FCKeditor nF = new FredCK.FCKeditorV2.FCKeditor();
				nF = new FredCK.FCKeditorV2.FCKeditor();
				nF.ID = "editor_fck_edit" + arrLang[i];
				nF.BasePath = "FCKeditor/";
				nF.BaseHref = "";
				nF.Width = 400;
				nF.Height = 200;
				nF.SkinPath = "skins/silver/";
				nF.ToolbarSet = "small";
				td.Controls.Add(nF);
			}

			for (int i = 0; i < arrLang.Length; i++)
			{
				HtmlTableCell td = (HtmlTableCell)FindControl("tdNewVariantDesc_" + arrLang[i]);
				if (td == null)
					throw new ApplicationException("Critical error. No output cell found");
				FredCK.FCKeditorV2.FCKeditor nF = new FredCK.FCKeditorV2.FCKeditor();
				nF = new FredCK.FCKeditorV2.FCKeditor();
				nF.ID = "editor_fck_variant" + arrLang[i];
				nF.BasePath = "FCKeditor/";
				nF.BaseHref = "";
				nF.Width = 400;
				nF.Height = 200;
				nF.SkinPath = "skins/silver/";
				nF.ToolbarSet = "small";
				td.Controls.Add(nF);
			}

			for (int i = 0; i < arrLang.Length; i++)
			{
				HtmlTableCell td = (HtmlTableCell)FindControl("tdEditVariantDesc_" + arrLang[i]);
				if (td == null)
					throw new ApplicationException("Critical error. No output cell found");
				FredCK.FCKeditorV2.FCKeditor nF = new FredCK.FCKeditorV2.FCKeditor();
				nF = new FredCK.FCKeditorV2.FCKeditor();
				nF.ID = "editor_fck_variant_edit" + arrLang[i];
				nF.BasePath = "FCKeditor/";
				nF.BaseHref = "";
				nF.Width = 400;
				nF.Height = 200;
				nF.SkinPath = "skins/silver/";
				nF.ToolbarSet = "small";
				td.Controls.Add(nF);
			}

			strQuery = "?page=" + Request.QueryString["page"].ToString();
			if(Shared.RqValue("submenu") != "")
				strQuery += "&submenu=" + Shared.RqValue("submenu");
			populateEquipmentList();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString["lang"] != null)
				intCurrentLang = Convert.ToInt16(Request.QueryString["lang"]);

			strQuery += "&lang=" + intCurrentLang.ToString();
		}

		/// <summary>
		/// event handler for button that saves a edited equipment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnEditEquipmentSubmit_Click(object sender, EventArgs e)
		{
			updateEquipment();
		}

		/// <summary>
		/// event handler for button that saves a NEW equipment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnNewEquipmentSubmit_Click(object sender, EventArgs e)
		{
			saveNewEquipment();
		}

		/// <summary>
		/// Delete an existing image
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ibtnDeleteExistingImage_Click(object sender, ImageClickEventArgs e)
		{
			int intDeleteId = 0;
			ImageButton imgB = (ImageButton)sender;
			int.TryParse(hideEquipmentId.Value, out intDeleteId);
			if (intDeleteId < 1)
				return;

			Database db = new Database();
			MySqlDataReader rs = db.select("SELECT ePicture FROM newEquipment WHERE eID = " + intDeleteId.ToString());
			if (rs.HasRows)
			{
				rs.Read();
				if (File.Exists(Server.MapPath("img_equipment\\" + rs[0].ToString())))
					File.Delete(Server.MapPath("img_equipment\\" + rs[0].ToString()));
				if (File.Exists(Server.MapPath("img_equipment\\thumb" + rs[0].ToString())))
					File.Delete(Server.MapPath("img_equipment\\thumb" + rs[0].ToString()));

			}
			rs.Close();
			db.objDataReader.Close();
			db.Dispose();
			db = new Database();
			db.execSql("UPDATE newEquipment SET ePicture = '' WHERE eID = " + intDeleteId.ToString());
			fileUplEditImage.Visible = true;
			imgExistingImage.Visible = false;
			ibtnDeleteExistingImage.Visible = false;
		}

		/// <summary>
		/// Delete equipment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void link_click_delete_equipment(object sender, ImageClickEventArgs e)
		{
			ImageButton lnkB = (ImageButton)sender;
			int intDeleteEquipmentId = 0;
			int.TryParse(lnkB.Attributes["deleteEquipment"], out intDeleteEquipmentId);
			if (intDeleteEquipmentId < 1)
				throw new ApplicationException("no equipment edit id found");

			Database db = new Database();
			MySqlDataReader RS = db.select("SELECT * FROM newEquipment WHERE eID = " + intDeleteEquipmentId.ToString());
			if (RS.HasRows)
			{
				RS.Read();
				string strFilename = RS["ePicture"].ToString();
				if (File.Exists(Server.MapPath("img_equipment\\" + strFilename)))
					File.Delete(Server.MapPath("img_equipment\\" + strFilename));
				if (File.Exists(Server.MapPath("img_equipment\\thumb" + strFilename)))
					File.Delete(Server.MapPath("img_equipment\\thumb" + strFilename));
			}
			RS.Close();
			db.objDataReader.Close();
			db.execSql("DELETE FROM newEquipment WHERE eID = " + intDeleteEquipmentId.ToString());
			db.execSql("DELETE FROM newEquipmentItem WHERE eiEquipment = " + intDeleteEquipmentId.ToString());
			db.Dispose();
			Response.Redirect(strQuery);
		}

		/// <summary>
		/// Delete equipment item (variant)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void link_click_delete_variant(object sender, ImageClickEventArgs e)
		{
			ImageButton lnkB = (ImageButton)sender;
			int intDeleteEquipmentId = 0;
			int.TryParse(lnkB.Attributes["deleteEquipmentItem"], out intDeleteEquipmentId);
			if (intDeleteEquipmentId < 1)
				throw new ApplicationException("no equipment edit id found");

			Database db = new Database();
			db.execSql("DELETE FROM newEquipmentItem WHERE eiID = " + intDeleteEquipmentId.ToString());
			db.Dispose();
			Response.Redirect(strQuery);
		}

		/// <summary>
		/// Generate form and load data for updating equipment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void link_click_edit_equipment(object sender, EventArgs e)
		{
			try {
				this.Main_form.DefaultButton = btnEditEquipmentSubmit.UniqueID;
			}
			catch (Exception) { }

			LinkButton lnkB = (LinkButton)sender;
			int intEditEquipmentId = 0;
			int.TryParse(lnkB.Attributes["editEquipment"], out intEditEquipmentId);
			if (intEditEquipmentId < 1)
				throw new ApplicationException("no equipment edit id found");

			hideEquipmentId.Value = intEditEquipmentId.ToString();
			pnlNewEqupiment.Visible = false;
			pnlExistingEquipment.Visible = false;
			pnlEditEqupiment.Visible = true;

			Database db = new Database();
			MySqlDataReader RS = db.select("SELECT * FROM newEquipment WHERE eID = " + intEditEquipmentId.ToString());

			if (RS.HasRows) {
				RS.Read();
				for (int i = 0; i < arrLang.Length; i++) {
					TextBox tbName = (TextBox)FindControl("tbEditEquipmentName_" + arrLang[i]);
					FredCK.FCKeditorV2.FCKeditor nf = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck_edit" + arrLang[i]);
					if (nf == null || tbName == null)
						return;

					tbName.Text = RS["eName_" + arrLang[i]].ToString();
					nf.Value = RS.GetString(RS.GetOrdinal("eDescription_" + arrLang[i])).ToString();
				}
				chkEditActive.Checked = RS.GetBoolean(RS.GetOrdinal("eActive"));
				if (RS["ePicture"].ToString().Length > 2)
				{
					imgExistingImage.ImageUrl = "../../img_equipment/" + RS["ePicture"].ToString();
					fileUplEditImage.Visible = false;
					imgExistingImage.Visible = true;
					ibtnDeleteExistingImage.Visible = true;
				}
			}
			RS.Close();
			db.objDataReader.Close();
		}

		/// <summary>
		/// Generate form and load data for updating equipment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void link_click_edit_equipment_item(object sender, EventArgs e)
		{
			
			try {
				this.Main_form.DefaultButton = btnEditVariantSubmit.UniqueID;
			}
			catch (Exception) { }

			LinkButton lnkB = (LinkButton)sender;
			int intEditEquipmentId = 0;
			int.TryParse(lnkB.Attributes["editItemId"], out intEditEquipmentId);
			if (intEditEquipmentId < 1)
				throw new ApplicationException("no equipment edit id found");

			hideEquipmentItem.Value = intEditEquipmentId.ToString();
			pnlNewEqupiment.Visible = false;
			pnlExistingEquipment.Visible = false;
			pnlEditEqupiment.Visible = false;
			pnlEditVariant.Visible = true;

			Database db = new Database();
			MySqlDataReader rs = db.select("SELECT eName_DK FROM newEquipment WHERE eID = " + hideEquipmentItem.Value);
			if (rs.HasRows)
			{
				rs.Read();
				litEditVariantParentEquipment.Text = "<b>\"" + rs[0].ToString() + "</b>\"";
			}
			db.objDataReader.Close();
			db.Dispose();

			db = new Database();
			MySqlDataReader RS = db.select("SELECT * FROM newEquipmentItem WHERE eiID = " + intEditEquipmentId.ToString());
			if (RS.HasRows)
			{
				RS.Read();
				for (int i = 0; i < arrLang.Length; i++)
				{
					TextBox tbPrice = (TextBox)FindControl("tbEditVariantPrice_" + arrLang[i].ToString());
					FredCK.FCKeditorV2.FCKeditor nf = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck_variant_edit" + arrLang[i]);
					if (nf == null || tbPrice == null)
						return;
					nf.Value = RS.GetString(RS.GetOrdinal("eiDescription_" + arrLang[i])).ToString();

					tbPrice.Text = RS["eiPrice_" + arrLang[i].ToString()].ToString().Replace(".", ",");
				}
				chkEditVariantActive.Checked = RS.GetBoolean(RS.GetOrdinal("eiActive"));
			}
			RS.Close();
			db.objDataReader.Close();
		}

		/// <summary>
		/// Handle click to create new variant
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void link_click_new_variant(object sender, EventArgs e)
		{
			try {
				this.Main_form.DefaultButton = btnNewVariantSubmit.UniqueID;
			}
			catch (Exception) { }
			int intParentEquipmentId = 0;
			LinkButton lnkB = (LinkButton)sender;
			int.TryParse(lnkB.Attributes["parentEquipment"], out intParentEquipmentId);
			if(intParentEquipmentId < 1)
				throw new ApplicationException("No parent equipment id found when trying to add new variant!");

			hideVariantParentEquipment.Value = intParentEquipmentId.ToString();
			pnlNewVariant.Visible = true;
			pnlExistingEquipment.Visible = false;

			Database db = new Database();
			MySqlDataReader rs = db.select("SELECT eName_DK FROM newEquipment WHERE eID = " + hideVariantParentEquipment.Value);
			if (rs.HasRows)
			{
				rs.Read();
				litNewVariantParentEquipment.Text = "<b>\"" + rs[0].ToString() + "</b>\"";
			}
			db.objDataReader.Close();
			db.Dispose();
		}

		protected void lnkBackToList_Click(object sender, EventArgs e)
		{
			Response.Redirect(strQuery);
		}
		
		// New equipment
		protected void lnkMakeNewEquipment_Click(object sender, EventArgs e)
		{
			pnlExistingEquipment.Visible = false;
			pnlNewEqupiment.Visible = true;
			hideEquipmentId.Value = "new";
			this.Main_form.DefaultButton = btnNewEquipmentSubmit.UniqueID;
		}

		/// <summary>
		/// Populates the list of existing equipment
		/// </summary>
		private void populateEquipmentList() {
			StringBuilder sb = new StringBuilder();
			Database db = new Database();
			int intTempId = -1;
			string strSuffix = "_DK";
			if(Shared.UserIsDist()) {
				//strSuffix = "_" + Shared.DbLangs[Shared.UserLang()];
			}
			
			string strSQL = "SELECT eID, eName" + strSuffix + ", eDescription" + strSuffix + ", eActive, eiActive," +
							" eiID, eiEquipment," +
							" eiDescription" + strSuffix + ", eiPrice" + strSuffix + ", ePicture" +
							" FROM newEquipment " +
							" LEFT JOIN newEquipmentItem ON eiEquipment = eID" +
							" ORDER BY CAST(eName_DK AS CHAR)"; 
			MySqlDataReader RS = db.select(strSQL);
			
			while (RS.Read())
			{
				if (intTempId != Convert.ToInt32(RS["eID"]))
				{
					intTempId = Convert.ToInt32(RS["eID"]);
					TableRow trHeader = new TableRow();
					TableCell tdAdd = new TableCell();
					TableCell tdDescription = new TableCell();
					TableCell tdDelete = new TableCell();

					LinkButton lnkEditImage = new LinkButton();
					lnkEditImage.ID = "lnkEditImage" + intTempId.ToString();
					lnkEditImage.Attributes.Add("editEquipmentItem", "new");
					lnkEditImage.Attributes.Add("parentEquipment", RS[0].ToString());
					lnkEditImage.Text = "<img src=\"gfx/new.gif\" alt=\"Ny variant...\">";
					lnkEditImage.Click += new EventHandler(link_click_new_variant);

					LinkButton lnkEditName = new LinkButton();
					lnkEditName.ID = "lnkEditName" + intTempId.ToString();
					lnkEditName.CssClass = "leavealone";
					lnkEditName.Attributes.Add("editEquipment", RS[0].ToString());
					lnkEditName.Attributes.Add("parentEquipment", RS[0].ToString());
					lnkEditName.Text = "&nbsp;" + RS["eName" + strSuffix].ToString();
					if (RS.GetBoolean(RS.GetOrdinal("eActive")) == false) {
						lnkEditName.Text = "<span style=\"color:#ddd;\">" + lnkEditName.Text + "</span>";
						lnkEditName.ToolTip = "Deaktiveret....";
					}
					if (RS["ePicture"].ToString().Length > 2)
						lnkEditName.Text += " <img src=\"img_equipment/" + RS["ePicture"].ToString() + "\" width=\"30\">";

					lnkEditName.Click += new EventHandler(link_click_edit_equipment);

					trHeader.CssClass = "trEquipmentHeader data_table_header";
					tdAdd.VerticalAlign = VerticalAlign.Top;

					tdAdd.Controls.Add(lnkEditImage);
					tdAdd.Controls.Add(lnkEditName);

					tdDescription.Text = RS.GetString(RS.GetOrdinal("eDescription" + strSuffix));

					ImageButton ibtnDelete = new ImageButton();
					ibtnDelete.ID = "ibtnDelete" + intTempId.ToString();
					ibtnDelete.ImageUrl = "../../gfx/delete.gif";
					ibtnDelete.Attributes.Add("deleteEquipment", RS[0].ToString());
					ibtnDelete.OnClientClick = "return sureDelete('equipment');";
					ibtnDelete.ToolTip = "Slet udstyr";
					ibtnDelete.Style.Add("border-width", "0px");
					ibtnDelete.Click += new ImageClickEventHandler(link_click_delete_equipment);

					tdDelete.VerticalAlign = VerticalAlign.Top;

					tdDelete.Controls.Add(ibtnDelete);
					trHeader.Cells.Add(tdAdd);
					trHeader.Cells.Add(tdDescription);
					trHeader.Cells.Add(tdDelete);
					
					tblExistingEquipment.Rows.Add(trHeader);
				}
				if (RS["eiID"] == DBNull.Value)
					continue;

				TableRow trItem = new TableRow();
				trItem.CssClass = "trEquipmentItem";

				TableCell tdEmpty = new TableCell();
				TableCell tdOne = new TableCell();
				TableCell tdTwo = new TableCell();
				LinkButton lnkEditEi = new LinkButton();
				lnkEditEi.ID = "lnkEditEquipmentItem" + RS["eiID"].ToString();
				lnkEditEi.CssClass = "leavealone";
				lnkEditEi.Attributes.Add("editItemId", RS["eiID"].ToString());
				lnkEditEi.Text = "<li>";
				
				if (RS.GetBoolean(RS.GetOrdinal("eiActive")) == false) {
					lnkEditEi.Text += "<span style=\"color:#ddd;\">";
					lnkEditEi.ToolTip = "Deaktiveret....";
				}
				lnkEditEi.Text += RS.GetString(RS.GetOrdinal("eiDescription" + strSuffix)).ToString() +
					"<br><b>Pris: " + RS["eiPrice" + strSuffix].ToString() + "</b>";
				
				if (RS.GetBoolean(RS.GetOrdinal("eiActive")) == false)
					lnkEditEi.Text += "</span>";

				lnkEditEi.Text += "</li>";
				lnkEditEi.Click += new EventHandler(link_click_edit_equipment_item);

				ImageButton ibtnDeleteItem = new ImageButton();
				ibtnDeleteItem.ID = "ibtnDeleteEi" + RS["eiID"].ToString();
				ibtnDeleteItem.ImageUrl = "../../gfx/delete.gif";
				ibtnDeleteItem.Attributes.Add("onclick", "return sureDelete('equipmentItem');");
				ibtnDeleteItem.Attributes.Add("onmouseover", "colorizeTableRow(true, this);");
				ibtnDeleteItem.Attributes.Add("onmouseout", "colorizeTableRow(false, this);");
				ibtnDeleteItem.Attributes.Add("deleteEquipmentItem", RS["eiID"].ToString());
				ibtnDeleteItem.Style.Add("border-width", "0px");
				ibtnDeleteItem.ToolTip = "Slet variant...";
				ibtnDeleteItem.Click += new ImageClickEventHandler(link_click_delete_variant);

				tdOne.Controls.Add(lnkEditEi);
				tdTwo.Controls.Add(ibtnDeleteItem);

				trItem.Cells.Add(tdEmpty);
				trItem.Cells.Add(tdOne);
				trItem.Cells.Add(tdTwo);
				tblExistingEquipment.Rows.Add(trItem);
			}
			db.objDataReader.Close();
			db = null;
		}
	
		/// <summary>
		/// Method to save new edited equipment to database
		/// </summary>
		private void updateEquipment()
		{
			string strFilename = "";
			string strExtension = "";
			string strFilenamePrefix = "";
			int intFilenamePrefix = 0;
			int intTempSaveId = 0;
			int.TryParse(hideEquipmentId.Value, out intTempSaveId);
			if(intTempSaveId < 1)
				throw new ApplicationException("Kritisk fejl. Ingen update ID fundet ved redigering af udstyr (equipment)");

			if (fileUplEditImage.HasFile)
			{
				strFilename = fileUplEditImage.FileName;
				strExtension = strFilename.Substring(strFilename.LastIndexOf(".") + 1);
				if (strExtension == "jpg" || strExtension == "png" || strExtension == "tif" || strExtension == "tiff" || strExtension == "gif" || strExtension == "bmp") { 
				} else {
					throw new ApplicationException("Kun jpg, tif, gif, png og bmp formater tilladt");
				}

				// check for file extension RIGHT HERE
				if (File.Exists(Server.MapPath("img_equipment\\_" + strFilename)))
				{
					intFilenamePrefix = 1;
					while (File.Exists(Server.MapPath("img_equipment\\_" + intFilenamePrefix.ToString() + strFilename)))
					{
						intFilenamePrefix += 1;
					}
				}
				if (intFilenamePrefix > 0)
					strFilenamePrefix = intFilenamePrefix.ToString();

				fileUplEditImage.SaveAs(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename));
			}

			string strDistEditLang = "";
			if (Shared.UserIsDist()) {
				strDistEditLang = arrLang[Shared.UserLang() - 1];
			}

			string strSQL = "UPDATE newEquipment SET" +
					" eName_DK = '" + noSQ(tbEditEquipmentName_DK.Text) + "'," +
					" eName_NO = '" + noSQ(tbEditEquipmentName_NO.Text) + "'," +
					" eName_UK = '" + noSQ(tbEditEquipmentName_UK.Text) + "'," + 
					" eName_DE = '" + noSQ(tbEditEquipmentName_DE.Text) + "',";

			for (int i = 0; i < arrLang.Length; i++) {
				FredCK.FCKeditorV2.FCKeditor editor = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck_edit" + arrLang[i]);
				if (editor == null)
					throw new ApplicationException("Critical error. No fckeditor found");
				strSQL += " eDescription_" + arrLang[i] + " = '" + noSQ(editor.Value.ToString()) + "',";
			}

			strSQL += " eActive = " + (chkEditActive.Checked ? "1" : "0");
			if (fileUplEditImage.HasFile)
			{
				strSQL += ", ePicture = '_" + noSQ(strFilenamePrefix + strFilename) + "'";
				ImageProcessing.ImageProcessing iP = new ImageProcessing.ImageProcessing();
				iP.ResizeImageAndSaveToFile(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename), new int[] { 1024, 768 }, System.Drawing.Imaging.ImageFormat.Jpeg, 95, 72, Server.MapPath("img_equipment\\_" + strFilenamePrefix + strFilename));
				iP.ResizeImageAndSaveToFile(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename), new int[] { 150, 100 }, System.Drawing.Imaging.ImageFormat.Jpeg, 95, 72, Server.MapPath("img_equipment\\thumb_" + strFilenamePrefix + strFilename));
				if (File.Exists(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename)))
				{
					File.Delete(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename));
				}
			}
			strSQL += " WHERE eID = " + hideEquipmentId.Value.ToString();  
			Database db = new Database();
			db.execSql(strSQL);
			db.dbDispose();
			Response.Redirect(strQuery);
		}

		/// <summary>
		/// Method to save new NEW equipment to database
		/// </summary>
		private void saveNewEquipment()
		{
			string strFilename = "";
			string strExtension = "";
			string strFilenamePrefix = "";
			int intFilenamePrefix = 0;

			if (fileUplNewImage.HasFile)
			{
				strFilename = fileUplNewImage.FileName;
				strExtension = strFilename.Substring(strFilename.LastIndexOf(".") + 1);
				if (strExtension == "jpg" || strExtension == "tif" || strExtension == "tiff" || strExtension == "gif" || strExtension == "bmp")
				{ } 
				else {
					throw new ApplicationException("Kun jpg, tif, gif og bmp formater tilladt");
				}

				// check for file extension RIGHT HERE
				if (File.Exists(Server.MapPath("img_equipment\\_" + strFilename)))
				{
					intFilenamePrefix = 1;
					while (File.Exists(Server.MapPath("img_equipment\\_" + intFilenamePrefix.ToString() + strFilename)))
					{
						intFilenamePrefix += 1;
					}
				}
				if (intFilenamePrefix > 0)
					strFilenamePrefix = intFilenamePrefix.ToString();

				fileUplNewImage.SaveAs(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename));
			}

			string strSQL = "INSERT INTO newEquipment(eName_DK, eName_NO, eName_UK, eName_DE," +
				" eDescription_DK, eDescription_NO, eDescription_UK, eDescription_DE, eActive, ePicture)" +
				" VALUES(";
			strSQL += "'" + noSQ(tbNewEquipmentName_DK.Text) + "', ";
			strSQL += "'" + noSQ(tbNewEquipmentName_NO.Text) + "', ";
			strSQL += "'" + noSQ(tbNewEquipmentName_UK.Text) + "', ";
			strSQL += "'" + noSQ(tbNewEquipmentName_DE.Text) + "', ";

			// now for descriptions
			for (int i = 0; i < arrLang.Length; i++)
			{
				FredCK.FCKeditorV2.FCKeditor editor = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck" + arrLang[i]);
				if (editor == null)
					throw new ApplicationException("Critical error. No fckeditor found");
				strSQL += "'" + noSQ(editor.Value.ToString()) + "', ";
			}
			strSQL += (chkActive.Checked ? "1" : "0") + ", ";
			strSQL += "'_" + noSQ(strFilenamePrefix + strFilename) + "')";
			if (fileUplNewImage.HasFile)
			{
				ImageProcessing.ImageProcessing iP = new ImageProcessing.ImageProcessing();
				iP.ResizeImageAndSaveToFile(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename), new int[] { 1024, 768 }, System.Drawing.Imaging.ImageFormat.Jpeg, 95, 72, Server.MapPath("img_equipment\\_" + strFilenamePrefix + strFilename));
				iP.ResizeImageAndSaveToFile(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename), new int[] { 150, 100 }, System.Drawing.Imaging.ImageFormat.Jpeg, 95, 72, Server.MapPath("img_equipment\\thumb_" + strFilenamePrefix + strFilename));
				if (File.Exists(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename)))
				{
					File.Delete(Server.MapPath("img_equipment\\" + strFilenamePrefix + strFilename));
				}
			}
			Database db = new Database();
			db.execSql(strSQL);
			db.dbDispose();
			Response.Redirect(strQuery);
		}

		private void saveNewVariant()
		{
			int intTempHideVariantParentEquipment = 0;
			int.TryParse(hideVariantParentEquipment.Value, out intTempHideVariantParentEquipment);
			if (intTempHideVariantParentEquipment < 1)
				throw new ApplicationException("no parent equipment id found upon saving new variant!");

			string strSQL = "INSERT INTO newEquipmentItem(eiEquipment," +
				" eiDescription_DK, eiDescription_NO, eiDescription_UK, eiDescription_DE," +
				" eiPrice_DK, eiPrice_NO, eiPrice_UK, eiPrice_DE, eiActive) VALUES(";

			strSQL += hideVariantParentEquipment.Value.ToString() + ", ";
			
			for (int i = 0; i < arrLang.Length; i++)
			{
				FredCK.FCKeditorV2.FCKeditor nf = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck_variant" + arrLang[i]);
				if (nf == null)
					return;

				strSQL += "'" + noSQ(nf.Value) + "', ";
			}
			strSQL += "'0" + tbNewVariantPrice_DK.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "'0" + tbNewVariantPrice_NO.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "'0" + tbNewVariantPrice_UK.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "'0" + tbNewVariantPrice_DE.Text.ToString().Replace(",", ".") + "', ";
			strSQL += (chkNewVariantActive.Checked ? "1" : "0") + ")";

			Database db = new Database();
			db.execSql(strSQL);
			db.objConnection.Close();
			db.Dispose();
			Response.Redirect(strQuery);
		}

		private void updateVariant()
		{
			int intTempHideVariantId = 0;
			int.TryParse(hideEquipmentItem.Value, out intTempHideVariantId);
			if (intTempHideVariantId < 1)
				throw new ApplicationException("no parent equipment id found upon saving new variant!");

			string strSQL = "UPDATE newEquipmentItem SET ";

			for (int i = 0; i < arrLang.Length; i++)
			{
				FredCK.FCKeditorV2.FCKeditor editor = (FredCK.FCKeditorV2.FCKeditor)FindControl("editor_fck_variant_edit" + arrLang[i]);
				if (editor == null)
					throw new ApplicationException("Critical error. No fckeditor found");
				strSQL += " eiDescription_" + arrLang[i] + " = '" + noSQ(editor.Value.ToString()) + "',";
			}

			strSQL += "eiPrice_DK = '" + tbEditVariantPrice_DK.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "eiPrice_NO = '" + tbEditVariantPrice_NO.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "eiPrice_UK = '" + tbEditVariantPrice_UK.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "eiPrice_DE = '" + tbEditVariantPrice_DE.Text.ToString().Replace(",", ".") + "', ";
			strSQL += "eiActive = " + (chkEditVariantActive.Checked ? "1" : "0");
			strSQL += " WHERE eiID = " + intTempHideVariantId.ToString();

			Database db = new Database();
			db.execSql(strSQL);
			db.objConnection.Close();
			db.Dispose();
			Response.Redirect(strQuery);
		}

		private string noSQ(string s)
		{
			return (s + "").ToString().Replace("'", "''");
		}

		protected void btnNewVariantSubmit_Click(object sender, EventArgs e)
		{
			saveNewVariant();
		}
		protected void btnEditVariantSubmit_Click(object sender, EventArgs e)
		{
			updateVariant(); 
		}
}
}
