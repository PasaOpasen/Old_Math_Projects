namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBoxUseOtherFont = new System.Windows.Forms.CheckBox();
            this.trackBarMaxWidth = new System.Windows.Forms.TrackBar();
            this.trackBarMaxHeight = new System.Windows.Forms.TrackBar();
            this.labelMaxWidthInPercent = new System.Windows.Forms.Label();
            this.labelMaxHeightInPercent = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(13, 28);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 100);
            this.button1.TabIndex = 0;
            this.button1.Text = "FlexibleMessageBox: \r\nA simple call...";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(13, 143);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(224, 100);
            this.button2.TabIndex = 1;
            this.button2.Text = "FlexibleMessageBox: \r\nUsing more parameters and a dialog result...";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Tomato;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(310, 28);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(224, 100);
            this.button3.TabIndex = 2;
            this.button3.Text = "Wrong .NET MessageBox: \r\nToo many rows to show...";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(310, 143);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(224, 100);
            this.button4.TabIndex = 3;
            this.button4.Text = "FlexibleMessageBox many rows: \r\nOne box to rule them all... :-)";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBoxUseOtherFont
            // 
            this.checkBoxUseOtherFont.AutoSize = true;
            this.checkBoxUseOtherFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseOtherFont.Location = new System.Drawing.Point(21, 298);
            this.checkBoxUseOtherFont.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxUseOtherFont.Name = "checkBoxUseOtherFont";
            this.checkBoxUseOtherFont.Size = new System.Drawing.Size(581, 34);
            this.checkBoxUseOtherFont.TabIndex = 9;
            this.checkBoxUseOtherFont.Text = "Example: Use italic style 12-point \"Impact\" Font";
            this.checkBoxUseOtherFont.UseVisualStyleBackColor = true;
            this.checkBoxUseOtherFont.CheckedChanged += new System.EventHandler(this.checkBoxUseOtherFont_CheckedChanged);
            // 
            // trackBarMaxWidth
            // 
            this.trackBarMaxWidth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarMaxWidth.LargeChange = 2;
            this.trackBarMaxWidth.Location = new System.Drawing.Point(74, 245);
            this.trackBarMaxWidth.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarMaxWidth.Minimum = 2;
            this.trackBarMaxWidth.Name = "trackBarMaxWidth";
            this.trackBarMaxWidth.Size = new System.Drawing.Size(221, 90);
            this.trackBarMaxWidth.TabIndex = 7;
            this.trackBarMaxWidth.Value = 2;
            this.trackBarMaxWidth.Scroll += new System.EventHandler(this.trackBarMaxWidth_Scroll);
            // 
            // trackBarMaxHeight
            // 
            this.trackBarMaxHeight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarMaxHeight.LargeChange = 2;
            this.trackBarMaxHeight.Location = new System.Drawing.Point(21, 48);
            this.trackBarMaxHeight.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarMaxHeight.Minimum = 2;
            this.trackBarMaxHeight.Name = "trackBarMaxHeight";
            this.trackBarMaxHeight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMaxHeight.Size = new System.Drawing.Size(90, 200);
            this.trackBarMaxHeight.TabIndex = 6;
            this.trackBarMaxHeight.Value = 2;
            this.trackBarMaxHeight.Scroll += new System.EventHandler(this.trackBarMaxHeight_Scroll);
            // 
            // labelMaxWidthInPercent
            // 
            this.labelMaxWidthInPercent.AutoSize = true;
            this.labelMaxWidthInPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxWidthInPercent.Location = new System.Drawing.Point(287, 245);
            this.labelMaxWidthInPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxWidthInPercent.Name = "labelMaxWidthInPercent";
            this.labelMaxWidthInPercent.Size = new System.Drawing.Size(157, 30);
            this.labelMaxWidthInPercent.TabIndex = 8;
            this.labelMaxWidthInPercent.Text = "<MaxWidth>";
            // 
            // labelMaxHeightInPercent
            // 
            this.labelMaxHeightInPercent.AutoSize = true;
            this.labelMaxHeightInPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxHeightInPercent.Location = new System.Drawing.Point(19, 30);
            this.labelMaxHeightInPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxHeightInPercent.Name = "labelMaxHeightInPercent";
            this.labelMaxHeightInPercent.Size = new System.Drawing.Size(165, 30);
            this.labelMaxHeightInPercent.TabIndex = 5;
            this.labelMaxHeightInPercent.Text = "<MaxHeight>";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.checkBoxUseOtherFont);
            this.groupBox1.Controls.Add(this.labelMaxWidthInPercent);
            this.groupBox1.Controls.Add(this.labelMaxHeightInPercent);
            this.groupBox1.Controls.Add(this.trackBarMaxHeight);
            this.groupBox1.Controls.Add(this.trackBarMaxWidth);
            this.groupBox1.Location = new System.Drawing.Point(594, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 334);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change common static parameters";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(84, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 179);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 120);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please choose the maximum \r\nwidth and height for all \r\nFlexibleMessageBox instanc" +
    "es \r\nin percent of the working area.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(998, 339);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 410);
            this.MinimumSize = new System.Drawing.Size(1024, 410);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "A short demo for the FlexibleMessageBox";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBoxUseOtherFont;
        private System.Windows.Forms.TrackBar trackBarMaxWidth;
        private System.Windows.Forms.TrackBar trackBarMaxHeight;
        private System.Windows.Forms.Label labelMaxWidthInPercent;
        private System.Windows.Forms.Label labelMaxHeightInPercent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}

