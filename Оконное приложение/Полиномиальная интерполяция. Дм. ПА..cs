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
using System.IO;
using Point = МатКлассы.Point;

namespace Оконное_приложение
{
    public partial class Form1 : Form
    {
        static int ind = 0,ind2=0,ind3=0;
        public RealFunc N, L, R, S,f=null;

        public Form1()
        {
            InitializeComponent();
            L = null; N = null; R = null; S = null;
            chart1.Series[0].Name = "";
            chart1.Series[1].Name = " ";
            chart1.Series[2].Name = "  ";
            chart1.Series[3].Name = "   ";
            chart1.Series[4].Name = "     ";

            radioButton2.Checked = true;
            radioButton4.Checked = true;
            checkBox1.Checked = true;
            checkBox4.Checked = true;
            label3.Hide();
            textBox4.Text = "1";
            textBox4.Hide();
            textBox5.Hide();
            textBox6.Hide();
            textBox7.Hide();
            label4.Hide();
            button4.Hide();
            //label5.Hide();
            //checkBox5.Hide();
            firstRowNum.Hide(); label7.Hide();

            textBox1.Text = "3"; textBox2.Text = "-2"; textBox3.Text = "2";
            textBox5.Text = "4 2"+Environment.NewLine+"5 1"+Environment.NewLine+"6 0"+ Environment.NewLine;
            textBox6.Text = "-2"; textBox7.Text = "2";
            textBox8.Text = textBox9.Text = "0";
        }

        private static void SHOWS(SLAU S)
        {
            int size = S.Size;
            for (int i = 0; i < size; i++)
            {
                string s = "";
                s+="||";
                for (int j = 0; j < size - 1; j++) s+=String.Format("{0} \t", S.A[i, j]);
                s+=String.Format("{0}\t|| \t||{1}|| \t{2}", S.A[i, size - 1], S.x[i], S.b[i]);
                Program.data.textBox1.Items.Add(s);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Draw_mas(Point[] r, Color color) //нарисовать массив точек
        {

            //SetColor(R, G, B); // задаем цвет линии
            //if(R==0) 
            //Program.FORM.chart1.Series[0].Color = Color.Green;
            FuncMethods.NetFunc u = new FuncMethods.NetFunc(r);

            int N = r.Length;
            double e = (u.MaxArg - u.MinArg) / 150;
            for (int i = 0; i < N; i++)
            {
                this.chart1.Series.Add("");
                int count = this.chart1.Series.Count - 1;
                this.chart1.Series[count].Color = color;

                this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart1.Series[count].IsVisibleInLegend = false;
                this.chart1.Series[count].BorderWidth = 3;
                this.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y - e); // устанавливаем курсор на точку
                this.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y + e); //рисуем line
                //-----------------------------------------------------------------
                this.chart1.Series.Add("");
                count = this.chart1.Series.Count - 1;
                this.chart1.Series[count].Color = color;

                this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart1.Series[count].IsVisibleInLegend = false;
                this.chart1.Series[count].BorderWidth = 3;
                this.chart1.Series[count].Points.AddXY(r[i].x - e, r[i].y + e); // устанавливаем курсор на точку
                this.chart1.Series[count].Points.AddXY(r[i].x + e, r[i].y - e); //рисуем line
            }
            //Program.FORM.chart1.Series[0].Color = Color.Red;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();
            chart1.Series[4].Points.Clear();

            for (int i = 5; i < this.chart1.Series.Count; i++) { this.chart1.Series.RemoveAt(i); i--; }
            Program.data.textBox1.Items.Clear();//Program.data.textBox1.Text = "";
            Program.data.textBox2.Text = "";

            f = Math.Sin;
            L = null; N = null; R = null; S=null;
            if (radioButton4.Checked) f = (double x) => { return Math.Cos(x) + 1; };
            if (radioButton5.Checked) f = (double x) => { return Math.Sin(x) * x; };
            if (radioButton7.Checked) f = (double x) => { return x * x + 2 * x - 1; };
            if (radioButton9.Checked) f = (double x) => { return Math.Exp(x) * Math.Sin(x); };
            if (radioButton10.Checked) f = (double x) => { return x * Math.Log(1+Math.Abs(x)); };
            if (radioButton11.Checked) f = (double x) => { return 5; };
            if (radioButton12.Checked) f = (double x) => { return x/(1+x*x); };
            if (radioButton13.Checked) f = (double x) => { return Math.Sin(2*x)/(Math.Abs(Math.Cos(x))+Math.Abs(x)+x*x); };
            if (radioButton14.Checked) f = (double x) => { return Math.Sin(x) +x*x; };
            if (radioButton15.Checked) f = (double x) => { return (x*x*x-x*x+2*x-1) / (x*x-2*x+6); };
            if (radioButton16.Checked) f = (double x) => { return Math.Sqrt(Math.Abs(x)); };
            if (radioButton17.Checked) f = (double x) => { return Math.Abs(Math.Sin(x))+Math.Cos(x)-1; };
            if (radioButton18.Checked)
            {
                string s = textBox10.Text;
                try
                {
                    f = Parser.GetDelegate(s);
                    textBox10.Text = Parser.FORMULA;
                }
                catch
                {
                    f = (double x) => 0;
                }
            }

            МатКлассы.Point[] mas = null;
            double min = Convert.ToDouble(textBox6.Text),max= Convert.ToDouble(textBox7.Text);

            if (radioButton1.Checked)
            {
                StreamReader fs = new StreamReader("input.txt");
                mas = МатКлассы.Point.Points(fs);
            }
            if (radioButton2.Checked)
            {
                double a = Convert.ToDouble(textBox2.Text);
                double b = Convert.ToDouble(textBox3.Text);
                int m = Convert.ToInt32(textBox1.Text);
                mas = МатКлассы.Point.Points(f, m-1, a, b);
                if (!checkBox5.Checked) { min = mas[0].x;max = mas[mas.Length - 1].x; }
                chart1.Series[0].Points.DataBindXY(МатКлассы.Point.PointsX(f, 100, min, max), МатКлассы.Point.PointsY(f, 100, min, max));
            }
            if(radioButton6.Checked)
            {
                int m = Convert.ToInt32(textBox1.Text);
                mas = new МатКлассы.Point[m];
                string s;
                string[] st;
                for (int k = 0; k < m; k++)
                {
                    s = textBox5.Lines[k];
                    st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа
                    mas[k] = new МатКлассы.Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
                }
            }
            if(radioButton8.Checked)
            {
                int m = Convert.ToInt32(textBox1.Text);
                mas = new МатКлассы.Point[m];
                string s;
                string[] st;
                for (int k = 0; k < m; k++)
                {
                    s = textBox5.Lines[k];
                    st = s.Split(' ');//в аргументах указывается массив символов, которым разделяются числа
                    double v = Convert.ToDouble(st[0]);
                    mas[k] = new МатКлассы.Point(v, f(v));
                }
                if (!checkBox5.Checked) { min = mas[0].x; max = mas[mas.Length - 1].x; }
                chart1.Series[0].Points.DataBindXY(МатКлассы.Point.PointsX(f, 100, min, max), МатКлассы.Point.PointsY(f, 100, min, max));
            }

            Draw_mas(mas, Color.Orchid);

            chart1.Series[0].Name = "Интерполируемая функция";
            chart1.Series[1].Name = "Полином Лагранжа";
            chart1.Series[2].Name = "Полином Ньютона";
            chart1.Series[3].Name = "Рациональная функция";
            chart1.Series[4].Name = "Сплайн";

            if (!checkBox5.Checked) { min = mas[0].x; max = mas[mas.Length - 1].x; }


            if (checkBox1.Checked)
            {
                Polynom p = new Polynom(mas);

                chart1.Series[1].Points.DataBindXY(МатКлассы.Point.PointsX(p.Value, 100, min, max), МатКлассы.Point.PointsY(p.Value, 100, min, max));
                Program.data.textBox1.Items.Add(String.Format("Интерполяционный полином Лагранжа в степенном базисе: {0}",p.ToString()));
                L = p.Value;
            }
            if (checkBox2.Checked)
            {
                Polynom p = Polynom.Neu(mas);

                chart1.Series[2].Points.DataBindXY(МатКлассы.Point.PointsX(p.Value, 100, min, max), МатКлассы.Point.PointsY(p.Value, 100, min, max));
                Program.data.textBox1.Items.Add("");
                Program.data.textBox1.Items.Add(String.Format("Интерполяционный полином Ньютона в степенном базисе: {0}", p.ToString()));
                N = p.Value;
            }
            if (checkBox4.Checked)
            {
                double a = Convert.ToDouble(textBox9.Text);
                double b = Convert.ToDouble(textBox8.Text);

                S = Polynom.CubeSpline(mas,a,b);

                chart1.Series[4].Points.DataBindXY(МатКлассы.Point.PointsX(S, 100, min, max), МатКлассы.Point.PointsY(S, 100, min, max));

                if(checkBox6.Checked)
                {
                    RealFunc S1 = Polynom.DSpline,S2=Polynom.D2Spline;

                    this.chart1.Series.Add("Первая производная сплайна");
                    int count = this.chart1.Series.Count - 1;
                    this.chart1.Series[count].Color = Color.Aqua;
                    this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    this.chart1.Series[count].BorderWidth = 3;
                    chart1.Series[count].Points.DataBindXY(МатКлассы.Point.PointsX(S1, 100, min, max), МатКлассы.Point.PointsY(S1, 100, min, max));

                    this.chart1.Series.Add("Вторая производная сплайна");
                    count = this.chart1.Series.Count - 1;
                    this.chart1.Series[count].Color = Color.Black;
                    this.chart1.Series[count].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    this.chart1.Series[count].BorderWidth = 3;
                    chart1.Series[count].Points.DataBindXY(МатКлассы.Point.PointsX(S2, 100, min, max), МатКлассы.Point.PointsY(S2, 100, min, max));
                }

                Program.data.textBox1.Items.Add("");
                if (Program.data.checkBox1.Checked)
                {
                    Program.data.textBox1.Items.Add("Полученная система: ");
                    SHOWS(Polynom.syst);
                }
                Program.data.textBox1.Items.Add("Интерполяционный сплайн (по отрезкам): " );
                for(int i=0;i<Polynom.SplinePol.Length;i++) Program.data.textBox1.Items.Add("\t"+Polynom.SplinePol[i]);
            }
            if (checkBox3.Checked)
            {
                //int p = Convert.ToInt32(textBox4.Text);
                int p = Convert.ToInt32(firstRowNum.Value);
                int bq= Convert.ToInt32(textBox4.Text);
                int q = mas.Length - 1 - p;
                R = Polynom.R(mas, p, q,bq);

                chart1.Series[3].Points.DataBindXY(МатКлассы.Point.PointsX(R, 100, min, max), МатКлассы.Point.PointsY(R, 100, min, max));

                Program.data.textBox1.Items.Add("");
                if (Program.data.checkBox1.Checked)
                {
                    Program.data.textBox1.Items.Add("Полученная система: ");
                    SHOWS(Polynom.syst);
                }
                Program.data.textBox1.Items.Add(String.Format("Интерполяционная рациональная функция: {0}", Polynom.Rat));
            }


            button4.Show();


            //double h = 0.2;
            //int n = (int)((10.0 + 10.0) / h), i = 0;
            //double[] xx = new double[n + 5];
            //double[] yy = new double[n + 5];
            //for (double x = -10.0; x <= 10.0; x += h)
            //{
            //    chart1.Series[0].Points.AddXY(x, Math.Cos(x));
            //    xx[i] = x;
            //    yy[i] = Math.Cos(x);
            //    i++;
            //}
            ////chart1.Series[0].Points.D

            ////RealFunc f = Math.Cos, p = Polynom.Derivative(f, 14, -10, 10, 4).Value, g = Polynom.Lag(f, 12, -10, 10).Value;

            //МатКлассы.Point[] mas = new МатКлассы.Point[5];
            //mas[0] = new МатКлассы.Point(0, 1);
            //mas[1] = new МатКлассы.Point(1, -5);
            //mas[2] = new МатКлассы.Point(2, 0);
            //mas[3] = new МатКлассы.Point(3, 1);
            //mas[4] = new МатКлассы.Point(4, 1);
            //RealFunc g = Polynom.CubeSpline(mas);

            //RealFunc f = (double x) => { return x * Math.Sin(x) + 1; };
            //SequenceFunc p = (double x, int n) => { return Polynom.Lezh(n).Value(x); };


            //FuncMethods.Approx(f, p, SequenceFundKind.Orthogonal, 6, -1, 1);

            //chart1.Series[0].Color = Color.Gold;
            //chart1.Series[0].Points.DataBindXY(МатКлассы.Point.PointsX(g, 100, -1, 2), МатКлассы.Point.PointsY(g, 100, -1, 2));
            //chart1.Series[1].Points.DataBindXY(МатКлассы.Point.PointsX(f, 100, -1, 1), МатКлассы.Point.PointsY(f, 100, -1, 1));

            //chart1.SaveImage(@"Image.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.form.Close();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ind++;
            if((ind-1)%2==0)
            { label3.Show(); textBox4.Show(); firstRowNum.Show(); label7.Show(); }
            else { label3.Hide(); textBox4.Hide(); firstRowNum.Hide(); label7.Hide(); }
            ind %= 2;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Hide();
            label2.Hide();
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            groupBox3.Hide();
            label4.Hide();
            label5.Hide();
            textBox5.Hide();
            //textBox6.Text = "-2";
            //textBox7.Text = "3";
            checkBox5.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            groupBox3.Show();
            label4.Hide();
            label5.Hide();
            textBox5.Hide();
            textBox6.Text = textBox2.Text;
            textBox7.Text = textBox3.Text;
            checkBox5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Program.form = new Form1();
            //Program.data = new Data();
            //Application.Run(Program.form);
            //Program.form.ShowDialog();
            Application.Restart();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Parser.INFORMATION, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            label2.Hide();
            textBox1.Show();
            textBox2.Hide();
            textBox3.Hide();
            groupBox3.Hide();
            label4.Show();
            label5.Hide();
            textBox5.Show();
            textBox5.Text = "4 2" + Environment.NewLine + "5 1" + Environment.NewLine + "6 0" + Environment.NewLine;
            textBox6.Text = "4";
            textBox7.Text = "6";
            checkBox5.Show();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            label2.Hide();
            textBox1.Show();
            textBox2.Hide();
            textBox3.Hide();
            groupBox3.Show();
            label4.Hide();
            textBox5.Show();
            label5.Show();
            textBox5.Text = "-2" + Environment.NewLine + "0" + Environment.NewLine + "3" + Environment.NewLine;

            textBox6.Text= "-2";
            textBox7.Text = "3";
            checkBox5.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.SaveImage(@"Image.png", System.Drawing.Imaging.ImageFormat.Png);
            SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                    chart1.SaveImage(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
       
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Program.data.ShowDialog();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ind3++;
            if (ind3 % 2 != 0)
            {
                textBox8.Show();
                textBox9.Show();
                label6.Show();
                checkBox6.Show();
            }
            else
            {
                textBox8.Hide();
                textBox9.Hide();
                label6.Hide();
                checkBox6.Hide();
            }
            ind3 %= 2;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            ind2++;
            if(ind2%2!=0)
            {            textBox6.Show();
                         textBox7.Show();
                //checkBox5.Show();
            }
            else
            {
                textBox6.Hide();
                textBox7.Hide();
                //checkBox5.Hide();
            }
            ind2 %= 2;
        }
    }
}
