using Apparat.Services;
using Apparat.Services.Interfaces;
using Apparat.ViewModel;
using Apparat.ViewModel.Interfaces;
using Data.Connect;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;
using WinObserver.Service;
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
        const string logPath = @"Files\log\log-.txt";

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                 .ConfigureLogging(builder =>
                 {
                     LoggerConfiguration loggerConfigure = new LoggerConfiguration()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Warning();

                     builder.ClearProviders();
                     builder.AddSerilog(loggerConfigure.CreateLogger());

                 })
                 .ConfigureServices(services =>
                 {
                     services.AddDbContext<ApplicationSettingContext>();
                     services.AddDbContext<ApplicationSettingFolderandHostContext>();
                     services.AddSingleton<IApplicationViewModel, ApplicationViewModel>();
                     services.AddSingleton<IAppSettingService, AppSettingService>();
                     services.AddSingleton<IAppSettingRepository, AppSettingRepository>();
                     services.AddScoped<ISaveStateFolderService, SaveStateFolderService>();
                     services.AddSingleton<IFolderAndHostLeftPanelRepository, FolderAndHostLeftPanelRepository>();
                     services.AddScoped<IHostViewModel, HostViewModel>();
                     services.AddScoped<ITracertService, TracertService>();

                     services.AddSingleton<MainWindow>();
                 })
                 .Build();
        }

        // Global errors.
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {   
            if(e.Exception.Source.ToString() == "System.Private.CoreLib")
            {
                e.Handled = true;
            }
            else
            {
                MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message + " " + e.Exception.Source, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
            }
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
