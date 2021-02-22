namespace ContourAnalysisDemo
{
    partial class AutoGenerateForm
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
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tbChars = new System.Windows.Forms.TextBox();
            this.btFont = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFont = new System.Windows.Forms.TextBox();
            this.cbAntipattern = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // fontDialog1
            // 
            this.fontDialog1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            // 
            // tbChars
            // 
            this.tbChars.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbChars.Location = new System.Drawing.Point(11, 71);
            this.tbChars.Multiline = true;
            this.tbChars.Name = "tbChars";
            this.tbChars.Size = new System.Drawing.Size(257, 81);
            this.tbChars.TabIndex = 2;
            this.tbChars.Text = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 
            // btFont
            // 
            this.btFont.Location = new System.Drawing.Point(221, 23);
            this.btFont.Name = "btFont";
            this.btFont.Size = new System.Drawing.Size(51, 23);
            this.btFont.TabIndex = 3;
            this.btFont.Text = "Font...";
            this.btFont.UseVisualStyleBackColor = true;
            this.btFont.Click += new System.EventHandler(this.btFont_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chars";
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(113, 181);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(155, 23);
            this.btGenerate.TabIndex = 5;
            this.btGenerate.Text = "Generate templates";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Font";
            // 
            // tbFont
            // 
            this.tbFont.Location = new System.Drawing.Point(11, 26);
            this.tbFont.Name = "tbFont";
            this.tbFont.ReadOnly = true;
            this.tbFont.Size = new System.Drawing.Size(204, 20);
            this.tbFont.TabIndex = 0;
            this.tbFont.TabStop = false;
            // 
            // cbAntipattern
            // 
            this.cbAntipattern.AutoSize = true;
            this.cbAntipattern.Location = new System.Drawing.Point(12, 158);
            this.cbAntipattern.Name = "cbAntipattern";
            this.cbAntipattern.Size = new System.Drawing.Size(137, 17);
            this.cbAntipattern.TabIndex = 6;
            this.cbAntipattern.Text = "Also create antipatterns";
            this.cbAntipattern.UseVisualStyleBackColor = true;
            // 
            // AutoGenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 217);
            this.Controls.Add(this.cbAntipattern);
            this.Controls.Add(this.btGenerate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btFont);
            this.Controls.Add(this.tbChars);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFont);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AutoGenerateForm";
            this.Text = "Generate Templates";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox tbChars;
        private System.Windows.Forms.Button btFont;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFont;
        private System.Windows.Forms.CheckBox cbAntipattern;
    }
}