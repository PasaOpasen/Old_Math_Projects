using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СЛАУ
{
    public static class Program
    {
        public static SLAUform F;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            F = new SLAUform();
            Application.Run(F);
        }
        public static void Ap()
        {
            /*var*/ F = new SLAUform();
            F.ShowDialog();
        }
    }
}
