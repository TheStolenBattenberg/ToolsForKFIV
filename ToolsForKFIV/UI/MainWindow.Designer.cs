
namespace ToolsForKFIV
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mwMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openKFIVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mwOpenKFIVDialog = new System.Windows.Forms.OpenFileDialog();
            this.mwSplit = new System.Windows.Forms.SplitContainer();
            this.mwFileTree = new ToolsForKFIV.UI.Control.FileTree();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.mwMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mwSplit)).BeginInit();
            this.mwSplit.Panel1.SuspendLayout();
            this.mwSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // mwMenuStrip
            // 
            this.mwMenuStrip.BackColor = System.Drawing.SystemColors.Window;
            this.mwMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mwMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mwMenuStrip.Name = "mwMenuStrip";
            this.mwMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mwMenuStrip.TabIndex = 0;
            this.mwMenuStrip.Text = "mwMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openKFIVToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openKFIVToolStripMenuItem
            // 
            this.openKFIVToolStripMenuItem.Name = "openKFIVToolStripMenuItem";
            this.openKFIVToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openKFIVToolStripMenuItem.Text = "Open KFIV";
            this.openKFIVToolStripMenuItem.Click += new System.EventHandler(this.openKFIVToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mwOpenKFIVDialog
            // 
            this.mwOpenKFIVDialog.FileName = "King\'s Field IV EXE";
            this.mwOpenKFIVDialog.Filter = "King\'s Field IV |SLUS_203.18;SLUS_203.53;SLPS_250.57;SLES_309.20";
            // 
            // mwSplit
            // 
            this.mwSplit.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mwSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mwSplit.Location = new System.Drawing.Point(0, 24);
            this.mwSplit.Name = "mwSplit";
            // 
            // mwSplit.Panel1
            // 
            this.mwSplit.Panel1.Controls.Add(this.mwFileTree);
            this.mwSplit.Panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mwSplit.Panel1MinSize = 200;
            this.mwSplit.Size = new System.Drawing.Size(800, 426);
            this.mwSplit.SplitterDistance = 200;
            this.mwSplit.TabIndex = 1;
            // 
            // mwFileTree
            // 
            this.mwFileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mwFileTree.Location = new System.Drawing.Point(0, 0);
            this.mwFileTree.Margin = new System.Windows.Forms.Padding(0);
            this.mwFileTree.Name = "mwFileTree";
            this.mwFileTree.Size = new System.Drawing.Size(200, 426);
            this.mwFileTree.TabIndex = 0;
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mwSplit);
            this.Controls.Add(this.mwMenuStrip);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mwMenuStrip;
            this.Name = "MainWindow";
            this.Text = "Tools For KFIV";
            this.mwMenuStrip.ResumeLayout(false);
            this.mwMenuStrip.PerformLayout();
            this.mwSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mwSplit)).EndInit();
            this.mwSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mwMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openKFIVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog mwOpenKFIVDialog;
        private System.Windows.Forms.SplitContainer mwSplit;
        private UI.Control.FileTree mwFileTree;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

