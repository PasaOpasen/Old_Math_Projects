namespace ContourAnalysisDemo
{
    partial class TemplateEditor
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
            this.dgvTemplates = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPreferredAngle = new System.Windows.Forms.CheckBox();
            this.btDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemplates)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTemplates
            // 
            this.dgvTemplates.AllowUserToAddRows = false;
            this.dgvTemplates.AllowUserToDeleteRows = false;
            this.dgvTemplates.AllowUserToResizeRows = false;
            this.dgvTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTemplates.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTemplates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTemplates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvTemplates.Location = new System.Drawing.Point(12, 23);
            this.dgvTemplates.Name = "dgvTemplates";
            this.dgvTemplates.RowHeadersVisible = false;
            this.dgvTemplates.Size = new System.Drawing.Size(260, 245);
            this.dgvTemplates.TabIndex = 0;
            this.dgvTemplates.VirtualMode = true;
            this.dgvTemplates.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvTemplates_CellValueNeeded);
            this.dgvTemplates.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvTemplates_CellValuePushed);
            this.dgvTemplates.SelectionChanged += new System.EventHandler(this.dgvTemplates_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.FillWeight = 1F;
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Templates";
            // 
            // cbPreferredAngle
            // 
            this.cbPreferredAngle.AutoSize = true;
            this.cbPreferredAngle.Location = new System.Drawing.Point(289, 251);
            this.cbPreferredAngle.Name = "cbPreferredAngle";
            this.cbPreferredAngle.Size = new System.Drawing.Size(154, 17);
            this.cbPreferredAngle.TabIndex = 3;
            this.cbPreferredAngle.Text = "Preferred angle no more 90";
            this.cbPreferredAngle.UseVisualStyleBackColor = true;
            this.cbPreferredAngle.CheckedChanged += new System.EventHandler(this.cbPreferredAngle_CheckedChanged);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(479, 245);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(99, 23);
            this.btDelete.TabIndex = 4;
            this.btDelete.Text = "Delete template";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // TemplateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 280);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPreferredAngle);
            this.Controls.Add(this.dgvTemplates);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(606, 318);
            this.Name = "TemplateEditor";
            this.Text = "Template Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemplates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTemplates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.CheckBox cbPreferredAngle;
        private System.Windows.Forms.Button btDelete;
    }
}