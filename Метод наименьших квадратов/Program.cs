using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Метод_наименьших_квадратов
{
    public static class Program
    {
        public static Form1 FORM;
        public static Данные_об_аппроксимации DATA;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FORM = new Form1();
            DATA = new Данные_об_аппроксимации();
            Application.Run(FORM);
        }
        public static void Ap()
        {
            FORM = new Form1();
            DATA = new Данные_об_аппроксимации();
            FORM.ShowDialog();
        }
    }
}
