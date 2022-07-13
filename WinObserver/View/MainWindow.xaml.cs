using Apparat.Services.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
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
        IApplicationViewModel _applicationViewModel;
        ILogger<MainWindow> _log;

        public MainWindow(IApplicationViewModel applicationViewModel, ILogger<MainWindow> log)
        {
            InitializeComponent();
            _log = log;
            _log.LogWarning("Init MainWindows object");
            _applicationViewModel = applicationViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _applicationViewModel;
            _log.LogWarning("General Windows Start", "Done");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}"));
            e.Handled = true;
        }
    }
}
