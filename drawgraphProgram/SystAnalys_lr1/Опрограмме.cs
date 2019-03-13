using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SystAnalys_lr1
{
    public partial class Опрограмме : Form
    {
        public Опрограмме()
        {
            InitializeComponent();

            StreamReader fs = new StreamReader(@"Опрограмме.txt", Encoding.GetEncoding(1251));
            //string s = Properties.Resources.About;
            //StreamReader fs = new StreamReader(s);
            //
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //label1.BackColor = Color.Transparent;

            string s = "";
            label1.Text = s;
            while (s!=null)
            {
                s = fs.ReadLine();
                label1.Text += s+Environment.NewLine;
            }
        }
    }
}
