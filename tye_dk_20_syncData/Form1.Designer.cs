namespace tye_dk_20_syncData {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tbConnSource = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbConnTarget = new System.Windows.Forms.TextBox();
			this.tbLog = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.lbDropTables = new System.Windows.Forms.CheckedListBox();
			this.btnLoadTables = new System.Windows.Forms.Button();
			this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
			this.lnkSelectNone = new System.Windows.Forms.LinkLabel();
			this.btnGoSelectedTables = new System.Windows.Forms.Button();
			this.btnDeleteAll = new System.Windows.Forms.Button();
			this.btnTruncate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbConnSource
			// 
			this.tbConnSource.Location = new System.Drawing.Point(12, 36);
			this.tbConnSource.Name = "tbConnSource";
			this.tbConnSource.Size = new System.Drawing.Size(1073, 22);
			this.tbConnSource.TabIndex = 0;
			this.tbConnSource.Text = "data source=.\\SQLEXPRESS;attachdbfilename=E:\\Development\\clients\\tye\\tye_dk_2.0\\A" +
    "pp_Data\\tye2.mdf;integrated security=True;user instance=True;multipleactiveresul" +
    "tsets=True;App=EntityFramework";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(161, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Connectionstring source";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(155, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Connectionstring target";
			// 
			// tbConnTarget
			// 
			this.tbConnTarget.Location = new System.Drawing.Point(12, 97);
			this.tbConnTarget.Name = "tbConnTarget";
			this.tbConnTarget.Size = new System.Drawing.Size(1073, 22);
			this.tbConnTarget.TabIndex = 2;
			this.tbConnTarget.Text = "data source=mssql3.unoeuro.com;initial catalog=tye_dk_db;persist security info=Tr" +
    "ue;user id=tye_dk;password=a3z4chdg;multipleactiveresultsets=True;App=EntityFram" +
    "ework";
			// 
			// tbLog
			// 
			this.tbLog.Location = new System.Drawing.Point(13, 166);
			this.tbLog.Multiline = true;
			this.tbLog.Name = "tbLog";
			this.tbLog.Size = new System.Drawing.Size(742, 431);
			this.tbLog.TabIndex = 4;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(12, 617);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(156, 23);
			this.btnGo.TabIndex = 5;
			this.btnGo.Text = "Go all tables!";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(773, 555);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(313, 42);
			this.button1.TabIndex = 7;
			this.button1.Text = "Drop selected tables from target";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lbDropTables
			// 
			this.lbDropTables.FormattingEnabled = true;
			this.lbDropTables.Location = new System.Drawing.Point(773, 251);
			this.lbDropTables.Name = "lbDropTables";
			this.lbDropTables.Size = new System.Drawing.Size(312, 293);
			this.lbDropTables.TabIndex = 8;
			// 
			// btnLoadTables
			// 
			this.btnLoadTables.Location = new System.Drawing.Point(772, 166);
			this.btnLoadTables.Name = "btnLoadTables";
			this.btnLoadTables.Size = new System.Drawing.Size(313, 42);
			this.btnLoadTables.TabIndex = 9;
			this.btnLoadTables.Text = "Load target db tables";
			this.btnLoadTables.UseVisualStyleBackColor = true;
			this.btnLoadTables.Click += new System.EventHandler(this.btnLoadTables_Click);
			// 
			// lnkSelectAll
			// 
			this.lnkSelectAll.AutoSize = true;
			this.lnkSelectAll.Location = new System.Drawing.Point(773, 215);
			this.lnkSelectAll.Name = "lnkSelectAll";
			this.lnkSelectAll.Size = new System.Drawing.Size(65, 17);
			this.lnkSelectAll.TabIndex = 10;
			this.lnkSelectAll.TabStop = true;
			this.lnkSelectAll.Text = "Select all";
			this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
			// 
			// lnkSelectNone
			// 
			this.lnkSelectNone.AutoSize = true;
			this.lnkSelectNone.Location = new System.Drawing.Point(875, 215);
			this.lnkSelectNone.Name = "lnkSelectNone";
			this.lnkSelectNone.Size = new System.Drawing.Size(83, 17);
			this.lnkSelectNone.TabIndex = 11;
			this.lnkSelectNone.TabStop = true;
			this.lnkSelectNone.Text = "Select none";
			this.lnkSelectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectNone_LinkClicked);
			// 
			// btnGoSelectedTables
			// 
			this.btnGoSelectedTables.Location = new System.Drawing.Point(213, 617);
			this.btnGoSelectedTables.Name = "btnGoSelectedTables";
			this.btnGoSelectedTables.Size = new System.Drawing.Size(179, 23);
			this.btnGoSelectedTables.TabIndex = 12;
			this.btnGoSelectedTables.Text = "Go selected tables!";
			this.btnGoSelectedTables.UseVisualStyleBackColor = true;
			this.btnGoSelectedTables.Click += new System.EventHandler(this.btnGoSelectedTables_Click);
			// 
			// btnDeleteAll
			// 
			this.btnDeleteAll.Location = new System.Drawing.Point(436, 617);
			this.btnDeleteAll.Name = "btnDeleteAll";
			this.btnDeleteAll.Size = new System.Drawing.Size(221, 23);
			this.btnDeleteAll.TabIndex = 13;
			this.btnDeleteAll.Text = "Delete all data from selected tables";
			this.btnDeleteAll.UseVisualStyleBackColor = true;
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
			// 
			// btnTruncate
			// 
			this.btnTruncate.Location = new System.Drawing.Point(699, 617);
			this.btnTruncate.Name = "btnTruncate";
			this.btnTruncate.Size = new System.Drawing.Size(221, 23);
			this.btnTruncate.TabIndex = 14;
			this.btnTruncate.Text = "Truncate selected tables";
			this.btnTruncate.UseVisualStyleBackColor = true;
			this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1098, 652);
			this.Controls.Add(this.btnTruncate);
			this.Controls.Add(this.btnDeleteAll);
			this.Controls.Add(this.btnGoSelectedTables);
			this.Controls.Add(this.lnkSelectNone);
			this.Controls.Add(this.lnkSelectAll);
			this.Controls.Add(this.btnLoadTables);
			this.Controls.Add(this.lbDropTables);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.tbLog);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbConnTarget);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbConnSource);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbConnSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbConnTarget;
		private System.Windows.Forms.TextBox tbLog;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckedListBox lbDropTables;
		private System.Windows.Forms.Button btnLoadTables;
		private System.Windows.Forms.LinkLabel lnkSelectAll;
		private System.Windows.Forms.LinkLabel lnkSelectNone;
		private System.Windows.Forms.Button btnGoSelectedTables;
		private System.Windows.Forms.Button btnDeleteAll;
		private System.Windows.Forms.Button btnTruncate;
	}
}

