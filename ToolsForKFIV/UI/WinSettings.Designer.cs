
namespace ToolsForKFIV.UI
{
    partial class WinSettings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinSettings));
            this.wsTabView = new System.Windows.Forms.TabControl();
            this.wsTabFileSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPrettifyAssetNames = new System.Windows.Forms.CheckBox();
            this.wsImageViewer = new System.Windows.Forms.TabPage();
            this.wsModelViewer = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.wsBGColPreview = new System.Windows.Forms.Panel();
            this.wsBtnSetBGCol = new System.Windows.Forms.Button();
            this.ttprettifyassetnames = new System.Windows.Forms.ToolTip(this.components);
            this.wsColourPicker = new System.Windows.Forms.ColorDialog();
            this.wsBtSetXA = new System.Windows.Forms.Button();
            this.wsXAColPreview = new System.Windows.Forms.Panel();
            this.wsLBXACol = new System.Windows.Forms.Label();
            this.wsLBBGCol = new System.Windows.Forms.Label();
            this.wsTabView.SuspendLayout();
            this.wsTabFileSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.wsModelViewer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.wsBGColPreview.SuspendLayout();
            this.wsXAColPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // wsTabView
            // 
            this.wsTabView.Controls.Add(this.wsTabFileSettings);
            this.wsTabView.Controls.Add(this.wsImageViewer);
            this.wsTabView.Controls.Add(this.wsModelViewer);
            this.wsTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wsTabView.Location = new System.Drawing.Point(0, 0);
            this.wsTabView.Name = "wsTabView";
            this.wsTabView.SelectedIndex = 0;
            this.wsTabView.Size = new System.Drawing.Size(800, 450);
            this.wsTabView.TabIndex = 0;
            // 
            // wsTabFileSettings
            // 
            this.wsTabFileSettings.Controls.Add(this.groupBox1);
            this.wsTabFileSettings.Location = new System.Drawing.Point(4, 24);
            this.wsTabFileSettings.Name = "wsTabFileSettings";
            this.wsTabFileSettings.Padding = new System.Windows.Forms.Padding(3);
            this.wsTabFileSettings.Size = new System.Drawing.Size(792, 422);
            this.wsTabFileSettings.TabIndex = 0;
            this.wsTabFileSettings.Text = "File Settings";
            this.wsTabFileSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPrettifyAssetNames);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(785, 415);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings that control various aspects of Tools For KFIV";
            // 
            // cbPrettifyAssetNames
            // 
            this.cbPrettifyAssetNames.AutoSize = true;
            this.cbPrettifyAssetNames.Location = new System.Drawing.Point(6, 22);
            this.cbPrettifyAssetNames.Name = "cbPrettifyAssetNames";
            this.cbPrettifyAssetNames.Size = new System.Drawing.Size(135, 19);
            this.cbPrettifyAssetNames.TabIndex = 0;
            this.cbPrettifyAssetNames.Text = "Prettify Asset Names";
            this.cbPrettifyAssetNames.UseVisualStyleBackColor = true;
            // 
            // wsImageViewer
            // 
            this.wsImageViewer.Location = new System.Drawing.Point(4, 24);
            this.wsImageViewer.Name = "wsImageViewer";
            this.wsImageViewer.Padding = new System.Windows.Forms.Padding(3);
            this.wsImageViewer.Size = new System.Drawing.Size(792, 422);
            this.wsImageViewer.TabIndex = 1;
            this.wsImageViewer.Text = "Image Tool Settings";
            this.wsImageViewer.UseVisualStyleBackColor = true;
            // 
            // wsModelViewer
            // 
            this.wsModelViewer.Controls.Add(this.groupBox2);
            this.wsModelViewer.Location = new System.Drawing.Point(4, 24);
            this.wsModelViewer.Name = "wsModelViewer";
            this.wsModelViewer.Size = new System.Drawing.Size(792, 422);
            this.wsModelViewer.TabIndex = 2;
            this.wsModelViewer.Text = "Model Viewer Settings";
            this.wsModelViewer.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.wsXAColPreview);
            this.groupBox2.Controls.Add(this.wsBtSetXA);
            this.groupBox2.Controls.Add(this.wsBGColPreview);
            this.groupBox2.Controls.Add(this.wsBtnSetBGCol);
            this.groupBox2.Location = new System.Drawing.Point(4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(785, 415);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings modifiying the model viewing tool";
            // 
            // wsBGColPreview
            // 
            this.wsBGColPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(11)))), ((int)(((byte)(23)))));
            this.wsBGColPreview.Controls.Add(this.wsLBBGCol);
            this.wsBGColPreview.Location = new System.Drawing.Point(162, 22);
            this.wsBGColPreview.Name = "wsBGColPreview";
            this.wsBGColPreview.Size = new System.Drawing.Size(618, 23);
            this.wsBGColPreview.TabIndex = 1;
            // 
            // wsBtnSetBGCol
            // 
            this.wsBtnSetBGCol.Location = new System.Drawing.Point(6, 22);
            this.wsBtnSetBGCol.Name = "wsBtnSetBGCol";
            this.wsBtnSetBGCol.Size = new System.Drawing.Size(150, 23);
            this.wsBtnSetBGCol.TabIndex = 0;
            this.wsBtnSetBGCol.Text = "Set Background Colour";
            this.wsBtnSetBGCol.UseVisualStyleBackColor = true;
            this.wsBtnSetBGCol.Click += new System.EventHandler(this.button1_Click);
            // 
            // wsBtSetXA
            // 
            this.wsBtSetXA.Location = new System.Drawing.Point(6, 51);
            this.wsBtSetXA.Name = "wsBtSetXA";
            this.wsBtSetXA.Size = new System.Drawing.Size(150, 23);
            this.wsBtSetXA.TabIndex = 2;
            this.wsBtSetXA.Text = "Set X Axis Colour";
            this.wsBtSetXA.UseVisualStyleBackColor = true;
            this.wsBtSetXA.Click += new System.EventHandler(this.wsBtSetXA_Click);
            // 
            // wsXAColPreview
            // 
            this.wsXAColPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.wsXAColPreview.Controls.Add(this.wsLBXACol);
            this.wsXAColPreview.Location = new System.Drawing.Point(162, 51);
            this.wsXAColPreview.Name = "wsXAColPreview";
            this.wsXAColPreview.Size = new System.Drawing.Size(618, 23);
            this.wsXAColPreview.TabIndex = 2;
            // 
            // wsLBXACol
            // 
            this.wsLBXACol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsLBXACol.AutoSize = true;
            this.wsLBXACol.Location = new System.Drawing.Point(267, 4);
            this.wsLBXACol.Name = "wsLBXACol";
            this.wsLBXACol.Size = new System.Drawing.Size(91, 15);
            this.wsLBXACol.TabIndex = 0;
            this.wsLBXACol.Text = "X AXIS COLOUR";
            this.wsLBXACol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wsLBBGCol
            // 
            this.wsLBBGCol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsLBBGCol.AutoSize = true;
            this.wsLBBGCol.Location = new System.Drawing.Point(241, 4);
            this.wsLBBGCol.Name = "wsLBBGCol";
            this.wsLBBGCol.Size = new System.Drawing.Size(135, 15);
            this.wsLBBGCol.TabIndex = 1;
            this.wsLBBGCol.Text = "BACKGROUND COLOUR";
            this.wsLBBGCol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WinSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.wsTabView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WinSettings";
            this.Text = "Tools For KFIV - Settings";
            this.wsTabView.ResumeLayout(false);
            this.wsTabFileSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.wsModelViewer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.wsBGColPreview.ResumeLayout(false);
            this.wsBGColPreview.PerformLayout();
            this.wsXAColPreview.ResumeLayout(false);
            this.wsXAColPreview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl wsTabView;
        private System.Windows.Forms.TabPage wsTabFileSettings;
        private System.Windows.Forms.TabPage wsImageViewer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbPrettifyAssetNames;
        private System.Windows.Forms.ToolTip ttprettifyassetnames;
        private System.Windows.Forms.TabPage wsModelViewer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button wsBtnSetBGCol;
        private System.Windows.Forms.ColorDialog wsColourPicker;
        private System.Windows.Forms.Panel wsBGColPreview;
        private System.Windows.Forms.Panel wsXAColPreview;
        private System.Windows.Forms.Button wsBtSetXA;
        private System.Windows.Forms.Label wsLBXACol;
        private System.Windows.Forms.Label wsLBBGCol;
    }
}