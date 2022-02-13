
namespace ToolsForKFIV.UI.Control
{
    partial class ToolFFini
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
            this.tfiTabView = new System.Windows.Forms.TabControl();
            this.tvPreview = new System.Windows.Forms.TabPage();
            this.tiTextBox = new System.Windows.Forms.RichTextBox();
            this.tvExport = new System.Windows.Forms.TabPage();
            this.tfiTabView.SuspendLayout();
            this.tvPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tfiTabView
            // 
            this.tfiTabView.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tfiTabView.Controls.Add(this.tvPreview);
            this.tfiTabView.Controls.Add(this.tvExport);
            this.tfiTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tfiTabView.Location = new System.Drawing.Point(0, 0);
            this.tfiTabView.Name = "tfiTabView";
            this.tfiTabView.SelectedIndex = 0;
            this.tfiTabView.Size = new System.Drawing.Size(256, 256);
            this.tfiTabView.TabIndex = 0;
            // 
            // tvPreview
            // 
            this.tvPreview.Controls.Add(this.tiTextBox);
            this.tvPreview.Location = new System.Drawing.Point(4, 27);
            this.tvPreview.Name = "tvPreview";
            this.tvPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tvPreview.Size = new System.Drawing.Size(248, 225);
            this.tvPreview.TabIndex = 0;
            this.tvPreview.Text = "Preview";
            this.tvPreview.UseVisualStyleBackColor = true;
            // 
            // tiTextBox
            // 
            this.tiTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tiTextBox.Location = new System.Drawing.Point(3, 3);
            this.tiTextBox.Name = "tiTextBox";
            this.tiTextBox.ReadOnly = true;
            this.tiTextBox.Size = new System.Drawing.Size(242, 219);
            this.tiTextBox.TabIndex = 0;
            this.tiTextBox.Text = "";
            this.tiTextBox.WordWrap = false;
            // 
            // tvExport
            // 
            this.tvExport.Location = new System.Drawing.Point(4, 27);
            this.tvExport.Name = "tvExport";
            this.tvExport.Padding = new System.Windows.Forms.Padding(3);
            this.tvExport.Size = new System.Drawing.Size(248, 225);
            this.tvExport.TabIndex = 1;
            this.tvExport.Text = "Export";
            this.tvExport.UseVisualStyleBackColor = true;
            // 
            // ToolFFini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tfiTabView);
            this.Name = "ToolFFini";
            this.Size = new System.Drawing.Size(256, 256);
            this.tfiTabView.ResumeLayout(false);
            this.tvPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tfiTabView;
        private System.Windows.Forms.TabPage tvPreview;
        private System.Windows.Forms.TabPage tvExport;
        private System.Windows.Forms.RichTextBox tiTextBox;
    }
}
