using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Оконное_приложение
{
    public static class Program
    {
        public static Form1 form;
        public static Data data;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            form = new Form1();
            data = new Data();
            //Application.Run(form);
            form.ShowDialog();
        }
        public static void Ap()
        {
            form = new Form1();
            data = new Data();
            //Application.Run(form);
            form.ShowDialog();
        }
    }
}
