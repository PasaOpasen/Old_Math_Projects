using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Критерии_принятия_решений_в_условиях_неопределённости
{
    public static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Критерии());
        }
        public static void Ap()
        {
            var F = new Критерии();
            F.ShowDialog();
        }
    }
}
