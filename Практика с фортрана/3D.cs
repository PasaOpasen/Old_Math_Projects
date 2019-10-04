using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;

namespace Практика_с_фортрана
{
    public partial class _3D : Form
    {
        public _3D()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double x0, X, z0, Z;
            int x, z;

            x0 = Convert.ToDouble(textBox1.Text);
            X = Convert.ToDouble(textBox2.Text);
            z0 = Convert.ToDouble(textBox3.Text);
            Z = Convert.ToDouble(textBox4.Text);
            x = Convert.ToInt32(numericUpDown1.Value);
            z = Convert.ToInt32(numericUpDown2.Value);

            РабКонсоль.SetPoles(РабКонсоль.polesBeg,РабКонсоль.polesEnd,РабКонсоль.steproot,РабКонсоль.epsroot,РабКонсоль.countroot);

            Func<double,double,double>f= (double a, double b) => РабКонсоль.uRes(a, b).Abs;
            if(radioButton2.Checked) f = (double a, double b) => РабКонсоль.uRes(a, b).Re;
            if (radioButton3.Checked) f = (double a, double b) => РабКонсоль.uRes(a, b).Im;

            ИнтеграцияСДругимиПрограммами.CreatTableInExcel(new DRealFunc( f), x0, X, x, z0, Z, z);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }
    }
}
