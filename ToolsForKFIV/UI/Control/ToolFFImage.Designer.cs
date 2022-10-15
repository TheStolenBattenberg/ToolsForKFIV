
namespace ToolsForKFIV.UI.Control
{
    partial class ToolFFImage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolFFImage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tiTabView = new System.Windows.Forms.TabControl();
            this.tiPreview = new System.Windows.Forms.TabPage();
            this.itHSplit = new System.Windows.Forms.SplitContainer();
            this.itPicture = new ToolsForKFIV.UI.Control.PictureBoxUnfiltered();
            this.itVSplit = new System.Windows.Forms.SplitContainer();
            this.itImageList = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.itIData = new System.Windows.Forms.DataGridView();
            this.idKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itCData = new System.Windows.Forms.DataGridView();
            this.cdKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tiExport = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.itBtDoExport = new System.Windows.Forms.Button();
            this.itbtExpSelPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ittbExpPath = new System.Windows.Forms.TextBox();
            this.itcbExpFmt = new System.Windows.Forms.ComboBox();
            this.tiTabView.SuspendLayout();
            this.tiPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itHSplit)).BeginInit();
            this.itHSplit.Panel1.SuspendLayout();
            this.itHSplit.Panel2.SuspendLayout();
            this.itHSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itVSplit)).BeginInit();
            this.itVSplit.Panel1.SuspendLayout();
            this.itVSplit.Panel2.SuspendLayout();
            this.itVSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itIData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itCData)).BeginInit();
            this.tiExport.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tiTabView
            // 
            this.tiTabView.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tiTabView.Controls.Add(this.tiPreview);
            this.tiTabView.Controls.Add(this.tiExport);
            this.tiTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tiTabView.Location = new System.Drawing.Point(0, 0);
            this.tiTabView.Name = "tiTabView";
            this.tiTabView.SelectedIndex = 0;
            this.tiTabView.Size = new System.Drawing.Size(512, 512);
            this.tiTabView.TabIndex = 0;
            // 
            // tiPreview
            // 
            this.tiPreview.Controls.Add(this.itHSplit);
            this.tiPreview.Location = new System.Drawing.Point(4, 27);
            this.tiPreview.Name = "tiPreview";
            this.tiPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tiPreview.Size = new System.Drawing.Size(504, 481);
            this.tiPreview.TabIndex = 0;
            this.tiPreview.Text = "Preview";
            this.tiPreview.UseVisualStyleBackColor = true;
            // 
            // itHSplit
            // 
            this.itHSplit.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.itHSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itHSplit.Location = new System.Drawing.Point(3, 3);
            this.itHSplit.Name = "itHSplit";
            // 
            // itHSplit.Panel1
            // 
            this.itHSplit.Panel1.Controls.Add(this.itPicture);
            this.itHSplit.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            // 
            // itHSplit.Panel2
            // 
            this.itHSplit.Panel2.Controls.Add(this.itVSplit);
            this.itHSplit.Size = new System.Drawing.Size(498, 475);
            this.itHSplit.SplitterDistance = 337;
            this.itHSplit.TabIndex = 0;
            // 
            // itPicture
            // 
            this.itPicture.BackColor = System.Drawing.SystemColors.Control;
            this.itPicture.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("itPicture.BackgroundImage")));
            this.itPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.itPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itPicture.Location = new System.Drawing.Point(0, 0);
            this.itPicture.Name = "itPicture";
            this.itPicture.Size = new System.Drawing.Size(337, 475);
            this.itPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.itPicture.TabIndex = 2;
            this.itPicture.TabStop = false;
            // 
            // itVSplit
            // 
            this.itVSplit.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.itVSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itVSplit.Location = new System.Drawing.Point(0, 0);
            this.itVSplit.Name = "itVSplit";
            this.itVSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // itVSplit.Panel1
            // 
            this.itVSplit.Panel1.Controls.Add(this.itImageList);
            this.itVSplit.Panel1MinSize = 96;
            // 
            // itVSplit.Panel2
            // 
            this.itVSplit.Panel2.Controls.Add(this.splitContainer1);
            this.itVSplit.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.itVSplit.Size = new System.Drawing.Size(157, 475);
            this.itVSplit.SplitterDistance = 208;
            this.itVSplit.TabIndex = 0;
            // 
            // itImageList
            // 
            this.itImageList.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.itImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itImageList.FormattingEnabled = true;
            this.itImageList.ItemHeight = 15;
            this.itImageList.Location = new System.Drawing.Point(0, 0);
            this.itImageList.Name = "itImageList";
            this.itImageList.Size = new System.Drawing.Size(157, 208);
            this.itImageList.TabIndex = 0;
            this.itImageList.SelectedIndexChanged += new System.EventHandler(this.itImageList_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.itIData);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.itCData);
            this.splitContainer1.Size = new System.Drawing.Size(157, 263);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 0;
            // 
            // itIData
            // 
            this.itIData.AllowUserToAddRows = false;
            this.itIData.AllowUserToDeleteRows = false;
            this.itIData.AllowUserToResizeColumns = false;
            this.itIData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.itIData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.itIData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.itIData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itIData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idKey,
            this.idValue});
            this.itIData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.itIData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itIData.Location = new System.Drawing.Point(0, 0);
            this.itIData.Name = "itIData";
            this.itIData.ReadOnly = true;
            this.itIData.RowHeadersVisible = false;
            this.itIData.Size = new System.Drawing.Size(157, 130);
            this.itIData.TabIndex = 0;
            // 
            // idKey
            // 
            this.idKey.HeaderText = "Property";
            this.idKey.Name = "idKey";
            this.idKey.ReadOnly = true;
            // 
            // idValue
            // 
            this.idValue.HeaderText = "Value";
            this.idValue.Name = "idValue";
            this.idValue.ReadOnly = true;
            // 
            // itCData
            // 
            this.itCData.AllowUserToAddRows = false;
            this.itCData.AllowUserToDeleteRows = false;
            this.itCData.AllowUserToResizeColumns = false;
            this.itCData.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.itCData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.itCData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.itCData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itCData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cdKey,
            this.cdValue});
            this.itCData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.itCData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itCData.Location = new System.Drawing.Point(0, 0);
            this.itCData.Name = "itCData";
            this.itCData.ReadOnly = true;
            this.itCData.RowHeadersVisible = false;
            this.itCData.RowTemplate.Height = 25;
            this.itCData.Size = new System.Drawing.Size(157, 129);
            this.itCData.TabIndex = 0;
            // 
            // cdKey
            // 
            this.cdKey.HeaderText = "Property";
            this.cdKey.Name = "cdKey";
            this.cdKey.ReadOnly = true;
            // 
            // cdValue
            // 
            this.cdValue.HeaderText = "Value";
            this.cdValue.Name = "cdValue";
            this.cdValue.ReadOnly = true;
            // 
            // tiExport
            // 
            this.tiExport.Controls.Add(this.groupBox2);
            this.tiExport.Controls.Add(this.groupBox1);
            this.tiExport.Location = new System.Drawing.Point(4, 27);
            this.tiExport.Name = "tiExport";
            this.tiExport.Padding = new System.Windows.Forms.Padding(3);
            this.tiExport.Size = new System.Drawing.Size(504, 481);
            this.tiExport.TabIndex = 1;
            this.tiExport.Text = "Export";
            this.tiExport.UseVisualStyleBackColor = true;
            this.tiExport.Enter += new System.EventHandler(this.tiExport_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(6, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 362);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Additional Settings...";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.itBtDoExport);
            this.groupBox1.Controls.Add(this.itbtExpSelPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ittbExpPath);
            this.groupBox1.Controls.Add(this.itcbExpFmt);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 107);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export...";
            // 
            // itBtDoExport
            // 
            this.itBtDoExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.itBtDoExport.Location = new System.Drawing.Point(411, 78);
            this.itBtDoExport.Name = "itBtDoExport";
            this.itBtDoExport.Size = new System.Drawing.Size(75, 23);
            this.itBtDoExport.TabIndex = 3;
            this.itBtDoExport.Text = "Do it";
            this.itBtDoExport.UseVisualStyleBackColor = true;
            this.itBtDoExport.Click += new System.EventHandler(this.itBtDoExport_Click);
            // 
            // itbtExpSelPath
            // 
            this.itbtExpSelPath.Location = new System.Drawing.Point(7, 22);
            this.itbtExpSelPath.Name = "itbtExpSelPath";
            this.itbtExpSelPath.Size = new System.Drawing.Size(161, 23);
            this.itbtExpSelPath.TabIndex = 2;
            this.itbtExpSelPath.Text = "Browse Export Path";
            this.itbtExpSelPath.UseVisualStyleBackColor = true;
            this.itbtExpSelPath.Click += new System.EventHandler(this.itbtExpSelPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Export Format:";
            // 
            // ittbExpPath
            // 
            this.ittbExpPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ittbExpPath.Location = new System.Drawing.Point(174, 22);
            this.ittbExpPath.Name = "ittbExpPath";
            this.ittbExpPath.Size = new System.Drawing.Size(312, 23);
            this.ittbExpPath.TabIndex = 0;
            // 
            // itcbExpFmt
            // 
            this.itcbExpFmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itcbExpFmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itcbExpFmt.Location = new System.Drawing.Point(174, 51);
            this.itcbExpFmt.Name = "itcbExpFmt";
            this.itcbExpFmt.Size = new System.Drawing.Size(312, 23);
            this.itcbExpFmt.TabIndex = 0;
            // 
            // ToolFFImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tiTabView);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "ToolFFImage";
            this.Size = new System.Drawing.Size(512, 512);
            this.tiTabView.ResumeLayout(false);
            this.tiPreview.ResumeLayout(false);
            this.itHSplit.Panel1.ResumeLayout(false);
            this.itHSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itHSplit)).EndInit();
            this.itHSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itPicture)).EndInit();
            this.itVSplit.Panel1.ResumeLayout(false);
            this.itVSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itVSplit)).EndInit();
            this.itVSplit.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itIData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itCData)).EndInit();
            this.tiExport.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tiTabView;
        private System.Windows.Forms.TabPage tiPreview;
        private System.Windows.Forms.TabPage tiExport;
        private System.Windows.Forms.SplitContainer itHSplit;
        private System.Windows.Forms.SplitContainer itVSplit;
        private System.Windows.Forms.ListBox itImageList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView itIData;
        private System.Windows.Forms.DataGridView itCData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button itBtDoExport;
        private System.Windows.Forms.Button itbtExpSelPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ittbExpPath;
        private System.Windows.Forms.ComboBox itcbExpFmt;
        private PictureBoxUnfiltered itPicture;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn idKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn idValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cdKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn cdValue;
    }
}
