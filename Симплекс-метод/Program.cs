using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Симплекс_метод
{
    public static class Program
    {
        public static SimplexTab FORM;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FORM = new SimplexTab();
            Application.Run(FORM);
        }
        public static void Ap()
        {
            FORM = new SimplexTab();
            FORM.ShowDialog();
        }
    }
}
