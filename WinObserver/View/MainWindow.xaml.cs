using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}"));
            e.Handled = true;
        }

        private void TextBlockGithub_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = true;
            PopUpText.Text = "Link to the author's github.";
        }

        private void TextBlockGithub_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = false;
        }

        private void TextBlockAppVersion_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = true;
            PopUpText.Text = "This version of the program may crash unexpectedly and give errors.";
        }

        private void TextBlockAppVersion_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = false;
        }

        private void GetIp_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = true;
            PopUpText.Text = "Enter hostname.";
        }

        private void GetIp_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PopUpGithub.IsOpen = false;
        }
    }
}
