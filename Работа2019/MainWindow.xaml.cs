using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using МатКлассы2018;
using МатКлассы2019;
using TekVISANet;

namespace Работа2019
{
    public static class Global
    {
        public static DComplexFunc RRToC;
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new kGrafic().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //new Практика_с_фортрана.UGrafic().Show();
            Forms.UG = new Практика_с_фортрана.UGrafic();
            Forms.UG.Show();
        }
    }
}
