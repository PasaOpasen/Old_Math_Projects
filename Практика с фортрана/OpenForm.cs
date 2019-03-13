using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика_с_фортрана
{
    public partial class OpenForm : Form
    {
        public OpenForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new UGrafic().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new wGrafic().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new kGrafic().Show();
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
            //new GrForm().Show();
            РабКонсоль.GRFORM = new GrForm();
            РабКонсоль.GRFORM.Show();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Energy().Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new _3D().Show();
        }
    }
}
