using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JR.Utils.GUI.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize height and width sliders
            trackBarMaxWidth.Value = Convert.ToInt32(FlexibleMessageBox.MAX_WIDTH_FACTOR * 10);
            this.trackBarMaxWidth_Scroll(this, new EventArgs());
            trackBarMaxHeight.Value = Convert.ToInt32(FlexibleMessageBox.MAX_HEIGHT_FACTOR * 10);
            this.trackBarMaxHeight_Scroll(this, new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlexibleMessageBox.Show("Some text", "Some caption");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = FlexibleMessageBox.Show("Some text with a link: www.google.com\nA second line that contains a very very very very very very very very very very very very very very long text.",
                                                 "I am a FlexibleMessageBox",
                                                 MessageBoxButtons.AbortRetryIgnore,
                                                 MessageBoxIcon.Information,
                                                 MessageBoxDefaultButton.Button2);
            FlexibleMessageBox.Show("You have clicked: " + result.ToString(), "DialogResult");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var batchOperationResults = GetBatchOperationResults();
            var result = MessageBox.Show(batchOperationResults, "Batch Operation");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var batchOperationResults = GetBatchOperationResults();
            var result = FlexibleMessageBox.Show(batchOperationResults, "Batch Operation");
        }

        private static String GetBatchOperationResults()
        {
            var builder = new StringBuilder("Batch operation report:\n\n");
            var random = new Random();
            var result = 0;

            for (int i = 0; i < 200; i++)
            {
                result = random.Next(1000);

                if (result < 950)
                {
                    builder.AppendFormat(" - Task {0}: Operation completed sucessfully.\n", i);
                }
                else
                {
                    builder.AppendFormat(" - Task {0}: Operation failed! A very very very very very very very very very very very very serious error has occured during this sub-operation. The errorcode is: {1}).\n", i, result);
                }
            }

            return builder.ToString();
        }

        private void trackBarMaxWidth_Scroll(object sender, EventArgs e)
        {
            FlexibleMessageBox.MAX_WIDTH_FACTOR = Math.Round(trackBarMaxWidth.Value * 0.1, 1);
            labelMaxWidthInPercent.Text = string.Format("{0}%", FlexibleMessageBox.MAX_WIDTH_FACTOR * 100);
        }

        private void trackBarMaxHeight_Scroll(object sender, EventArgs e)
        {
            FlexibleMessageBox.MAX_HEIGHT_FACTOR = Math.Round(trackBarMaxHeight.Value * 0.1, 1);
            labelMaxHeightInPercent.Text = string.Format("{0}%", FlexibleMessageBox.MAX_HEIGHT_FACTOR * 100);
        }

        private void checkBoxUseOtherFont_CheckedChanged(object sender, EventArgs e)
        {
            FlexibleMessageBox.FONT = checkBoxUseOtherFont.Checked ? new Font("Impact", 12, FontStyle.Italic) : SystemFonts.MessageBoxFont;
        }
    }
}
