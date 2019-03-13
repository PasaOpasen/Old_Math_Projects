using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы2018;

namespace SystAnalys_lr1
{
    public static class Program
    {
        public static Form1 FORM;
        public static CheckList CHECK;
        public static A MATRIX;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FORM = new Form1();
            CHECK = new CheckList();
            MATRIX = new A();
            //Application.Run(new Form1());
            Application.Run(FORM);
        }
        public static void Ap()
        {
            FORM = new Form1();
            CHECK = new CheckList();
            MATRIX = new A();
            //Application.Run(new Form1());
            FORM.ShowDialog();
        }
    }
}
