using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Интегралы.Дм.ПА
{
    public static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PER = new Perc();
            G = new IntegForm();
            Application.Run(G);
        }
       public static Perc PER;
        public static IntegForm G;
        public static void Ap()
        {
            PER = new Perc();
            G = new IntegForm();
            G.ShowDialog();
        }
    }
}
