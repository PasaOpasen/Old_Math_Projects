using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ContourAnalysisNS;

namespace ContourAnalysisDemo
{
    public partial class AutoGenerateForm : Form
    {
        ImageProcessor processor;

        public AutoGenerateForm(ImageProcessor processor)
        {
            InitializeComponent();
            tbFont.Text = new FontConverter().ConvertToString(tbChars.Font);
            this.processor = processor;
        }

        private void btFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbChars.Font = fontDialog1.Font;
                tbFont.Text = new FontConverter().ConvertToString(tbChars.Font);
            }
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int oldCount = processor.templates.Count;
                TemplateGenerator.GenerateChars(processor, tbChars.Text.ToCharArray(), tbChars.Font);
                if(cbAntipattern.Checked)
                    TemplateGenerator.GenerateAntipatterns(processor);
                MessageBox.Show("Added "+(processor.templates.Count - oldCount)+" templates");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
