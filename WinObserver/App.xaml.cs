using Apparat.Services;
using Apparat.Services.Interfaces;
using Data.Connect;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WinObserver.ViewModel;

namespace WinObserver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                 .ConfigureServices(services =>
                 {
                     services.AddDbContext<ApplicationSettingContext>();
                     services.AddSingleton<IApplicationViewModel, ApplicationViewModel>();
                     services.AddSingleton<IAppSettingService, AppSettingService>();
                     services.AddSingleton<IAppSettingRepository, AppSettingRepository>();
                     services.AddSingleton<MainWindow>();
                 })
                 .Build();
        }

        // Global errors.
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
