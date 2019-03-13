using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Полином_Эрмита
{
    public static class Program
    {
        public static Form1 FORM;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            FORM = new Form1();
            Application.Run(FORM);
        }
        public static void Ap()
        {
            FORM = new Form1();
            FORM.ShowDialog();
        }
    }
}
