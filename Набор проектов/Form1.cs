using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Интегралы;
using Истинное_значение_величины;
using МатКлассы;
using Критерии_принятия_решений_в_условиях_неопределённости;
using Метод_наименьших_квадратов;
using Полином_Эрмита;
using Оконное_приложение;
using СЛАУ;
using Симплекс_метод;
using SystAnalys_lr1;


namespace Набор_проектов
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 400;
            toolTip1.ShowAlways = true;

            button12.Hide();

            toolTip1.SetToolTip(this.button2, "Вычисление интегралов от действительных функций разными методами");
            toolTip1.SetToolTip(this.button10, "Решение системы линейных алгебраических уравнений разными методами");
            toolTip1.SetToolTip(this.button7, "Решение классической задачи линейного программирования симплекс-методом");
            toolTip1.SetToolTip(this.button9, "Поиск основных характеристик неориентированных графов");
            toolTip1.SetToolTip(this.button4, "Аппроксимация методом наименьших квадратов");
            toolTip1.SetToolTip(this.button6, "Интерполяция по кратным узлам");
            toolTip1.SetToolTip(this.button8, "Интерполяция полиномами и сплайнами");
            toolTip1.SetToolTip(this.button3, "Исследование характеристик случайной величины");
            toolTip1.SetToolTip(this.button5, "Простейшее использование критериев принятия решений в условиях неопределённости");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Интегралы.Дм.ПА.Program.Ap();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            СЛАУ.Program.Ap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Симплекс_метод.Program.Ap();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SystAnalys_lr1.Program.Ap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Метод_наименьших_квадратов.Program.Ap();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Полином_Эрмита.Program.Ap();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Оконное_приложение.Program.Ap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Истинное_значение_величины.Дм.ПА.Program.Ap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Критерии_принятия_решений_в_условиях_неопределённости.Program.Ap();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new Задача_Штурма_Лиувилля.Form1().Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            new КодировкаДекодировка.Coder().Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            new Курсач.MyForm().Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new Решение_уравнения_теплопроводности.Дм.ПА.Shoot().Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Практика_с_фортрана.РабКонсоль.Main(new string[0]);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            new Явная_и_неявная_схема.Дм.ПА.Form1().Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            new Сжатие_изображения_DCT.Compress().Show();
        }
    }
}
