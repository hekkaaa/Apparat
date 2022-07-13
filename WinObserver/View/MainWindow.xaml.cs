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
        ILogger<MainWindow> _logger;

        public MainWindow(IApplicationViewModel applicationViewModel, ILogger<MainWindow> logger)
        {
            InitializeComponent();
            _logger = logger;
            _logger.LogWarning("Init MainWindows object");
            _applicationViewModel = applicationViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _applicationViewModel;

            _logger.LogWarning("General Windows Start", "Done");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}"));
            e.Handled = true;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _logger.LogWarning("Application is closed! Goodbye!");
        }
    }
}
