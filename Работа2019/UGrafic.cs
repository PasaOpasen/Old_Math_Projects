using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using МатКлассы2018;
using static МатКлассы2018.Number;
using Библиотека_графики;
using static Functions;
using Работа2019;
using Point = МатКлассы2018.Point;
using МатКлассы2019;

namespace Практика_с_фортрана
{
    public partial class UGrafic : Form
    {
        public UGrafic()
        {

            InitializeComponent();
            label8.BackColor = Color.Transparent;
            button7.Hide();

            colorDialog1.FullOpen = true;
            colorDialog1.Color = Color.Green;
            ForChart.SetToolTips(ref chart1);
            listBox1.SelectedItem = "По лучу от центра окружности через точку";
            timer1.Interval = 1800;
            timer1.Tick += new EventHandler(Timer1_Tick);

            stophide();

            timer2 = new Timer() { Interval = 1800 };
            timer2.Tick += new EventHandler(GetLenDic);

            toolStripStatusLabel1.Text = "Ожидание команды";

            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].IsVisibleInLegend = false;
            }
            label9.Hide();
            numericUpDown2.Hide();
        }

       public System.Threading.CancellationTokenSource source;

        public void stophide()
        {
            label8.Hide();
            button8.Hide();
        }
        public void stopshow()
        {
            label8.Show();
            button8.Show();
        }


        public int[] prbar;
        public int many;
        public void IsManyPRMSN()
        {
            int s = 0;
            switch (РабКонсоль.NodesCount)
            {
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK15:
                    s = 15;
                    break;
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK21:
                    s = 21;
                    break;
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK31:
                    s = 31;
                    break;
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK41:
                    s = 41;
                    break;
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK51:
                    s = 51;
                    break;
                case FuncMethods.DefInteg.GaussKronrod.NodesCount.GK61:
                    s = 61;
                    break;
            }
            many = 13000 * s / 31;
            if (prmsnmem.Lenght >= many)
            {
                errorProvider1.SetError(button8, "Достаточно много значений PMRSN");
            }
        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            if (prbar.Sum() > 0)
            {
                toolStripStatusLabel1.Text = $"Начальная мемоизация завершена. Считаются значения функции u. Сохранено значений PMRSN: {prmsnmem.Lenght}. Осталось найти примерно {prbar.Length - prbar.Sum()} точек";
                timer2.Stop();
                IsManyPRMSN();
            }

            progressBar1.Value = (prbar.Sum().ToDouble() / prbar.Length * progressBar1.Maximum).ToInt();
        }
        private void GetLenDic(object Sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"Мемоизация PMRSN. Сохранено значений: {prmsnmem.Lenght}";
        }

        double[] xval = new double[0], uRval = new double[0], uIval = new double[0], umodval = new double[0];
        double[] xvalz = new double[0], uRz = new double[0], uIz = new double[0], uAz = new double[0];
        int width = 3; Color color = Color.Blue;
        double fix, beg, end;
        public Func<double, double, double, double, Point,Point/*[]*/, Complex[]> uj = (double x, double y, double z, double w,Point center, Point/*[]*/ normal) =>
        {//u(x,y,z,w,normal)->ux,uy,uz
                   x -= center.x;
                   y -= center.y;
                   //CVectors sum = new CVectors(new Complex[] { normal[0].x, normal[0].y, 0 });
                   //for (int i = 1; i < normal.Length; i++)
                   //    sum += new CVectors(new Complex[] { normal[i].x, normal[i].y, 0 });
            CVectors Q = new CVectors(new Complex[] { normal.x, normal.y, 0 });
            //подынтегральная функция
            FuncMethods.DefInteg.GaussKronrod.ComplexVectorFunc tmp = (Complex a, int n) =>
               {
                   //CSqMatrix G = K(a, x, y, z, w);
                   if (a.Abs == 0) return new Complex[]{1,1,0 };
                   return (K(a, x, y, z, w)*Q*a).ComplexMas;
                   
                   //CVectors t =(a==0)?new CVectors(3):K(a, x, y, z, w) * Q * a;//t.Show();
                   //return (K(a, x, y, z, w) * Q * a).ComplexMas;
               };
            //массив полюсов, может лучше убрать
            //Vectors poles= Roots.OtherMethod((Complex cc)=>Deltass(cc,w), РабКонсоль.polesBeg, РабКонсоль.polesEnd, РабКонсоль.steproot, РабКонсоль.eps, МатКлассы2019.Roots.MethodRoot.Brent, false);
            //poles.UnionWith(DeltassNPosRoots(w, РабКонсоль.polesBeg, РабКонсоль.polesEnd));

            Vectors poles = PolesPoles(w);
            double min = poles.Min*0, max = poles.Max*2;
            //интеграл
            return FuncMethods.DefInteg.GaussKronrod.DINN5_GK(tmp, min, min, min, max, РабКонсоль.tm, РабКонсоль.tp, РабКонсоль.eps, РабКонсоль.pr, РабКонсоль.gr, 3, РабКонсоль.NodesCount).Div(2 * Math.PI);
        };

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void UGrafic_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            switch (i)
            {
                case 0:
                    label5.Show();
                    textBox6.Show();
                    label3.Show();
                    textBox2.Show();
                    textBox3.Show();
                    label7.Show();
                    textBox7.Show();
                    break;
                case 1:
                    label5.Show();
                    textBox6.Show();
                    if (textBox2.Text.ToDouble() == 0) textBox2.Text = РабКонсоль.polesBeg.ToString();
                    label3.Show();
                    textBox2.Show();
                    textBox3.Show();
                    label7.Hide();
                    textBox7.Hide();
                    break;
                default:
                    label5.Hide();
                    textBox6.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    label7.Show();
                    textBox7.Show();
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new ParametrsQu().Show();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Запущен диалог относительно 3D графика";
            //timer1.Start();timer2.Start();
            //prbar = new int[1];
            new _3Duruz().ShowDialog();
            //timer1.Stop(); timer2.Stop();
            //toolStripStatusLabel1.Text = "Вычисления выполнены и записаны в файл. Запущен script из R";
        }

        string message = "При неправильном выборе параметра tm или отрезка обхода полюсов интеграл может проходить через полюс (либо слишком близко), из-за чего происходят NaN от деления на 0 или бесконечности от суммирования больших чисел." + Environment.NewLine +
            "В этом случае метод интегрирования дробит шаг и начинает считать намного больше значений, в следствие этого значений PMRSN сохраняется слишком много. Из-за близости полюсов программа работает в разы дольше, а в конечных данных появляются выбросы." + Environment.NewLine +
            "В этом случае, опираясь на число PMRSN, следует прервать программу и изменить параметры интегрирования. Число значений PMRSN (при фикс. частоте и GK31) бывает: хорошее (3-4к), среднее (8-10к), плохое (>12к). Программу однозначно следует прерывать при числе PMRSN, большем 16к, если только не взята большая размерность. Если частота изменяется, указанные числа умножаются на количество разных частот, а при изменении GK - на соответсвующее отношение." + Environment.NewLine +
            "Этот способ поможет только 1 раз, так как сохранённые значения не удаляются (зачем?). Данные сотрутся автоматически при изменении условий задачи";
        private void label8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(message, "Когда отменять асинхронную операцию?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        public void SourceKill()=>source.Cancel();
        private void button8_Click(object sender, EventArgs e)
        {
            SourceKill();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked || checkBox6.Checked)) { checkBox2.Checked = true; checkBox5.Checked = true; }
            ReDraw();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radio();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radio();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Чтение данных и генерация переменных";
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
                chart1.Series[i].IsVisibleInLegend = false;
            }

            //simpleSound.Stop();
            button3.Show();

            //РабКонсоль.SetPolesDef();

            int k = Convert.ToInt32(numericUpDown1.Value);
            xval = new double[k]; uRval = new double[k]; uIval = new double[k]; umodval = new double[k];
            xvalz = new double[k]; uRz = new double[k]; uIz = new double[k]; uAz = new double[k];

            double cor = textBox6.Text.ToDouble() * Math.PI / 180;
            beg = textBox2.Text.ToDouble();
            end = textBox3.Text.ToDouble();
            double r = textBox5.Text.ToDouble();
            Point center = new Point(textBox1.Text.ToDouble(), textBox4.Text.ToDouble());
            Waves.Circle circle = new Waves.Circle(center, r);
            double h = (end - beg) / (k - 1);
            Waves.Normal2D[] norms = circle.GetNormalsOnCircle(k);
            Waves.Normal2D N = circle.GetNormal(cor);
            double w = textBox7.Text.ToDouble();

            //массив нормалей
            //int NN = Convert.ToInt32(numericUpDown2.Value);
            //Point[] Nmas = Waves.Normal2D.NormalsToPoins(circle.GetNormalsOnCircle(NN));

            prbar = new int[k]; timer1.Enabled = true;
            int ind = listBox1.SelectedIndex;
            toolStripStatusLabel1.Text = "Мемоизация PMRSN (занимает неопределённое время)"; timer2.Start();
            DateTime t1 = DateTime.Now;

            stopshow();

            source = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = source.Token;
            await Task.Run(() =>
            {
                switch (ind)
                {
                    case 0:
                        Parallel.For(0, k, (int i) =>
                        {
                            if (token.IsCancellationRequested) { toolStripStatusLabel1.Text = "Асинхронная операция была отменена"; return; }
                            xval[i] = beg + i * h;
                            Complex[] tmp = uj(N.Position.x + N.n.x * xval[i], N.Position.y + N.n.y * xval[i], 0, w, center,/*Nmas*/N.n);
                            Complex ur = Complex.Sqrt(tmp[0].Sqr() + tmp[1].Sqr());
                            Complex uz = tmp[2];
                            uRval[i] = ur.Re; uIval[i] = ur.Im; umodval[i] = ur.Abs;
                            uRz[i] = uz.Re; uIz[i] = uz.Im; uAz[i] = uz.Abs;
                            prbar[i] = 1;
                        });
                        //chart1.Titles[0].Text = $"(x,y): [{N.Position.x}; {N.Position.x + N.n.x * xval[k-1]}]...[{N.Position.y}; {N.Position.y + N.n.y * xval[k - 1]}], z = 0, w = {w}";
                        break;
                    case 1:
                        Parallel.For(0, k, (int i) =>
                        {
                            if (token.IsCancellationRequested) { toolStripStatusLabel1.Text = "Асинхронная операция была отменена"; return; }
                            xval[i] = beg + i * h;
                            Complex[] tmp = uj(N.Position.x , N.Position.y , 0, xval[i],center, /*Nmas*/N.n);
                            Complex ur = Complex.Sqrt(tmp[0].Sqr() + tmp[1].Sqr());
                            Complex uz = tmp[2];
                            uRval[i] = ur.Re; uIval[i] = ur.Im; umodval[i] = ur.Abs;
                            uRz[i] = uz.Re; uIz[i] = uz.Im; uAz[i] = uz.Abs;
                            prbar[i] = 1;
                        });
                        //chart1.Titles[0].Text = $"(x,y) = ({N.Position.x}; {N.Position.y}), z = 0, w = {beg} ... {end}";
                        break;
                    ////////////
                    default://тут надо хорошо подумать!!!!!!!!
                        beg = 0;
                        end = 2 * Math.PI;
                        //Complex[][] mm = new Complex[k][];
                        Parallel.For(0, k, (int i) =>
                        {
                            if (token.IsCancellationRequested) { toolStripStatusLabel1.Text = "Асинхронная операция была отменена"; return; }
                            xval[i] = beg + i * h;
                            Complex[] tmp = uj(norms[i].Position.x, norms[i].Position.y, 0, w,center,norms[i].n/* Waves.Normal2D.NormalsToPoins(norms)*/);
                            Complex ur = Complex.Sqrt(tmp[0].Sqr() + tmp[1].Sqr());
                            Complex uz = tmp[2];
                            uRval[i] = ur.Re; uIval[i] = ur.Im; umodval[i] = ur.Abs;
                            uRz[i] = uz.Re; uIz[i] = uz.Im; uAz[i] = uz.Abs;
                        //mm[i] = tmp;
                        prbar[i] = 1;
                        });
                        //chart1.Titles[0].Text = $"(x,y) in ({center}; {r}), z = 0, w = {w}";
                        break;
                }
            });
            stophide();

            switch (ind)
            {
                case 0:
                    chart1.Titles[0].Text = $"(x,y): [{N.Position.x}; {N.Position.y}]...[{N.Position.x + N.n.x * xval[k - 1]}; {N.Position.y + N.n.y * xval[k - 1]}], z = 0, w = {w}";
                    break;
                case 1:
                    chart1.Titles[0].Text = $"(x,y) = ({N.Position.x}; {N.Position.y}), z = 0, w = {beg} ... {end}";
                    break;
                ////////////
                default://тут надо хорошо подумать!!!!!!!!
                    chart1.Titles[0].Text = $"(x,y) in ({center}; {r}), z = 0, w = {w}";
                    break;
            }

            timer2.Stop();
            timer1.Stop();
            toolStripStatusLabel1.Text = $"Вычисления закончены. Время: {DateTime.Now - t1}";
            Expendator2.WriteInFile($"ur, uz (last)", new Vectors(xval), new Vectors(uRval), new Vectors(uIval), new Vectors(umodval), new Vectors(uRz), new Vectors(uIz), new Vectors(uAz));
            pictureBox1.Hide();
            ReDraw();
        }

        private void ReDraw()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
                chart1.Series[i].IsVisibleInLegend = false;
            }

            var list = new List<double>();
            if (checkBox1.Checked) { chart1.Series[0].Points.DataBindXY(xval, uRval); chart1.Series[0].IsVisibleInLegend = true; list.AddRange(uRval); }
            if (checkBox2.Checked) { chart1.Series[2].Points.DataBindXY(xval, umodval); chart1.Series[2].IsVisibleInLegend = true; list.AddRange(umodval); }
            if (checkBox3.Checked) { chart1.Series[1].Points.DataBindXY(xval, uIval); chart1.Series[1].IsVisibleInLegend = true; list.AddRange(uIval); }
            if (checkBox4.Checked) { chart1.Series[4].Points.DataBindXY(xval, uIz); chart1.Series[4].IsVisibleInLegend = true; list.AddRange(uIz); }
            if (checkBox5.Checked) { chart1.Series[5].Points.DataBindXY(xval, uAz); chart1.Series[5].IsVisibleInLegend = true; list.AddRange(uAz); }
            if (checkBox6.Checked) { chart1.Series[3].Points.DataBindXY(xval, uRz); chart1.Series[3].IsVisibleInLegend = true; list.AddRange(uRz); }

            //simpleSound.PlayLooping();
            if (list.Count > 0)
            {
                double max = list.Max(), min = list.Min(), t = 0.05;
                chart1.ChartAreas[0].AxisY.Minimum = (min > 0) ? min * (1 - t) : min * (1 + t);
                chart1.ChartAreas[0].AxisY.Maximum = (max > 0) ? max * (1 + t) : max * (1 - t);
            }

        }



        private void button5_Click(object sender, EventArgs e)
        {
            new DINN5().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = $"Функции ur,uz";
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить рисунок как...";
            savedialog.FileName = name;
            savedialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";

            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(savedialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Рисунок не сохранён", ee.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            this.color = colorDialog1.Color;
            chart1.BackColor = this.color;

            //chart1.Series[0].Color = colorDialog1.Color;
            //if (uRval != null) { chart1.Series[0].Points.Clear(); chart1.Series[0].Points.DataBindXY(xval, uRval); }
        }

        SoundPlayer simpleSound = new SoundPlayer(@"1.wav");

        private void button2_Click(object sender, EventArgs e)
        {
            simpleSound.Stop();
            this.Close();
        }

        private void radio()
        {
            if (radioButton1.Checked)
            {
                button7.Hide();
                listBox1.Show();
                label5.Show();
                textBox6.Show();
                label7.Show();
                textBox7.Show();
                label3.Show();
                textBox2.Show();
                textBox3.Show();
                label4.Show();
                numericUpDown1.Show();
                //label9.Show();
                //numericUpDown2.Show();
            }
            else
            {
                button7.Show();
                listBox1.Hide();
                label5.Hide();
                textBox6.Hide();
                label7.Hide();
                textBox7.Hide();
                label3.Hide();
                textBox2.Hide();
                textBox3.Hide();
                label4.Hide();
                numericUpDown1.Hide();
                //label9.Hide();
                //numericUpDown2.Hide();
            }
        }
    }
}
