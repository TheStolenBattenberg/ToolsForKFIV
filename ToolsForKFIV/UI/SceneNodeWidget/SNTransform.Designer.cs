
namespace ToolsForKFIV.UI.SceneNodeWidget
{
    partial class SNTransform
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
            this.gbTransform = new System.Windows.Forms.GroupBox();
            this.ntPosZ = new System.Windows.Forms.NumericUpDown();
            this.ntPosY = new System.Windows.Forms.NumericUpDown();
            this.ntPosX = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ntScaleX = new System.Windows.Forms.NumericUpDown();
            this.ntRotX = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ntScaleY = new System.Windows.Forms.NumericUpDown();
            this.ntRotY = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ntScaleZ = new System.Windows.Forms.NumericUpDown();
            this.ntRotZ = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbTransform.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotY)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotZ)).BeginInit();
            this.SuspendLayout();
            // 
            // gbTransform
            // 
            this.gbTransform.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gbTransform.Controls.Add(this.label3);
            this.gbTransform.Controls.Add(this.label2);
            this.gbTransform.Controls.Add(this.label1);
            this.gbTransform.Controls.Add(this.ntScaleX);
            this.gbTransform.Controls.Add(this.ntScaleY);
            this.gbTransform.Controls.Add(this.ntRotX);
            this.gbTransform.Controls.Add(this.ntPosZ);
            this.gbTransform.Controls.Add(this.ntRotY);
            this.gbTransform.Controls.Add(this.ntPosY);
            this.gbTransform.Controls.Add(this.ntPosX);
            this.gbTransform.Controls.Add(this.panel1);
            this.gbTransform.Controls.Add(this.panel2);
            this.gbTransform.Controls.Add(this.panel3);
            this.gbTransform.Location = new System.Drawing.Point(3, -2);
            this.gbTransform.MaximumSize = new System.Drawing.Size(300, 128);
            this.gbTransform.Name = "gbTransform";
            this.gbTransform.Size = new System.Drawing.Size(300, 104);
            this.gbTransform.TabIndex = 1;
            this.gbTransform.TabStop = false;
            this.gbTransform.Text = "Transform";
            // 
            // ntPosZ
            // 
            this.ntPosZ.DecimalPlaces = 3;
            this.ntPosZ.Location = new System.Drawing.Point(235, 22);
            this.ntPosZ.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntPosZ.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntPosZ.Name = "ntPosZ";
            this.ntPosZ.ReadOnly = true;
            this.ntPosZ.Size = new System.Drawing.Size(59, 23);
            this.ntPosZ.TabIndex = 4;
            // 
            // ntPosY
            // 
            this.ntPosY.DecimalPlaces = 3;
            this.ntPosY.Location = new System.Drawing.Point(170, 22);
            this.ntPosY.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntPosY.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntPosY.Name = "ntPosY";
            this.ntPosY.ReadOnly = true;
            this.ntPosY.Size = new System.Drawing.Size(59, 23);
            this.ntPosY.TabIndex = 3;
            // 
            // ntPosX
            // 
            this.ntPosX.BackColor = System.Drawing.SystemColors.Control;
            this.ntPosX.DecimalPlaces = 3;
            this.ntPosX.Location = new System.Drawing.Point(105, 22);
            this.ntPosX.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntPosX.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntPosX.Name = "ntPosX";
            this.ntPosX.ReadOnly = true;
            this.ntPosX.Size = new System.Drawing.Size(59, 23);
            this.ntPosX.TabIndex = 2;
            this.ntPosX.Value = new decimal(new int[] {
            314,
            0,
            0,
            131072});
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.panel1.Location = new System.Drawing.Point(105, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(59, 77);
            this.panel1.TabIndex = 5;
            // 
            // ntScaleX
            // 
            this.ntScaleX.BackColor = System.Drawing.SystemColors.Control;
            this.ntScaleX.DecimalPlaces = 3;
            this.ntScaleX.Location = new System.Drawing.Point(105, 76);
            this.ntScaleX.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntScaleX.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntScaleX.Name = "ntScaleX";
            this.ntScaleX.ReadOnly = true;
            this.ntScaleX.Size = new System.Drawing.Size(59, 23);
            this.ntScaleX.TabIndex = 9;
            this.ntScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntRotX
            // 
            this.ntRotX.BackColor = System.Drawing.Color.White;
            this.ntRotX.DecimalPlaces = 3;
            this.ntRotX.Location = new System.Drawing.Point(105, 49);
            this.ntRotX.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntRotX.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntRotX.Name = "ntRotX";
            this.ntRotX.ReadOnly = true;
            this.ntRotX.Size = new System.Drawing.Size(59, 23);
            this.ntRotX.TabIndex = 8;
            this.ntRotX.Value = new decimal(new int[] {
            565,
            0,
            0,
            131072});
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(169)))), ((int)(((byte)(244)))));
            this.panel2.Location = new System.Drawing.Point(170, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(59, 77);
            this.panel2.TabIndex = 6;
            // 
            // ntScaleY
            // 
            this.ntScaleY.BackColor = System.Drawing.SystemColors.Control;
            this.ntScaleY.DecimalPlaces = 3;
            this.ntScaleY.Location = new System.Drawing.Point(170, 75);
            this.ntScaleY.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntScaleY.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntScaleY.Name = "ntScaleY";
            this.ntScaleY.ReadOnly = true;
            this.ntScaleY.Size = new System.Drawing.Size(59, 23);
            this.ntScaleY.TabIndex = 10;
            this.ntScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntRotY
            // 
            this.ntRotY.BackColor = System.Drawing.Color.White;
            this.ntRotY.DecimalPlaces = 3;
            this.ntRotY.Location = new System.Drawing.Point(170, 49);
            this.ntRotY.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntRotY.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntRotY.Name = "ntRotY";
            this.ntRotY.ReadOnly = true;
            this.ntRotY.Size = new System.Drawing.Size(59, 23);
            this.ntRotY.TabIndex = 9;
            this.ntRotY.Value = new decimal(new int[] {
            565,
            0,
            0,
            131072});
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.panel3.Controls.Add(this.ntScaleZ);
            this.panel3.Controls.Add(this.ntRotZ);
            this.panel3.Location = new System.Drawing.Point(235, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(59, 77);
            this.panel3.TabIndex = 7;
            // 
            // ntScaleZ
            // 
            this.ntScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ntScaleZ.BackColor = System.Drawing.SystemColors.Control;
            this.ntScaleZ.DecimalPlaces = 3;
            this.ntScaleZ.Location = new System.Drawing.Point(0, 54);
            this.ntScaleZ.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntScaleZ.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntScaleZ.Name = "ntScaleZ";
            this.ntScaleZ.ReadOnly = true;
            this.ntScaleZ.Size = new System.Drawing.Size(59, 23);
            this.ntScaleZ.TabIndex = 11;
            this.ntScaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntRotZ
            // 
            this.ntRotZ.BackColor = System.Drawing.Color.White;
            this.ntRotZ.DecimalPlaces = 3;
            this.ntRotZ.Location = new System.Drawing.Point(0, 27);
            this.ntRotZ.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.ntRotZ.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.ntRotZ.Name = "ntRotZ";
            this.ntRotZ.ReadOnly = true;
            this.ntRotZ.Size = new System.Drawing.Size(59, 23);
            this.ntRotZ.TabIndex = 10;
            this.ntRotZ.Value = new decimal(new int[] {
            565,
            0,
            0,
            131072});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Translation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Rotation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "Scale";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SNTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.gbTransform);
            this.MinimumSize = new System.Drawing.Size(128, 64);
            this.Name = "SNTransform";
            this.Size = new System.Drawing.Size(306, 105);
            this.gbTransform.ResumeLayout(false);
            this.gbTransform.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotY)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ntScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntRotZ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTransform;
        private System.Windows.Forms.NumericUpDown ntPosZ;
        private System.Windows.Forms.NumericUpDown ntPosY;
        private System.Windows.Forms.NumericUpDown ntPosX;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ntScaleX;
        private System.Windows.Forms.NumericUpDown ntRotX;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown ntScaleY;
        private System.Windows.Forms.NumericUpDown ntRotY;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown ntScaleZ;
        private System.Windows.Forms.NumericUpDown ntRotZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
