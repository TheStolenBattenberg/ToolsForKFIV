
namespace ToolsForKFIV.UI.Control
{
    partial class FileTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileTree));
            this.ftTreeView = new System.Windows.Forms.TreeView();
            this.ftTreeViewIconSet = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ftTreeView
            // 
            this.ftTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ftTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ftTreeView.ImageIndex = 0;
            this.ftTreeView.ImageList = this.ftTreeViewIconSet;
            this.ftTreeView.Indent = 24;
            this.ftTreeView.ItemHeight = 20;
            this.ftTreeView.Location = new System.Drawing.Point(0, 0);
            this.ftTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.ftTreeView.Name = "ftTreeView";
            this.ftTreeView.PathSeparator = "/";
            this.ftTreeView.SelectedImageIndex = 0;
            this.ftTreeView.Size = new System.Drawing.Size(150, 150);
            this.ftTreeView.TabIndex = 0;
            this.ftTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ftTreeView_NodeMouseDoubleClick);
            this.ftTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ftTreeView_KeyDown);
            this.ftTreeView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ftTreeView_KeyPress);
            this.ftTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ftTreeView_KeyUp);
            // 
            // ftTreeViewIconSet
            // 
            this.ftTreeViewIconSet.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ftTreeViewIconSet.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ftTreeViewIconSet.ImageStream")));
            this.ftTreeViewIconSet.TransparentColor = System.Drawing.Color.Transparent;
            this.ftTreeViewIconSet.Images.SetKeyName(0, "ficn_folder.png");
            this.ftTreeViewIconSet.Images.SetKeyName(1, "ficn_unknown.png");
            this.ftTreeViewIconSet.Images.SetKeyName(2, "ficn_db.png");
            this.ftTreeViewIconSet.Images.SetKeyName(3, "ficn_ini.png");
            this.ftTreeViewIconSet.Images.SetKeyName(4, "ficn_map.png");
            this.ftTreeViewIconSet.Images.SetKeyName(5, "ficn_font.png");
            this.ftTreeViewIconSet.Images.SetKeyName(6, "ficn_midi.png");
            this.ftTreeViewIconSet.Images.SetKeyName(7, "ficn_wave.png");
            this.ftTreeViewIconSet.Images.SetKeyName(8, "ficn_image.png");
            this.ftTreeViewIconSet.Images.SetKeyName(9, "ficn_model.png");
            this.ftTreeViewIconSet.Images.SetKeyName(10, "ficn_animation.png");
            this.ftTreeViewIconSet.Images.SetKeyName(11, "ficn_archive.png");
            // 
            // FileTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ftTreeView);
            this.Name = "FileTree";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView ftTreeView;
        private System.Windows.Forms.ImageList ftTreeViewIconSet;
    }
}
