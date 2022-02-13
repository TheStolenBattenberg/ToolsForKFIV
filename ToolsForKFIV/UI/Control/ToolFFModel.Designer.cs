
using OpenTK.WinForms;

namespace ToolsForKFIV.UI.Control
{
    partial class ToolFFModel
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
            this.tmTabView = new System.Windows.Forms.TabControl();
            this.tmTvPreview = new System.Windows.Forms.TabPage();
            this.tmHAlign1 = new System.Windows.Forms.SplitContainer();
            this.tmOGL = new OpenTK.WinForms.GLControl();
            this.tmVAlign = new System.Windows.Forms.SplitContainer();
            this.tmLbMeshes = new System.Windows.Forms.CheckedListBox();
            this.tmDgModelProperties = new System.Windows.Forms.DataGridView();
            this.mtdgKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mtdgValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tmTvExport = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tmBtExport = new System.Windows.Forms.Button();
            this.itBtDoExport = new System.Windows.Forms.Button();
            this.tmBtSelExpPath = new System.Windows.Forms.Button();
            this.tmTxExpFormat = new System.Windows.Forms.Label();
            this.tmTbExpPath = new System.Windows.Forms.TextBox();
            this.tmLbExpFormat = new System.Windows.Forms.ComboBox();
            this.tmTabView.SuspendLayout();
            this.tmTvPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmHAlign1)).BeginInit();
            this.tmHAlign1.Panel1.SuspendLayout();
            this.tmHAlign1.Panel2.SuspendLayout();
            this.tmHAlign1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmVAlign)).BeginInit();
            this.tmVAlign.Panel1.SuspendLayout();
            this.tmVAlign.Panel2.SuspendLayout();
            this.tmVAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmDgModelProperties)).BeginInit();
            this.tmTvExport.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmTabView
            // 
            this.tmTabView.Controls.Add(this.tmTvPreview);
            this.tmTabView.Controls.Add(this.tmTvExport);
            this.tmTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmTabView.Location = new System.Drawing.Point(0, 0);
            this.tmTabView.Name = "tmTabView";
            this.tmTabView.SelectedIndex = 0;
            this.tmTabView.Size = new System.Drawing.Size(512, 512);
            this.tmTabView.TabIndex = 0;
            // 
            // tmTvPreview
            // 
            this.tmTvPreview.Controls.Add(this.tmHAlign1);
            this.tmTvPreview.Location = new System.Drawing.Point(4, 24);
            this.tmTvPreview.Name = "tmTvPreview";
            this.tmTvPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tmTvPreview.Size = new System.Drawing.Size(504, 484);
            this.tmTvPreview.TabIndex = 0;
            this.tmTvPreview.Text = "Preview";
            this.tmTvPreview.UseVisualStyleBackColor = true;
            // 
            // tmHAlign1
            // 
            this.tmHAlign1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tmHAlign1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmHAlign1.Location = new System.Drawing.Point(3, 3);
            this.tmHAlign1.Name = "tmHAlign1";
            // 
            // tmHAlign1.Panel1
            // 
            this.tmHAlign1.Panel1.Controls.Add(this.tmOGL);
            this.tmHAlign1.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            // 
            // tmHAlign1.Panel2
            // 
            this.tmHAlign1.Panel2.Controls.Add(this.tmVAlign);
            this.tmHAlign1.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmHAlign1.Size = new System.Drawing.Size(498, 478);
            this.tmHAlign1.SplitterDistance = 357;
            this.tmHAlign1.TabIndex = 0;
            // 
            // tmOGL
            // 
            this.tmOGL.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.tmOGL.APIVersion = new System.Version(3, 3, 0, 0);
            this.tmOGL.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmOGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmOGL.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.tmOGL.IsEventDriven = true;
            this.tmOGL.Location = new System.Drawing.Point(0, 0);
            this.tmOGL.Name = "tmOGL";
            this.tmOGL.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            this.tmOGL.Size = new System.Drawing.Size(357, 478);
            this.tmOGL.TabIndex = 0;
            this.tmOGL.Text = "OpenGL";
            this.tmOGL.Load += new System.EventHandler(this.tmOGL_Load);
            this.tmOGL.Paint += new System.Windows.Forms.PaintEventHandler(this.tmOGL_Paint);
            this.tmOGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tmOGL_MouseMove);
            this.tmOGL.Resize += new System.EventHandler(this.tmOGL_Resize);
            // 
            // tmVAlign
            // 
            this.tmVAlign.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmVAlign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmVAlign.Location = new System.Drawing.Point(0, 0);
            this.tmVAlign.Name = "tmVAlign";
            this.tmVAlign.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // tmVAlign.Panel1
            // 
            this.tmVAlign.Panel1.Controls.Add(this.tmLbMeshes);
            this.tmVAlign.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            // 
            // tmVAlign.Panel2
            // 
            this.tmVAlign.Panel2.Controls.Add(this.tmDgModelProperties);
            this.tmVAlign.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmVAlign.Size = new System.Drawing.Size(137, 478);
            this.tmVAlign.SplitterDistance = 293;
            this.tmVAlign.TabIndex = 0;
            // 
            // tmLbMeshes
            // 
            this.tmLbMeshes.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmLbMeshes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmLbMeshes.FormattingEnabled = true;
            this.tmLbMeshes.Location = new System.Drawing.Point(0, 0);
            this.tmLbMeshes.Name = "tmLbMeshes";
            this.tmLbMeshes.Size = new System.Drawing.Size(137, 293);
            this.tmLbMeshes.TabIndex = 0;
            this.tmLbMeshes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.tmLbMeshes_ItemCheck);
            // 
            // tmDgModelProperties
            // 
            this.tmDgModelProperties.AllowUserToAddRows = false;
            this.tmDgModelProperties.AllowUserToDeleteRows = false;
            this.tmDgModelProperties.AllowUserToResizeColumns = false;
            this.tmDgModelProperties.AllowUserToResizeRows = false;
            this.tmDgModelProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tmDgModelProperties.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tmDgModelProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tmDgModelProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mtdgKey,
            this.mtdgValue});
            this.tmDgModelProperties.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tmDgModelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmDgModelProperties.Location = new System.Drawing.Point(0, 0);
            this.tmDgModelProperties.Name = "tmDgModelProperties";
            this.tmDgModelProperties.ReadOnly = true;
            this.tmDgModelProperties.RowHeadersVisible = false;
            this.tmDgModelProperties.RowTemplate.Height = 25;
            this.tmDgModelProperties.Size = new System.Drawing.Size(137, 181);
            this.tmDgModelProperties.TabIndex = 0;
            // 
            // mtdgKey
            // 
            this.mtdgKey.HeaderText = "Parameter";
            this.mtdgKey.Name = "mtdgKey";
            this.mtdgKey.ReadOnly = true;
            // 
            // mtdgValue
            // 
            this.mtdgValue.HeaderText = "Value";
            this.mtdgValue.Name = "mtdgValue";
            this.mtdgValue.ReadOnly = true;
            // 
            // tmTvExport
            // 
            this.tmTvExport.BackColor = System.Drawing.SystemColors.Control;
            this.tmTvExport.Controls.Add(this.groupBox1);
            this.tmTvExport.Location = new System.Drawing.Point(4, 24);
            this.tmTvExport.Name = "tmTvExport";
            this.tmTvExport.Padding = new System.Windows.Forms.Padding(3);
            this.tmTvExport.Size = new System.Drawing.Size(504, 484);
            this.tmTvExport.TabIndex = 1;
            this.tmTvExport.Text = "Export";
            this.tmTvExport.Enter += new System.EventHandler(this.tmTvExport_Enter);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tmBtExport);
            this.groupBox1.Controls.Add(this.itBtDoExport);
            this.groupBox1.Controls.Add(this.tmBtSelExpPath);
            this.groupBox1.Controls.Add(this.tmTxExpFormat);
            this.groupBox1.Controls.Add(this.tmTbExpPath);
            this.groupBox1.Controls.Add(this.tmLbExpFormat);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 107);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export...";
            // 
            // tmBtExport
            // 
            this.tmBtExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tmBtExport.Location = new System.Drawing.Point(411, 78);
            this.tmBtExport.Name = "tmBtExport";
            this.tmBtExport.Size = new System.Drawing.Size(75, 23);
            this.tmBtExport.TabIndex = 5;
            this.tmBtExport.Text = "Do it";
            this.tmBtExport.UseVisualStyleBackColor = true;
            this.tmBtExport.Click += new System.EventHandler(this.tmBtExport_Click);
            // 
            // itBtDoExport
            // 
            this.itBtDoExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.itBtDoExport.Location = new System.Drawing.Point(703, 85);
            this.itBtDoExport.Name = "itBtDoExport";
            this.itBtDoExport.Size = new System.Drawing.Size(75, 23);
            this.itBtDoExport.TabIndex = 3;
            this.itBtDoExport.Text = "Do it";
            this.itBtDoExport.UseVisualStyleBackColor = true;
            // 
            // tmBtSelExpPath
            // 
            this.tmBtSelExpPath.Location = new System.Drawing.Point(7, 22);
            this.tmBtSelExpPath.Name = "tmBtSelExpPath";
            this.tmBtSelExpPath.Size = new System.Drawing.Size(161, 23);
            this.tmBtSelExpPath.TabIndex = 2;
            this.tmBtSelExpPath.Text = "Browse Export Path";
            this.tmBtSelExpPath.UseVisualStyleBackColor = true;
            this.tmBtSelExpPath.Click += new System.EventHandler(this.tmBtSelExpPath_Click);
            // 
            // tmTxExpFormat
            // 
            this.tmTxExpFormat.AutoSize = true;
            this.tmTxExpFormat.Location = new System.Drawing.Point(83, 54);
            this.tmTxExpFormat.Name = "tmTxExpFormat";
            this.tmTxExpFormat.Size = new System.Drawing.Size(85, 15);
            this.tmTxExpFormat.TabIndex = 1;
            this.tmTxExpFormat.Text = "Export Format:";
            // 
            // tmTbExpPath
            // 
            this.tmTbExpPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tmTbExpPath.Location = new System.Drawing.Point(174, 22);
            this.tmTbExpPath.Name = "tmTbExpPath";
            this.tmTbExpPath.Size = new System.Drawing.Size(312, 23);
            this.tmTbExpPath.TabIndex = 0;
            // 
            // tmLbExpFormat
            // 
            this.tmLbExpFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tmLbExpFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tmLbExpFormat.Location = new System.Drawing.Point(174, 51);
            this.tmLbExpFormat.Name = "tmLbExpFormat";
            this.tmLbExpFormat.Size = new System.Drawing.Size(312, 23);
            this.tmLbExpFormat.TabIndex = 0;
            // 
            // ToolFFModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tmTabView);
            this.Name = "ToolFFModel";
            this.Size = new System.Drawing.Size(512, 512);
            this.tmTabView.ResumeLayout(false);
            this.tmTvPreview.ResumeLayout(false);
            this.tmHAlign1.Panel1.ResumeLayout(false);
            this.tmHAlign1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tmHAlign1)).EndInit();
            this.tmHAlign1.ResumeLayout(false);
            this.tmVAlign.Panel1.ResumeLayout(false);
            this.tmVAlign.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tmVAlign)).EndInit();
            this.tmVAlign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tmDgModelProperties)).EndInit();
            this.tmTvExport.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tmTabView;
        private System.Windows.Forms.TabPage tmTvPreview;
        private System.Windows.Forms.TabPage tmTvExport;
        private System.Windows.Forms.SplitContainer tmHAlign1;
        private OpenTK.WinForms.GLControl tmOGL;
        private System.Windows.Forms.SplitContainer tmVAlign;
        private System.Windows.Forms.CheckedListBox tmLbMeshes;
        private System.Windows.Forms.DataGridView tmDgModelProperties;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button itBtDoExport;
        private System.Windows.Forms.Button tmBtSelExpPath;
        private System.Windows.Forms.Label tmTxExpFormat;
        private System.Windows.Forms.TextBox tmTbExpPath;
        private System.Windows.Forms.ComboBox tmLbExpFormat;
        private System.Windows.Forms.Button tmBtExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn mtdgKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn mtdgValue;
    }
}
