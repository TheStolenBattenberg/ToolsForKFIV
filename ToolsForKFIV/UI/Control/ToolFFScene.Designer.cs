
namespace ToolsForKFIV.UI.Control
{
    partial class ToolFFScene
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolFFScene));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.stPreviewTab = new System.Windows.Forms.TabPage();
            this.stPreviewSplit1 = new System.Windows.Forms.SplitContainer();
            this.stToolStrip = new System.Windows.Forms.ToolStrip();
            this.stEnableStuffBtLst = new System.Windows.Forms.ToolStripSplitButton();
            this.stTSEnableObj = new System.Windows.Forms.ToolStripMenuItem();
            this.stTSEnableLight = new System.Windows.Forms.ToolStripMenuItem();
            this.stTSEnableCollision = new System.Windows.Forms.ToolStripMenuItem();
            this.stPreviewGL = new OpenTK.WinForms.GLControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.stSceneNodeTree = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.stPropertiesTab = new System.Windows.Forms.TabPage();
            this.stExportTab = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.stPreviewTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stPreviewSplit1)).BeginInit();
            this.stPreviewSplit1.Panel1.SuspendLayout();
            this.stPreviewSplit1.Panel2.SuspendLayout();
            this.stPreviewSplit1.SuspendLayout();
            this.stToolStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.stPreviewTab);
            this.tabControl1.Controls.Add(this.stPropertiesTab);
            this.tabControl1.Controls.Add(this.stExportTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(512, 512);
            this.tabControl1.TabIndex = 0;
            // 
            // stPreviewTab
            // 
            this.stPreviewTab.Controls.Add(this.stPreviewSplit1);
            this.stPreviewTab.Location = new System.Drawing.Point(4, 27);
            this.stPreviewTab.Name = "stPreviewTab";
            this.stPreviewTab.Padding = new System.Windows.Forms.Padding(3);
            this.stPreviewTab.Size = new System.Drawing.Size(504, 481);
            this.stPreviewTab.TabIndex = 0;
            this.stPreviewTab.Text = "Preview";
            this.stPreviewTab.UseVisualStyleBackColor = true;
            // 
            // stPreviewSplit1
            // 
            this.stPreviewSplit1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.stPreviewSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stPreviewSplit1.Location = new System.Drawing.Point(3, 3);
            this.stPreviewSplit1.Name = "stPreviewSplit1";
            // 
            // stPreviewSplit1.Panel1
            // 
            this.stPreviewSplit1.Panel1.Controls.Add(this.stToolStrip);
            this.stPreviewSplit1.Panel1.Controls.Add(this.stPreviewGL);
            this.stPreviewSplit1.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            // 
            // stPreviewSplit1.Panel2
            // 
            this.stPreviewSplit1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.stPreviewSplit1.Size = new System.Drawing.Size(498, 475);
            this.stPreviewSplit1.SplitterDistance = 364;
            this.stPreviewSplit1.TabIndex = 0;
            // 
            // stToolStrip
            // 
            this.stToolStrip.BackColor = System.Drawing.Color.LightGray;
            this.stToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.stToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stEnableStuffBtLst});
            this.stToolStrip.Location = new System.Drawing.Point(0, 0);
            this.stToolStrip.Name = "stToolStrip";
            this.stToolStrip.Size = new System.Drawing.Size(364, 25);
            this.stToolStrip.TabIndex = 1;
            this.stToolStrip.Text = "toolStrip1";
            // 
            // stEnableStuffBtLst
            // 
            this.stEnableStuffBtLst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stEnableStuffBtLst.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stTSEnableObj,
            this.stTSEnableLight,
            this.stTSEnableCollision});
            this.stEnableStuffBtLst.Image = ((System.Drawing.Image)(resources.GetObject("stEnableStuffBtLst.Image")));
            this.stEnableStuffBtLst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stEnableStuffBtLst.Name = "stEnableStuffBtLst";
            this.stEnableStuffBtLst.Size = new System.Drawing.Size(57, 22);
            this.stEnableStuffBtLst.Text = "View...";
            // 
            // stTSEnableObj
            // 
            this.stTSEnableObj.Checked = true;
            this.stTSEnableObj.CheckOnClick = true;
            this.stTSEnableObj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stTSEnableObj.Name = "stTSEnableObj";
            this.stTSEnableObj.Size = new System.Drawing.Size(158, 22);
            this.stTSEnableObj.Text = "Enable Objects";
            // 
            // stTSEnableLight
            // 
            this.stTSEnableLight.Checked = true;
            this.stTSEnableLight.CheckOnClick = true;
            this.stTSEnableLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stTSEnableLight.Name = "stTSEnableLight";
            this.stTSEnableLight.Size = new System.Drawing.Size(158, 22);
            this.stTSEnableLight.Text = "Enable Lights";
            // 
            // stTSEnableCollision
            // 
            this.stTSEnableCollision.Checked = true;
            this.stTSEnableCollision.CheckOnClick = true;
            this.stTSEnableCollision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stTSEnableCollision.Name = "stTSEnableCollision";
            this.stTSEnableCollision.Size = new System.Drawing.Size(158, 22);
            this.stTSEnableCollision.Text = "Enable Collision";
            // 
            // stPreviewGL
            // 
            this.stPreviewGL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stPreviewGL.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.stPreviewGL.APIVersion = new System.Version(3, 3, 0, 0);
            this.stPreviewGL.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.stPreviewGL.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.stPreviewGL.IsEventDriven = true;
            this.stPreviewGL.Location = new System.Drawing.Point(-3, 25);
            this.stPreviewGL.Margin = new System.Windows.Forms.Padding(0);
            this.stPreviewGL.Name = "stPreviewGL";
            this.stPreviewGL.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            this.stPreviewGL.Size = new System.Drawing.Size(364, 450);
            this.stPreviewGL.TabIndex = 0;
            this.stPreviewGL.Text = "glControl1";
            this.stPreviewGL.Load += new System.EventHandler(this.stPreviewGL_Load);
            this.stPreviewGL.Paint += new System.Windows.Forms.PaintEventHandler(this.stPreviewGL_Paint);
            this.stPreviewGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.stPreviewGL_MouseMove);
            this.stPreviewGL.Resize += new System.EventHandler(this.stPreviewGL_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.stSceneNodeTree, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.63158F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.36842F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(130, 475);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // stSceneNodeTree
            // 
            this.stSceneNodeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stSceneNodeTree.Location = new System.Drawing.Point(3, 3);
            this.stSceneNodeTree.Name = "stSceneNodeTree";
            this.stSceneNodeTree.Size = new System.Drawing.Size(124, 168);
            this.stSceneNodeTree.TabIndex = 0;
            this.stSceneNodeTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.stSceneNodeTree_NodeMouseClick);
            this.stSceneNodeTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.stSceneNodeTree_KeyDown);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 177);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(124, 295);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            this.propertyGrid1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.propertyGrid1_PreviewKeyDown);
            // 
            // stPropertiesTab
            // 
            this.stPropertiesTab.Location = new System.Drawing.Point(4, 27);
            this.stPropertiesTab.Name = "stPropertiesTab";
            this.stPropertiesTab.Padding = new System.Windows.Forms.Padding(3);
            this.stPropertiesTab.Size = new System.Drawing.Size(504, 481);
            this.stPropertiesTab.TabIndex = 1;
            this.stPropertiesTab.Text = "Properties";
            this.stPropertiesTab.UseVisualStyleBackColor = true;
            // 
            // stExportTab
            // 
            this.stExportTab.Location = new System.Drawing.Point(4, 27);
            this.stExportTab.Name = "stExportTab";
            this.stExportTab.Size = new System.Drawing.Size(504, 481);
            this.stExportTab.TabIndex = 2;
            this.stExportTab.Text = "Export";
            this.stExportTab.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ToolFFScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ToolFFScene";
            this.Size = new System.Drawing.Size(512, 512);
            this.tabControl1.ResumeLayout(false);
            this.stPreviewTab.ResumeLayout(false);
            this.stPreviewSplit1.Panel1.ResumeLayout(false);
            this.stPreviewSplit1.Panel1.PerformLayout();
            this.stPreviewSplit1.Panel2.ResumeLayout(false);
            this.stPreviewSplit1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stPreviewSplit1)).EndInit();
            this.stPreviewSplit1.ResumeLayout(false);
            this.stToolStrip.ResumeLayout(false);
            this.stToolStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage stPreviewTab;
        private System.Windows.Forms.SplitContainer stPreviewSplit1;
        private System.Windows.Forms.TabPage stPropertiesTab;
        private System.Windows.Forms.TabPage stExportTab;
        private OpenTK.WinForms.GLControl stPreviewGL;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStrip stToolStrip;
        private System.Windows.Forms.ToolStripSplitButton stEnableStuffBtLst;
        private System.Windows.Forms.ToolStripMenuItem stTSEnableObj;
        private System.Windows.Forms.ToolStripMenuItem stTSEnableLight;
        private System.Windows.Forms.ToolStripMenuItem stTSEnableCollision;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView stSceneNodeTree;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}
