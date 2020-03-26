using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Явная_и_неявная_схема.Дм.ПА
{
    static class Program
    {

        public static Form1 F1;
        public static Form2 F2;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            F1 = new Form1();
            Application.Run(F1);
        }
    }
}
