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

        }

        private void DG1_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (Visibility == Visibility.Visible)
            //{
            //    DG1.Items.Refresh();
            //    //CollectionViewSource.GetDefaultView(DG1.ItemsSource).Refresh();

            //}
            //else
            //{
            //    DG1.Items.Refresh();
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //DG1.Items.Refresh();
            //CollectionViewSource.GetDefaultView(DG1.ItemsSource).Refresh();
        }
    }
}
