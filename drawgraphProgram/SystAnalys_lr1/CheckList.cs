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
using МатКлассы2018;
using static МатКлассы2018.Graphs;

namespace SystAnalys_lr1
{
    public partial class CheckList : Form
    {
        public CheckList()
        {
            InitializeComponent();
            checkBox1.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox11.Checked = true;
            checkBox12.Checked = true;
            checkBox14.Checked = true;
            radioButton2.Checked = true;
            radioButton3.Checked = true;
            groupBox2.Hide();
        }

        private void InfoError(int t, Exception e)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine("НЕ УДАЛОСЬ (окончательно) ВЫПОЛНИТЬ ПУНКТ {0} ВВИДУ ИСКЛЮЧИТЕЛЬНОЙ СИТУАЦИИ", t);
            Console.WriteLine("Причина исключения: " + e.Message);
            Console.WriteLine("--------------------------");
            Console.WriteLine();
        }
        private int tmp = 0;
        DateTime StartTime, EndTime;
        private void InfoTime(int i)
        {
            Console.WriteLine("///////////////Время работы процедуры " + i + ": " + (EndTime - StartTime));
            Console.WriteLine();
        }

        public class TextBoxStreamWriter : TextWriter
        {
            TextBox _output = null;

            public TextBoxStreamWriter(TextBox output)
            {
                _output = output;
            }

            public override void Write(char value)
            {
                base.Write(value);
                _output.AppendText(value.ToString()); // Когда символ записывается в поток, добавляем его в textbox.
            }

            public override Encoding Encoding
            {
                get { return System.Text.Encoding.UTF8; }
            }
        }

        private /*async*/ void button1_Click(object sender, EventArgs e)
        {
            const string message = "Операции могут занять от нескольких секунд до нескольких минут ввиду долгого поиска всех вершинных и рёберных покрытий, паросочетаний, хроматического полинома и эйлеровых циклов; длительность работы зависит от количества вершин и количества рёбер в графе. По окончанию всех операций приложение выдаст результат и будет готово продолжать работу";
            const string caption = "ПРЕДУПРЕЖДЕНИЕ";

            if (checkBox13.Checked || checkBox11.Checked || checkBox10.Checked || checkBox14.Checked)
            {
                var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Hide(); 

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);

            
            if (radioButton4.Checked)
            {
                Console.SetOut(new TextBoxStreamWriter(Program.FORM.textBox1));
                Program.FORM.textBox1.Show();
                Program.FORM.textBox1.BringToFront();
                //Program.FORM.Hide();
                //Program.FORM.Focus();
                //Program.FORM.BringToFront();
                Program.FORM.Show();
            }
            else
            {
                Console.SetOut(sw);
                Program.FORM.Show();
            }


            //this.Close();
            //g.ShowInfoConsole();

            Program.FORM.g.ShowCheck0();
            if (checkBox1.Checked)
                try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck1();
                    EndTime = DateTime.Now;
                    InfoTime(1);
                }
                catch (Exception ex) { InfoError(1, ex); }
            if (checkBox2.Checked) try
                {
                    StartTime = DateTime.Now;

                    Program.FORM.g.ShowCheck2();

                    EndTime = DateTime.Now;
                    InfoTime(2);

                }
                catch (Exception ex) { InfoError(2, ex); }
            if (checkBox3.Checked)
                try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck3();
                    EndTime = DateTime.Now;
                    InfoTime(3);
                }
                catch (Exception ex) { InfoError(3, ex); }
            if (checkBox4.Checked)
                try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck4();
                    EndTime = DateTime.Now;
                    InfoTime(4);
                }
                catch (Exception ex) { InfoError(4, ex); }
            if (checkBox5.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck5();
                    EndTime = DateTime.Now;
                    InfoTime(5);
                }
                catch (Exception ex) { InfoError(5, ex); }
            if (checkBox6.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck6();
                    EndTime = DateTime.Now;
                    InfoTime(6);
                }
                catch (Exception ex) { InfoError(6, ex); }
            if (checkBox7.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck7();
                    EndTime = DateTime.Now;
                    InfoTime(7);
                }
                catch (Exception ex) { InfoError(7, ex); }
            if (checkBox8.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck8();
                    EndTime = DateTime.Now;
                    InfoTime(8);
                }
                catch (Exception ex) { InfoError(8, ex); }
            if (checkBox9.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck9();
                    EndTime = DateTime.Now;
                    InfoTime(9);
                }
                catch (Exception ex) { InfoError(9, ex); }
            if (checkBox10.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck10();
                    EndTime = DateTime.Now;
                    InfoTime(10);
                }
                catch (Exception ex) { InfoError(10, ex); }
            if (checkBox11.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck11();
                    EndTime = DateTime.Now;
                    InfoTime(11);
                }
                catch (Exception ex) { InfoError(11, ex); }
            if (checkBox12.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck12();
                    EndTime = DateTime.Now;
                    InfoTime(12);
                }
                catch (Exception ex) { InfoError(12, ex); }
            if (checkBox13.Checked)
            {
                //var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                try
                {
                    StartTime = DateTime.Now;
                    if (radioButton2.Checked) Program.FORM.g.ShowCheck13();
                    else Program.FORM.g.ShowCheck13Full();
                     //await Task.Run(()=>Program.FORM.g.ShowCheck13Full());
                    EndTime = DateTime.Now;
                    InfoTime(13);
                }
                catch (Exception ex) { InfoError(13, ex); }
            }
            if (checkBox14.Checked) try
                {
                    StartTime = DateTime.Now;
                    Program.FORM.g.ShowCheck14();
                    EndTime = DateTime.Now;
                    InfoTime(14);
                }
                catch (Exception ex) { InfoError(14, ex); }

            if (radioButton4.Checked) InFile(sw, Program.FORM.textBox1);
            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            this.Close();
        }

        private void InFile(StreamWriter w, TextBox T)
        {
            string s = "";
            s = T.Text;
            w.Write(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;
            checkBox12.Checked = false;
            checkBox13.Checked = false;
            checkBox14.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox11.Checked = true;
            checkBox12.Checked = true;
            checkBox13.Checked = true;
            checkBox14.Checked = true;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            tmp++;
            if (tmp % 2 == 0) { groupBox2.Hide(); tmp = 0; }
            else groupBox2.Show();
        }
    }
}
