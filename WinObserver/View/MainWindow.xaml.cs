using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WinObserver.Model;
using WinObserver.ViewModel;

namespace WinObserver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ApplicationViewModel();

            //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            //double[] data2 = new double[] { 12, 24, 29, 46, 25 };
            //WpfPlot1.Plot.AddScatter(dataX, dataY);
            //WpfPlot1.Plot.AddScatter(dataX, data2);
            //WpfPlot1.Refresh();
        }
    }
}
