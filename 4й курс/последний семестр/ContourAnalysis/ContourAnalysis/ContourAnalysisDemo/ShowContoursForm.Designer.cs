namespace ContourAnalysisDemo
{
    partial class ShowContoursForm
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
            this.dgvContours = new System.Windows.Forms.DataGridView();
            this.Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbTemplateName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContours)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvContours
            // 
            this.dgvContours.AllowUserToAddRows = false;
            this.dgvContours.AllowUserToDeleteRows = false;
            this.dgvContours.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContours.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvContours.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvContours.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContours.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column});
            this.dgvContours.Location = new System.Drawing.Point(12, 12);
            this.dgvContours.Name = "dgvContours";
            this.dgvContours.ReadOnly = true;
            this.dgvContours.RowTemplate.Height = 200;
            this.dgvContours.Size = new System.Drawing.Size(507, 409);
            this.dgvContours.TabIndex = 0;
            this.dgvContours.VirtualMode = true;
            this.dgvContours.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvContours_CellPainting);
            // 
            // Column
            // 
            this.Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column.HeaderText = "Contour";
            this.Column.Name = "Column";
            this.Column.ReadOnly = true;
            // 
            // tbTemplateName
            // 
            this.tbTemplateName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbTemplateName.ForeColor = System.Drawing.Color.Gray;
            this.tbTemplateName.Location = new System.Drawing.Point(205, 431);
            this.tbTemplateName.Name = "tbTemplateName";
            this.tbTemplateName.Size = new System.Drawing.Size(114, 20);
            this.tbTemplateName.TabIndex = 1;
            this.tbTemplateName.Text = "<template name>";
            this.tbTemplateName.Enter += new System.EventHandler(this.tbTemplateName_Enter);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 434);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(18, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add selected contour as Template:";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(326, 431);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "enter template name or image file name (*.png, *.jpg)";
            // 
            // ShowContoursForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 459);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTemplateName);
            this.Controls.Add(this.dgvContours);
            this.Name = "ShowContoursForm";
            this.Text = "Create templates";
            ((System.ComponentModel.ISupportInitialize)(this.dgvContours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvContours;
        private System.Windows.Forms.TextBox tbTemplateName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column;
        private System.Windows.Forms.Label label2;
    }
}